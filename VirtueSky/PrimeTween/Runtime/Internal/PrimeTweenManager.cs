// ReSharper disable Unity.RedundantHideInInspectorAttribute
#if PRIME_TWEEN_SAFETY_CHECKS && UNITY_ASSERTIONS
#define SAFETY_CHECKS
#endif
using System;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;
using UnityEngine;
using Debug = UnityEngine.Debug;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PrimeTween {
    [AddComponentMenu("")]
    internal class PrimeTweenManager : MonoBehaviour {
        #if UNITY_EDITOR || SAFETY_CHECKS
            internal static PrimeTweenManager _instance;
            internal static PrimeTweenManager Instance {
                get {
                    if (!HasInstance) {
                        if (Application.isEditor) {
                            CreateInstance();
                            _instance.gameObject.hideFlags = HideFlags.HideAndDontSave;
                        } else {
                            const string error = nameof(PrimeTweenManager) + " is not created yet. Please add the 'PRIME_TWEEN_EXPERIMENTAL' define to your project, then use '" + nameof(PrimeTweenConfig) + "." + nameof(PrimeTweenConfig.ManualInitialize) + "()' to initialize PrimeTween before '" + nameof(RuntimeInitializeLoadType) + "." + nameof(RuntimeInitializeLoadType.BeforeSceneLoad) + "'.";
                            throw new Exception(error);
                        }
                    } /*else if (_instance == null) { // todo this throws if PrimeTween API is called from OnDestroy(). See the DestructionOrderTest scene. How to detect manual PrimeTweenManager destruction?
                        throw new Exception(nameof(PrimeTweenManager) + " was manually destroyed after creation, which is not allowed. Please check you're not destroying all objects manually.");
                    }*/
                    return _instance;
                }
                private set => _instance = value;
            }
        #else
            internal static PrimeTweenManager Instance;
        #endif

        internal static bool HasInstance {
            get {
                #if UNITY_EDITOR || SAFETY_CHECKS
                    return !ReferenceEquals(null, _instance);
                #else
                    return Instance != null;
                #endif
            }
        }

        #if UNITY_EDITOR
            static bool isHotReload = true;
        #endif
        internal static int customInitialCapacity = -1;

        /// Item can be null if the list is accessed from the <see cref="ReusableTween.updateAndCheckIfRunning"/> via onValueChange() or onComplete()
        /// Changing list to array gives about 8% performance improvement and is possible to do in the future
        ///     The current implementation is simpler and PrimeTweenManagerInspector can draw tweens with no additional code
        #if UNITY_2021_3_OR_NEWER
        [ItemCanBeNull]
        #endif
        [SerializeField] internal List<ReusableTween> tweens;
        [SerializeField] internal List<ReusableTween> lateUpdateTweens;
        [SerializeField] internal List<ReusableTween> fixedUpdateTweens;
        [NonSerialized] internal List<ReusableTween> pool;
        /// startValue can't be replaced with 'Tween lastTween'
        /// because the lastTween may already be dead, but the tween before it is still alive (count >= 1)
        /// and we can't retrieve the startValue from the dead lastTween
        internal Dictionary<(Transform, TweenType), (ValueContainer startValue, int count)> shakes;
        internal int currentPoolCapacity { get; private set; }
        internal int maxSimultaneousTweensCount { get; private set; }

        [HideInInspector]
        internal long lastId = 1;
        internal Ease defaultEase = Ease.OutQuad;
        internal _UpdateType defaultUpdateType = _UpdateType.Update;
        internal const Ease defaultShakeEase = Ease.OutQuad;
        internal bool warnTweenOnDisabledTarget = true;
        internal bool warnZeroDuration = true;
        internal bool warnStructBoxingAllocationInCoroutine = true;
        internal bool warnBenchmarkWithAsserts = true;
        internal bool validateCustomCurves = true;
        internal bool warnEndValueEqualsCurrent = true;
        int processedCount;
        int lateUpdateTweensProcessedCount;
        int fixedUpdateTweensProcessedCount;
        int maxLateUpdateCount;
        internal int updateDepth;
        internal static readonly object dummyTarget = new object();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void beforeSceneLoad() {
            if (!HasInstance) {
                CreateInstanceAndDontDestroy();
            }
        }

        internal static void CreateInstanceAndDontDestroy() {
            CreateInstance();
            DontDestroyOnLoad(Instance.gameObject);
        }

        static void CreateInstance() {
            #if UNITY_EDITOR
            isHotReload = false;
            #endif
            GameObject go;
            try {
                go = new GameObject(nameof(PrimeTweenManager));
            } catch (UnityException e) {
                if (e.Message.Contains("is not allowed to be called from a MonoBehaviour constructor")) {
                    const string message = "this PrimeTween's API is not allowed to be called from a MonoBehaviour constructor (or instance field initializer).";
                    throw new Exception(message);
                }
                throw;
            }
            var instance = go.AddComponent<PrimeTweenManager>();
            const int defaultInitialCapacity = 200;
            instance.init(customInitialCapacity != -1 ? customInitialCapacity : defaultInitialCapacity);
            Instance = instance;
        }

        void init(int capacity) {
            tweens = new List<ReusableTween>(capacity);
            lateUpdateTweens = new List<ReusableTween>(capacity);
            fixedUpdateTweens = new List<ReusableTween>(capacity);
            pool = new List<ReusableTween>(capacity);
            for (int i = 0; i < capacity; i++) {
                pool.Add(new ReusableTween());
            }
            shakes = new Dictionary<(Transform, TweenType), (ValueContainer, int)>(capacity);
            currentPoolCapacity = capacity;
        }

        const string manualInstanceCreationIsNotAllowedMessage = "Please don't create the " + nameof(PrimeTweenManager) + " instance manually.";
        void Awake() => Assert.IsFalse(HasInstance, manualInstanceCreationIsNotAllowedMessage);

        #if UNITY_EDITOR
        [InitializeOnLoadMethod]
        static void iniOnLoad() {
            float curTime = (float)EditorApplication.timeSinceStartup;
            EditorApplication.update += () => {
                if (Application.isPlaying) {
                    return;
                }
                float newTime = (float)EditorApplication.timeSinceStartup;
                float unscaledDeltaTime = newTime - curTime;
                if (unscaledDeltaTime < 1f / 120f) {
                    return;
                }
                if (unscaledDeltaTime > 1f / 10f) {
                    // Unity Editor doesn't trigger EditorApplication.update when a context menu is open, which results in a big time jump. Clamp dt in this case
                    unscaledDeltaTime = 1f / 120f;
                }
                curTime = newTime;
                if (HasInstance && Instance.tweensCount > 0) {
                    float deltaTime = unscaledDeltaTime * Time.timeScale;
                    Instance.UpdateTweens(_UpdateType.Update, deltaTime, unscaledDeltaTime);
                    Instance.UpdateTweens(_UpdateType.LateUpdate, deltaTime, unscaledDeltaTime);
                    Instance.UpdateTweens(_UpdateType.FixedUpdate, deltaTime, unscaledDeltaTime);
                }
            };

            EditorApplication.playModeStateChanged += state => {
                switch (state) {
                    case PlayModeStateChange.EnteredEditMode:
                        Instance = null;
                        customInitialCapacity = -1;
                        break;
                    case PlayModeStateChange.ExitingEditMode:
                        if (HasInstance) {
                            DestroyImmediate(Instance.gameObject);
                            Instance = null;
                        }
                        break;
                }
            };
            if (!isHotReload) {
                return;
            }
            if (HasInstance) {
                return;
            }
            var instances = Resources.FindObjectsOfTypeAll<PrimeTweenManager>();
            Assert.IsTrue(instances.Length <= 1, instances.Length);
            if (instances.Length == 0) {
                return;
            }
            var foundInScene = instances[0];
            if (foundInScene == null) {
                return;
            }
            #if PRIME_TWEEN_INSPECTOR_DEBUGGING
            Debug.LogError("PRIME_TWEEN_INSPECTOR_DEBUGGING doesn't work with 'Recompile And Continue Playing' because Tween.id is serializable but Tween.tween is not.");
            return;
            #endif
            var count = foundInScene.tweensCount;
            if (count > 0) {
                Debug.Log($"All tweens ({count}) were stopped because of 'Recompile And Continue Playing'.");
            }
            foundInScene.init(foundInScene.currentPoolCapacity);
            foundInScene.updateDepth = 0;
            Instance = foundInScene;
        }

        void Reset() {
            Assert.IsFalse(Application.isPlaying);
        }
        #endif

        void Start() {
            Assert.AreEqual(Instance, this, manualInstanceCreationIsNotAllowedMessage);
        }

        internal void FixedUpdate() => UpdateTweens(_UpdateType.FixedUpdate);

        /// <summary>
        /// The most common tween lifecycle:
        /// 1. User's script creates a tween in Update() in frame N.
        /// 2. PrimeTweenManager.LateUpdate() applies the 'startValue' to the tween in the SAME FRAME N. This guarantees that the animation is rendered at the 'startValue' in the same frame the tween is created.
        /// 3. PrimeTweenManager.Update() executes the first animation step on frame N+1. PrimeTweenManager's execution order is -2000, this means that
        ///     all tweens created in previous frames will already be updated before user's script Update() (if user's script execution order is greater than -2000).
        /// 4. PrimeTweenManager.Update() completes the tween on frame N+(duration*targetFrameRate) given that targetFrameRate is stable.
        /// </summary>
        internal void Update() => UpdateTweens(_UpdateType.Update);

        void update(List<ReusableTween> tweens, float deltaTime, float unscaledDeltaTime, out int processedCount, int? maxCount = null) {
            if (updateDepth != 0) {
                throw new Exception("updateDepth != 0");
            }
            updateDepth++;
            // onComplete and onValueChange can create new tweens. Cache count to process only those tweens that were present when the update started
            int oldCount = maxCount < tweens.Count ? maxCount.Value : tweens.Count;
            var numRemoved = 0;
            // Process tweens in the order of creation.
            // This allows to create tween duplicates because the latest tween on the same value will overwrite the previous ones.
            for (int i = 0; i < oldCount; i++) {
                var tween = tweens[i];
                var newIndex = i - numRemoved;
                #if SAFETY_CHECKS
                Assert.IsNotNull(tween);
                if (numRemoved > 0) {
                    Assert.IsNull(tweens[newIndex]);
                }
                #endif
                // ReSharper disable once PossibleNullReferenceException
                // delay release for one frame if coroutineEnumerator.resetEnumerator()
                if (tween.updateAndCheckIfRunning(tween.settings.useUnscaledTime ? unscaledDeltaTime : deltaTime) || tween.coroutineEnumerator.resetEnumerator()) {
                    if (i != newIndex) {
                        tweens[i] = null;
                        tweens[newIndex] = tween;
                    }
                    continue;
                }
                releaseTweenToPool(tween);
                tweens[i] = null; // set to null after releaseTweenToPool() so in case of an exception, the tween will stay inspectable via Inspector
                numRemoved++;
            }
            processedCount = oldCount - numRemoved;
            #if SAFETY_CHECKS
            for (int i = oldCount - numRemoved; i < oldCount; i++) { // Check removed tweens are shifted to the left and are null
                Assert.IsNull(tweens[i]);
            }
            for (int i = oldCount; i < tweens.Count; i++) { // Check all newly created tweens are not null
                Assert.IsNotNull(tweens[i]);
            }
            #endif
            updateDepth--;
            if (numRemoved != 0) {
                var newCount = tweens.Count;
                for (int i = oldCount; i < newCount; i++) {
                    var tween = tweens[i];
                    var newIndex = i - numRemoved;
                    #if SAFETY_CHECKS
                    Assert.IsNotNull(tween);
                    #endif
                    tweens[newIndex] = tween;
                }
                tweens.RemoveRange(newCount - numRemoved, numRemoved);
                Assert.AreEqual(tweens.Count, newCount - numRemoved);
                #if SAFETY_CHECKS
                foreach (var t in tweens) {
                    Assert.IsNotNull(t);
                }
                // Check no duplicates
                hashSet.Clear();
                hashSet.UnionWith(tweens);
                Assert.AreEqual(hashSet.Count, tweens.Count);
                #endif
            }
        }

        #if SAFETY_CHECKS
        readonly HashSet<ReusableTween> hashSet = new HashSet<ReusableTween>();
        #endif

        internal void LateUpdate() {
            UpdateTweens(_UpdateType.LateUpdate);
            ApplyStartValues(_UpdateType.Update);
            ApplyStartValues(_UpdateType.LateUpdate);
        }

        internal void ApplyStartValues(_UpdateType updateType) {
            switch (updateType) {
                case _UpdateType.Default:
                    Debug.LogError("Please provide non-default update type.");
                    break;
                case _UpdateType.Update:
                    ApplyStartValuesInternal(tweens, processedCount);
                    break;
                case _UpdateType.LateUpdate:
                    ApplyStartValuesInternal(lateUpdateTweens, lateUpdateTweensProcessedCount);
                    break;
                case _UpdateType.FixedUpdate:
                    ApplyStartValuesInternal(fixedUpdateTweens, fixedUpdateTweensProcessedCount);
                    break;
                default: throw new Exception($"Invalid update type: {updateType}");
            }

            void ApplyStartValuesInternal(List<ReusableTween> list, int processedCount) {
                updateDepth++;
                int cachedCount = list.Count;
                for (int i = processedCount; i < cachedCount; i++) {
                    var tween = list[i];
                    // ReSharper disable once PossibleNullReferenceException
                    if (tween._isAlive && !tween.startFromCurrent && tween.settings.startDelay == 0 && !tween.isUnityTargetDestroyed() && !tween.isAdditive
                        && tween.canManipulate()
                        && tween.elapsedTimeTotal == 0f) {
                        tween.SetElapsedTimeTotal(0f);
                    }
                }
                updateDepth--;
            }
        }

        internal void UpdateTweens(_UpdateType updateType, float? deltaTime = null, float? unscaledDeltaTime = null) {
            switch (updateType) {
                case _UpdateType.Default:
                    Debug.LogError("Please provide non-default update type.");
                    break;
                case _UpdateType.Update:
                    update(tweens, deltaTime ?? Time.deltaTime, unscaledDeltaTime ?? Time.unscaledDeltaTime, out processedCount);
                    break;
                case _UpdateType.LateUpdate:
                    update(lateUpdateTweens, deltaTime ?? Time.deltaTime, unscaledDeltaTime ?? Time.unscaledDeltaTime, out lateUpdateTweensProcessedCount, maxLateUpdateCount);
                    maxLateUpdateCount = lateUpdateTweens.Count;
                    break;
                case _UpdateType.FixedUpdate:
                    update(fixedUpdateTweens, deltaTime ?? Time.fixedDeltaTime, unscaledDeltaTime ?? Time.fixedUnscaledDeltaTime, out fixedUpdateTweensProcessedCount);
                    break;
                default: throw new Exception($"Invalid update type: {updateType}");
            }
        }

        void releaseTweenToPool([NotNull] ReusableTween tween) {
            #if SAFETY_CHECKS
            checkNotInSequence(tweens);
            checkNotInSequence(lateUpdateTweens);
            checkNotInSequence(fixedUpdateTweens);
            void checkNotInSequence(List<ReusableTween> list) {
                foreach (var t in list) {
                    if (t != null) {
                        Assert.AreNotEqual(tween.id, t.next.id);
                        Assert.AreNotEqual(tween.id, t.nextSibling.id);
                        Assert.AreNotEqual(tween.id, t.prev.id);
                    }
                }
            }
            #endif
            tween.Reset();
            pool.Add(tween);
        }

        /// Returns null if target is a destroyed UnityEngine.Object
        internal static Tween? delayWithoutDurationCheck([CanBeNull] object target, float duration, bool useUnscaledTime) {
            var tween = fetchTween();
            var settings = new TweenSettings {
                duration = duration,
                ease = Ease.Linear,
                useUnscaledTime = useUnscaledTime
            };
            tween.Setup(target, ref settings, _ => {}, null, false, TweenType.Delay);
            var result = addTween(tween);
            // ReSharper disable once RedundantCast
            return result.IsCreated ? result : (Tween?)null;
        }

        [NotNull]
        internal static ReusableTween fetchTween() {
            return Instance.fetchTween_internal();
        }

        [NotNull]
        ReusableTween fetchTween_internal() {
            ReusableTween result;
            if (pool.Count == 0) {
                result = new ReusableTween();
                if (tweensCount + 1 > currentPoolCapacity) {
                    var newCapacity = currentPoolCapacity == 0 ? 4 : currentPoolCapacity * 2;
                    Debug.LogWarning($"Tweens capacity has been increased from {currentPoolCapacity} to {newCapacity}. Please increase the capacity manually to prevent memory allocations at runtime by calling {Constants.setTweensCapacityMethod}.\n" +
                                     $"To know the highest number of simultaneously running tweens, please observe the '{nameof(PrimeTweenManager)}/{Constants.maxAliveTweens}' in Inspector.\n");
                    currentPoolCapacity = newCapacity;
                }
            } else {
                var lastIndex = pool.Count - 1;
                result = pool[lastIndex];
                pool.RemoveAt(lastIndex);
            }
            Assert.AreEqual(-1, result.id);
            result.id = lastId;
            return result;
        }

        internal static Tween Animate([NotNull] ReusableTween tween) {
            checkDuration(tween.target, tween.settings.duration);
            return addTween(tween);
        }

        internal static void checkDuration<T>([CanBeNull] T target, float duration) where T : class {
            if (Instance.warnZeroDuration && duration <= 0) {
                Debug.LogWarning($"Tween duration ({duration}) <= 0. {Constants.buildWarningCanBeDisabledMessage(nameof(warnZeroDuration))}", target as UnityEngine.Object);
            }
        }

        internal static Tween addTween([NotNull] ReusableTween tween) {
            return Instance.addTween_internal(tween);
        }

        Tween addTween_internal([NotNull] ReusableTween tween) {
            Assert.IsNotNull(tween);
            Assert.IsTrue(tween.id > 0);
            if (tween.target == null || tween.isUnityTargetDestroyed()) {
                Debug.LogError($"Tween's target is null: {tween.GetDescription()}. This error can mean that:\n" +
                               "- The target reference is null.\n" +
                               "- UnityEngine.Object target reference is not populated in the Inspector.\n" +
                               "- UnityEngine.Object target has been destroyed.\n" +
                               "Please ensure you're using a valid target.\n");
                tween.kill();
                releaseTweenToPool(tween);
                return default;
            }
            if (warnTweenOnDisabledTarget) {
                if (tween.target is Component comp && !comp.gameObject.activeInHierarchy) {
                    Debug.LogWarning($"Tween is started on GameObject that is not active in hierarchy: {comp.name}. {Constants.buildWarningCanBeDisabledMessage(nameof(warnTweenOnDisabledTarget))}", comp);
                }
            }
            if (tween.settings._updateType == _UpdateType.Default) {
                tween.settings._updateType = defaultUpdateType;
            }
            switch (tween.settings._updateType) {
                case _UpdateType.Update:
                    tweens.Add(tween);
                    break;
                case _UpdateType.LateUpdate:
                    lateUpdateTweens.Add(tween);
                    break;
                case _UpdateType.FixedUpdate:
                    fixedUpdateTweens.Add(tween);
                    break;
                default:
                    Debug.LogError($"Invalid update type: {tween.settings._updateType}");
                    return default;
            }
            #if SAFETY_CHECKS
            // Debug.Log($"[{Time.frameCount}] created: {tween.GetDescription()}", tween.unityTarget);
            StackTraces.Record(tween.id);
            #endif
            lastId++; // increment only when tween added successfully
            #if UNITY_ASSERTIONS && !PRIME_TWEEN_DISABLE_ASSERTIONS
            maxSimultaneousTweensCount = Math.Max(maxSimultaneousTweensCount, tweensCount);
            if (warnBenchmarkWithAsserts && maxSimultaneousTweensCount > 50000) {
                warnBenchmarkWithAsserts = false;
                var msg = "PrimeTween detected more than 50000 concurrent tweens. If you're running benchmarks, please add the PRIME_TWEEN_DISABLE_ASSERTIONS to the 'ProjectSettings/Player/Script Compilation' to disable assertions. This will ensure PrimeTween runs with the release performance.\n" +
                          "Also disable optional convenience features: PrimeTweenConfig.warnZeroDuration and PrimeTweenConfig.warnTweenOnDisabledTarget.\n";
                if (Application.isEditor) {
                    msg += "Please also run the tests in real builds, not in the Editor, to measure the performance correctly.\n";
                }
                msg += $"{Constants.buildWarningCanBeDisabledMessage(nameof(PrimeTweenConfig.warnBenchmarkWithAsserts))}\n";
                Debug.LogError(msg);
            }
            #endif
            return new Tween(tween);
        }

        internal static int processAll([CanBeNull] object onTarget, [NotNull] Predicate<ReusableTween> predicate, bool allowToProcessTweensInsideSequence) {
            return Instance.processAll_internal(onTarget, predicate, allowToProcessTweensInsideSequence);
        }

        internal static bool logCantManipulateError = true;

        int processAll_internal([CanBeNull] object onTarget, [NotNull] Predicate<ReusableTween> predicate, bool allowToProcessTweensInsideSequence) {
            return processInList(tweens) + processInList(lateUpdateTweens) + processInList(fixedUpdateTweens);
            int processInList(List<ReusableTween> tweens) {
                int numProcessed = 0;
                int totalCount = 0;
                var count = tweens.Count; // this is not an optimization, OnComplete() may create new tweens
                for (var i = 0; i < count; i++) {
                    var tween = tweens[i];
                    if (tween == null) {
                        continue;
                    }
                    totalCount++;
                    if (onTarget != null) {
                        if (tween.target != onTarget) {
                            continue;
                        }
                        if (!allowToProcessTweensInsideSequence && tween.IsInSequence()) {
                            // To support stopping sequences by target, I can add new API 'Sequence.Create(object sequenceTarget)'.
                            // But 'sequenceTarget' is a different concept to tween's target, so I should not mix these two concepts together:
                            //     'sequenceTarget' serves the purpose of unique 'id', while tween's target is the animated object.
                            // In my opinion, the benefits of this new API don't outweigh the added complexity. A much more simpler approach is to store the Sequence reference and call sequence.Stop() directly.
                            Assert.IsFalse(tween.isMainSequenceRoot());
                            if (logCantManipulateError) {
                                Assert.LogError(Constants.cantManipulateNested, tween.id);
                            }
                            continue;
                        }
                    }
                    if (tween._isAlive && predicate(tween)) {
                        numProcessed++;
                    }
                }
                if (onTarget == null) {
                    return totalCount;
                }
                return numProcessed;
            }
        }

        internal void SetTweensCapacity(int capacity) {
            var runningTweens = tweensCount;
            if (capacity < runningTweens) {
                Debug.LogError($"New capacity ({capacity}) should be greater than the number of currently running tweens ({runningTweens}).\n" +
                        $"You can use {nameof(Tween)}.{nameof(Tween.StopAll)}() to stop all running tweens.");
                return;
            }
            tweens.Capacity = capacity;
            lateUpdateTweens.Capacity = capacity;
            fixedUpdateTweens.Capacity = capacity;
            #if UNITY_2021_2_OR_NEWER
            shakes.EnsureCapacity(capacity);
            #endif
            resizeAndSetCapacity(pool, capacity - runningTweens, capacity);
            currentPoolCapacity = capacity;
            Assert.AreEqual(capacity, tweens.Capacity);
            Assert.AreEqual(capacity, lateUpdateTweens.Capacity);
            Assert.AreEqual(capacity, fixedUpdateTweens.Capacity);
            Assert.AreEqual(capacity, pool.Capacity);
        }

        internal int tweensCount => tweens.Count + lateUpdateTweens.Count + fixedUpdateTweens.Count;

        internal static void resizeAndSetCapacity([NotNull] List<ReusableTween> list, int newCount, int newCapacity) {
            Assert.IsTrue(newCapacity >= newCount);
            int curCount = list.Count;
            if (curCount > newCount) {
                var numToRemove = curCount - newCount;
                list.RemoveRange(newCount, numToRemove);
                list.Capacity = newCapacity;
            } else {
                list.Capacity = newCapacity;
                if (newCount > curCount) {
                    var numToCreate = newCount - curCount;
                    for (int i = 0; i < numToCreate; i++) {
                        list.Add(new ReusableTween());
                    }
                }
            }
            Assert.AreEqual(newCount, list.Count);
            Assert.AreEqual(newCapacity, list.Capacity);
        }

        [Conditional("UNITY_ASSERTIONS")]
        internal void warnStructBoxingInCoroutineOnce(long id) {
            if (!warnStructBoxingAllocationInCoroutine) {
                return;
            }
            warnStructBoxingAllocationInCoroutine = false;
            Assert.LogWarning("Please use Tween/Sequence." + nameof(Tween.ToYieldInstruction) + "() when waiting for a Tween/Sequence in coroutines to prevent struct boxing.\n" +
                              Constants.buildWarningCanBeDisabledMessage(nameof(PrimeTweenConfig.warnStructBoxingAllocationInCoroutine)) + "\n", id);
        }
    }
}
