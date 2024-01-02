// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMethodReturnValue.Global
using System;
using JetBrains.Annotations;
using UnityEngine;

namespace PrimeTween {
    public partial struct Tween {
        /// <summary>Returns the number of alive tweens.</summary>
        /// <param name="onTarget">If specified, returns the number of running tweens on the target. Please note: if target is specified, this method call has O(n) complexity where n is the total number of running tweens.</param>
        public static int GetTweensCount([CanBeNull] object onTarget = null) {
            #if UNITY_EDITOR
            if (Constants.warnNoInstance) {
                return default;
            }
            #endif
            var manager = PrimeTweenManager.Instance;
            if (onTarget == null && manager.updateDepth == 0) {
                int result = manager.tweensCount;
                #if PRIME_TWEEN_SAFETY_CHECKS && UNITY_ASSERTIONS
                Assert.AreEqual(result, PrimeTweenManager.processAll(null, _ => true));
                #endif
                return result;
            }
            return PrimeTweenManager.processAll(onTarget, _ => true); // call processAll to filter null tweens
        }

        #if PRIME_TWEEN_EXPERIMENTAL
        public static int GetTweensCapacity() {
            var instance = PrimeTweenConfig.Instance;
            if (instance == null) {
                return PrimeTweenManager.customInitialCapacity;
            }
            return instance.currentPoolCapacity;
        }
        #endif

        /// <summary>Stops all tweens and sequences.<br/>
        /// If <see cref="onTarget"/> is provided, stops only tweens on this target (stopping a tween inside a Sequence is not allowed).</summary>
        /// <returns>The number of stopped tweens.</returns>
        public static int StopAll([CanBeNull] object onTarget = null) {
            var result = PrimeTweenManager.processAll(onTarget, tween => {
                if (tween.IsInSequence()) {
                    if (tween.isMainSequenceRoot()) {
                        tween.sequence.Stop();
                    }
                    // do nothing with nested tween or sequence. The main sequence root will process it
                } else {
                    tween.kill();
                }
                return true;
            });
            forceUpdateManagerIfTargetIsNull(onTarget);
            return result;
        }

        /// <summary>Completes all tweens and sequences.<br/>
        /// If <see cref="onTarget"/> is provided, completes only tweens on this target (completing a tween inside a Sequence is not allowed).</summary>
        /// <returns>The number of completed tweens.</returns>
        public static int CompleteAll([CanBeNull] object onTarget = null) {
            var result = PrimeTweenManager.processAll(onTarget, tween => {
                if (tween.IsInSequence()) {
                    if (tween.isMainSequenceRoot()) {
                        tween.sequence.Complete();
                    }
                    // do nothing with nested tween or sequence. The main sequence root will process it
                } else {
                    tween.ForceComplete();
                }
                return true;
            });
            forceUpdateManagerIfTargetIsNull(onTarget);
            return result;
        }

        static void forceUpdateManagerIfTargetIsNull([CanBeNull] object onTarget) {
            if (onTarget == null) {
                var manager = PrimeTweenManager.Instance;
                if (manager != null) {
                    if (manager.updateDepth == 0) {
                        manager.FixedUpdate();
                        manager.Update();
                    }
                    // Assert.AreEqual(0, manager.tweens.Count); // fails if user's OnComplete() creates new tweens
                }
            }
        }
        
        /// <summary>Pauses/unpauses all tweens and sequences.<br/>
        /// If <see cref="onTarget"/> is provided, pauses/unpauses only tweens on this target (pausing/unpausing a tween inside a Sequence is not allowed).</summary>
        /// <returns>The number of paused/unpaused tweens.</returns>
        public static int SetPausedAll(bool isPaused, [CanBeNull] object onTarget = null) {
            if (isPaused) {
                return PrimeTweenManager.processAll(onTarget, tween => {
                    return tween.trySetPause(true);
                });
            }
            return PrimeTweenManager.processAll(onTarget, tween => {
                return tween.trySetPause(false);
            });
        }

        /// <summary>Please note: delay may outlive the caller (the calling UnityEngine.Object may already be destroyed).
        /// When using this overload, it's user's responsibility to ensure that <see cref="onComplete"/> is safe to execute once the delay is finished.
        /// It's preferable to use the <see cref="Delay{T}"/> overload because it checks if the UnityEngine.Object target is still alive before calling the <see cref="onComplete"/>.</summary>
        /// <param name="warnIfTargetDestroyed">https://github.com/KyryloKuzyk/PrimeTween/discussions/4</param>
        public static Tween Delay(float duration, [CanBeNull] Action onComplete = null, bool useUnscaledTime = false, bool warnIfTargetDestroyed = true) {
            return delay(PrimeTweenManager.dummyTarget, duration, onComplete, useUnscaledTime, warnIfTargetDestroyed);
        }
        /// <param name="warnIfTargetDestroyed">https://github.com/KyryloKuzyk/PrimeTween/discussions/4</param>
        public static Tween Delay([NotNull] object target, float duration, [CanBeNull] Action onComplete = null, bool useUnscaledTime = false, bool warnIfTargetDestroyed = true) {
            return delay(target, duration, onComplete, useUnscaledTime, warnIfTargetDestroyed);
        }
        static Tween delay([CanBeNull] object target, float duration, [CanBeNull] Action onComplete, bool useUnscaledTime, bool warnIfTargetDestroyed) {
            var result = delay_internal(target, duration, useUnscaledTime);
            if (onComplete != null) {
                result?.tween.OnComplete(onComplete, warnIfTargetDestroyed);
            }
            return result ?? default;
        }
        
        /// <summary> This is the most preferable overload of all Delay functions:<br/>
        /// - It checks if UnityEngine.Object target is still alive before calling the <see cref="onComplete"/> callback.<br/>
        /// - It allows to call any method on <see cref="target"/> without producing garbage.</summary>
        /// <example>
        /// <code>
        /// Tween.Delay(this, duration: 1f, onComplete: _this =&gt; {
        ///     // Please note: we're using '_this' variable from the onComplete callback. Calling DoSomething() directly will implicitly capture 'this' variable (creating a closure) and generate garbage.
        ///     _this.DoSomething();
        /// });
        /// </code>
        /// </example>
        /// <param name="warnIfTargetDestroyed">https://github.com/KyryloKuzyk/PrimeTween/discussions/4</param>
        public static Tween Delay<T>([NotNull] T target, float duration, [NotNull] Action<T> onComplete, bool useUnscaledTime = false, bool warnIfTargetDestroyed = true) where T : class {
            var maybeDelay = delay_internal(target, duration, useUnscaledTime);
            if (!maybeDelay.HasValue) {
                return default;
            }
            var delay = maybeDelay.Value;
            delay.tween.OnComplete(target, onComplete, warnIfTargetDestroyed);
            return delay;
        }

        static Tween? delay_internal([CanBeNull] object target, float duration, bool useUnscaledTime) {
            PrimeTweenManager.checkDuration(target, duration);
            return PrimeTweenManager.delayWithoutDurationCheck(target, duration, useUnscaledTime);
        }

        public static Tween MaterialColor([NotNull] Material target, int propertyId, Color endValue, float duration, Ease ease = default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false)
            => MaterialColor(target, propertyId, new TweenSettings<Color>(endValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime)));
        public static Tween MaterialColor([NotNull] Material target, int propertyId, Color endValue, float duration, Easing ease, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false)
            => MaterialColor(target, propertyId, new TweenSettings<Color>(endValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime)));
        public static Tween MaterialColor([NotNull] Material target, int propertyId, Color startValue, Color endValue, float duration, Ease ease = default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false)
            => MaterialColor(target, propertyId, new TweenSettings<Color>(startValue, endValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime)));
        public static Tween MaterialColor([NotNull] Material target, int propertyId, Color startValue, Color endValue, float duration, Easing ease, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false)
            => MaterialColor(target, propertyId, new TweenSettings<Color>(startValue, endValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime)));
        public static Tween MaterialColor([NotNull] Material target, int propertyId, Color endValue, TweenSettings settings) => MaterialColor(target, propertyId, new TweenSettings<Color>(endValue, settings));
        public static Tween MaterialColor([NotNull] Material target, int propertyId, Color startValue, Color endValue, TweenSettings settings) => MaterialColor(target, propertyId, new TweenSettings<Color>(startValue, endValue, settings));
        public static Tween MaterialColor([NotNull] Material target, int propertyId, TweenSettings<Color> settings) {
            return animateWithIntParam(target, propertyId, ref settings,
                tween => (tween.unityTarget as Material).SetColor(tween.intParam, tween.ColorVal),
                tween => (tween.unityTarget as Material).GetColor(tween.intParam).ToContainer());
        }

        public static Tween MaterialProperty([NotNull] Material target, int propertyId, float endValue, float duration, Ease ease = default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false)
            => MaterialProperty(target, propertyId, new TweenSettings<float>(endValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime)));
        public static Tween MaterialProperty([NotNull] Material target, int propertyId, float endValue, float duration, Easing ease, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false)
            => MaterialProperty(target, propertyId, new TweenSettings<float>(endValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime)));
        public static Tween MaterialProperty([NotNull] Material target, int propertyId, float startValue, float endValue, float duration, Ease ease = default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false)
            => MaterialProperty(target, propertyId, new TweenSettings<float>(startValue, endValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime)));
        public static Tween MaterialProperty([NotNull] Material target, int propertyId, float startValue, float endValue, float duration, Easing ease, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false)
            => MaterialProperty(target, propertyId, new TweenSettings<float>(startValue, endValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime)));
        public static Tween MaterialProperty([NotNull] Material target, int propertyId, float endValue, TweenSettings settings) => MaterialProperty(target, propertyId, new TweenSettings<float>(endValue, settings));
        public static Tween MaterialProperty([NotNull] Material target, int propertyId, float startValue, float endValue, TweenSettings settings) => MaterialProperty(target, propertyId, new TweenSettings<float>(startValue, endValue, settings));
        public static Tween MaterialProperty([NotNull] Material target, int propertyId, TweenSettings<float> settings) {
            return animateWithIntParam(target, propertyId, ref settings,
                tween => (tween.unityTarget as Material).SetFloat(tween.intParam, tween.FloatVal),
                tween => (tween.unityTarget as Material).GetFloat(tween.intParam).ToContainer());
        }

        public static Tween MaterialAlpha([NotNull] Material target, int propertyId, float endValue, float duration, Ease ease = default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false)
            => MaterialAlpha(target, propertyId, new TweenSettings<float>(endValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime)));
        public static Tween MaterialAlpha([NotNull] Material target, int propertyId, float endValue, float duration, Easing ease, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false)
            => MaterialAlpha(target, propertyId, new TweenSettings<float>(endValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime)));
        public static Tween MaterialAlpha([NotNull] Material target, int propertyId, float startValue, float endValue, float duration, Ease ease = default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false)
            => MaterialAlpha(target, propertyId, new TweenSettings<float>(startValue, endValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime)));
        public static Tween MaterialAlpha([NotNull] Material target, int propertyId, float startValue, float endValue, float duration, Easing ease, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false)
            => MaterialAlpha(target, propertyId, new TweenSettings<float>(startValue, endValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime)));
        public static Tween MaterialAlpha([NotNull] Material target, int propertyId, float endValue, TweenSettings settings) => MaterialAlpha(target, propertyId, new TweenSettings<float>(endValue, settings));
        public static Tween MaterialAlpha([NotNull] Material target, int propertyId, float startValue, float endValue, TweenSettings settings) => MaterialAlpha(target, propertyId, new TweenSettings<float>(startValue, endValue, settings));
        public static Tween MaterialAlpha([NotNull] Material target, int propertyId, TweenSettings<float> settings) {
            return animateWithIntParam(target, propertyId, ref settings,
                tween => {
                    var _target = tween.unityTarget as Material;
                    var _propId = tween.intParam;
                    _target.SetColor(_propId, _target.GetColor(_propId).WithAlpha(tween.FloatVal));
                },
                tween => (tween.unityTarget as Material).GetColor(tween.intParam).a.ToContainer());
        }

        public static Tween MaterialTextureOffset([NotNull] Material target, int propertyId, Vector2 endValue, float duration, Ease ease = default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false)
            => MaterialTextureOffset(target, propertyId, new TweenSettings<Vector2>(endValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime)));
        public static Tween MaterialTextureOffset([NotNull] Material target, int propertyId, Vector2 endValue, float duration, Easing ease, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false)
            => MaterialTextureOffset(target, propertyId, new TweenSettings<Vector2>(endValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime)));
        public static Tween MaterialTextureOffset([NotNull] Material target, int propertyId, Vector2 startValue, Vector2 endValue, float duration, Ease ease = default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false)
            => MaterialTextureOffset(target, propertyId, new TweenSettings<Vector2>(startValue, endValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime)));
        public static Tween MaterialTextureOffset([NotNull] Material target, int propertyId, Vector2 startValue, Vector2 endValue, float duration, Easing ease, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false)
            => MaterialTextureOffset(target, propertyId, new TweenSettings<Vector2>(startValue, endValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime)));
        public static Tween MaterialTextureOffset([NotNull] Material target, int propertyId, Vector2 endValue, TweenSettings settings) => MaterialTextureOffset(target, propertyId, new TweenSettings<Vector2>(endValue, settings));
        public static Tween MaterialTextureOffset([NotNull] Material target, int propertyId, Vector2 startValue, Vector2 endValue, TweenSettings settings) => MaterialTextureOffset(target, propertyId, new TweenSettings<Vector2>(startValue, endValue, settings));
        public static Tween MaterialTextureOffset([NotNull] Material target, int propertyId, TweenSettings<Vector2> settings) {
            return animateWithIntParam(target, propertyId, ref settings,
                tween => (tween.unityTarget as Material).SetTextureOffset(tween.intParam, tween.Vector2Val),
                tween => (tween.unityTarget as Material).GetTextureOffset(tween.intParam).ToContainer());
        }

        public static Tween MaterialTextureScale([NotNull] Material target, int propertyId, Vector2 endValue, float duration, Ease ease = default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false)
            => MaterialTextureScale(target, propertyId, new TweenSettings<Vector2>(endValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime)));
        public static Tween MaterialTextureScale([NotNull] Material target, int propertyId, Vector2 endValue, float duration, Easing ease, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false)
            => MaterialTextureScale(target, propertyId, new TweenSettings<Vector2>(endValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime)));
        public static Tween MaterialTextureScale([NotNull] Material target, int propertyId, Vector2 startValue, Vector2 endValue, float duration, Ease ease = default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false)
            => MaterialTextureScale(target, propertyId, new TweenSettings<Vector2>(startValue, endValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime)));
        public static Tween MaterialTextureScale([NotNull] Material target, int propertyId, Vector2 startValue, Vector2 endValue, float duration, Easing ease, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false)
            => MaterialTextureScale(target, propertyId, new TweenSettings<Vector2>(startValue, endValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime)));
        public static Tween MaterialTextureScale([NotNull] Material target, int propertyId, Vector2 endValue, TweenSettings settings) => MaterialTextureScale(target, propertyId, new TweenSettings<Vector2>(endValue, settings));
        public static Tween MaterialTextureScale([NotNull] Material target, int propertyId, Vector2 startValue, Vector2 endValue, TweenSettings settings) => MaterialTextureScale(target, propertyId, new TweenSettings<Vector2>(startValue, endValue, settings));
        public static Tween MaterialTextureScale([NotNull] Material target, int propertyId, TweenSettings<Vector2> settings) {
            return animateWithIntParam(target, propertyId, ref settings,
                tween => (tween.unityTarget as Material).SetTextureScale(tween.intParam, tween.Vector2Val),
                tween => (tween.unityTarget as Material).GetTextureScale(tween.intParam).ToContainer());
        }

        public static Tween MaterialProperty([NotNull] Material target, int propertyId, Vector4 endValue, float duration, Ease ease = default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false)
            => MaterialProperty(target, propertyId, new TweenSettings<Vector4>(endValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime)));
        public static Tween MaterialProperty([NotNull] Material target, int propertyId, Vector4 endValue, float duration, Easing ease, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false)
            => MaterialProperty(target, propertyId, new TweenSettings<Vector4>(endValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime)));
        public static Tween MaterialProperty([NotNull] Material target, int propertyId, Vector4 startValue, Vector4 endValue, float duration, Ease ease = default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false)
            => MaterialProperty(target, propertyId, new TweenSettings<Vector4>(startValue, endValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime)));
        public static Tween MaterialProperty([NotNull] Material target, int propertyId, Vector4 startValue, Vector4 endValue, float duration, Easing ease, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false)
            => MaterialProperty(target, propertyId, new TweenSettings<Vector4>(startValue, endValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime)));
        public static Tween MaterialProperty([NotNull] Material target, int propertyId, Vector4 endValue, TweenSettings settings) => MaterialProperty(target, propertyId, new TweenSettings<Vector4>(endValue, settings));
        public static Tween MaterialProperty([NotNull] Material target, int propertyId, Vector4 startValue, Vector4 endValue, TweenSettings settings) => MaterialProperty(target, propertyId, new TweenSettings<Vector4>(startValue, endValue, settings));
        public static Tween MaterialProperty([NotNull] Material target, int propertyId, TweenSettings<Vector4> settings) {
            return animateWithIntParam(target, propertyId, ref settings,
                tween => (tween.unityTarget as Material).SetVector(tween.intParam, tween.Vector4Val),
                tween => (tween.unityTarget as Material).GetVector(tween.intParam).ToContainer());
        }
 
        // No 'startFromCurrent' overload
        public static Tween EulerAngles([NotNull] Transform target, Vector3 startValue, Vector3 endValue, float duration, Ease ease = Ease.Default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false) 
            => EulerAngles(target, new TweenSettings<Vector3>(startValue, endValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime)));
        public static Tween EulerAngles([NotNull] Transform target, Vector3 startValue, Vector3 endValue, float duration, Easing ease, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false) 
            => EulerAngles(target, new TweenSettings<Vector3>(startValue, endValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime)));
        public static Tween EulerAngles([NotNull] Transform target, Vector3 startValue, Vector3 endValue, TweenSettings settings) => EulerAngles(target, new TweenSettings<Vector3>(startValue, endValue, settings));
        public static Tween EulerAngles([NotNull] Transform target, TweenSettings<Vector3> settings) {
            validateEulerAnglesData(ref settings);
            return animate(target, ref settings, _ => { (_.unityTarget as Transform).eulerAngles = _.Vector3Val; }, _ => (_.unityTarget as Transform).eulerAngles.ToContainer());
        }
        
        public static Tween LocalEulerAngles([NotNull] Transform target, Vector3 startValue, Vector3 endValue, float duration, Ease ease = Ease.Default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false) 
            => LocalEulerAngles(target, new TweenSettings<Vector3>(startValue, endValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime)));
        public static Tween LocalEulerAngles([NotNull] Transform target, Vector3 startValue, Vector3 endValue, float duration, Easing ease, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false) 
            => LocalEulerAngles(target, new TweenSettings<Vector3>(startValue, endValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime)));
        public static Tween LocalEulerAngles([NotNull] Transform target, Vector3 startValue, Vector3 endValue, TweenSettings settings) => LocalEulerAngles(target, new TweenSettings<Vector3>(startValue, endValue, settings));
        public static Tween LocalEulerAngles([NotNull] Transform target, TweenSettings<Vector3> settings) {
            validateEulerAnglesData(ref settings);
            return animate(target, ref settings, _ => { (_.unityTarget as Transform).localEulerAngles = _.Vector3Val; }, _ => (_.unityTarget as Transform).localEulerAngles.ToContainer());
        }
        static void validateEulerAnglesData(ref TweenSettings<Vector3> settings) {
            if (settings.startFromCurrent) {
                settings.startFromCurrent = false;
                Debug.LogWarning("Animating euler angles from the current value may produce unexpected results because there is more than one way to represent the current rotation using Euler angles.\n" +
                                 "'" + nameof(TweenSettings<float>.startFromCurrent) + "' was ignored.\n" +
                                 "More info: https://docs.unity3d.com/ScriptReference/Transform-eulerAngles.html\n");
            }
        }
        
        // Called from TweenGenerated.cs
        public static Tween Scale([NotNull] Transform target, TweenSettings<float> uniformScaleSettings) {
            var remapped = new TweenSettings<Vector3>(uniformScaleSettings.startValue * Vector3.one, uniformScaleSettings.endValue * Vector3.one, uniformScaleSettings.settings) { startFromCurrent = uniformScaleSettings.startFromCurrent };
            return Scale(target, remapped);
        }
        public static Tween Rotation([NotNull] Transform target, TweenSettings<Vector3> eulerAnglesSettings) => Rotation(target, toQuaternion(eulerAnglesSettings));
        public static Tween LocalRotation([NotNull] Transform target, TweenSettings<Vector3> localEulerAnglesSettings) => LocalRotation(target, toQuaternion(localEulerAnglesSettings));
        static TweenSettings<Quaternion> toQuaternion(TweenSettings<Vector3> s) => new TweenSettings<Quaternion>(Quaternion.Euler(s.startValue), Quaternion.Euler(s.endValue), s.settings) { startFromCurrent = s.startFromCurrent };
        
        #if TEXT_MESH_PRO_INSTALLED
        public static Tween TextMaxVisibleCharacters([NotNull] TMPro.TMP_Text target, TweenSettings<int> settings) {
            int oldCount = target.textInfo.characterCount;
            target.ForceMeshUpdate();
            if (oldCount != target.textInfo.characterCount) {
                Debug.LogWarning("Please call TMP_Text.ForceMeshUpdate() before animating maxVisibleCharacters.");
            }
            var floatSettings = new TweenSettings<float>(settings.startValue, settings.endValue, settings.settings);
            return animate(target, ref floatSettings, _tween => {
                var _target = _tween.target as TMPro.TMP_Text;
                _target.maxVisibleCharacters = Mathf.RoundToInt(_tween.FloatVal);
            }, t => new ValueContainer { FloatVal = (t.target as TMPro.TMP_Text).maxVisibleCharacters });
        }
        #endif
        
        public static Tween GlobalTimeScale(Single endValue, float duration, Ease ease = Ease.Default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0) 
            => GlobalTimeScale(new TweenSettings<float>(endValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, true)));
        public static Tween GlobalTimeScale(Single endValue, float duration, Easing ease, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0) 
            => GlobalTimeScale(new TweenSettings<float>(endValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, true)));
        public static Tween GlobalTimeScale(Single startValue, Single endValue, float duration, Ease ease = Ease.Default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0) 
            => GlobalTimeScale(new TweenSettings<float>(startValue, endValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, true)));
        public static Tween GlobalTimeScale(Single startValue, Single endValue, float duration, Easing ease, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0) 
            => GlobalTimeScale(new TweenSettings<float>(startValue, endValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, true)));
        public static Tween GlobalTimeScale(Single endValue, TweenSettings settings) => GlobalTimeScale(new TweenSettings<float>(endValue, settings));
        public static Tween GlobalTimeScale(Single startValue, Single endValue, TweenSettings settings) => GlobalTimeScale(new TweenSettings<float>(startValue, endValue, settings));
        public static Tween GlobalTimeScale(TweenSettings<float> settings) {
            clampTimescale(ref settings.startValue);
            clampTimescale(ref settings.endValue);
            if (!settings.settings.useUnscaledTime) {
                Debug.LogWarning("Setting " + nameof(TweenSettings.useUnscaledTime) + " to true to animate Time.timeScale correctly.");
                settings.settings.useUnscaledTime = true;
            }
            return animate(PrimeTweenManager.dummyTarget, ref settings, t => Time.timeScale = t.FloatVal, _ => Time.timeScale.ToContainer());
            
            void clampTimescale(ref float value) {
                if (value < 0) {
                    Debug.LogError($"timeScale should be >= 0, but was {value}");
                    value = 0;
                }
            }
        }

        public static Tween TweenTimeScale(Tween tween, TweenSettings<float> settings) {
            if (!tween.tryManipulate()) {
                return default;
            }
            var result = animate(tween.tween, ref settings, t => {
                var target = t.target as ReusableTween;
                if (t.intParam != target.id) {
                    t.EmergencyStop();
                    return;
                }
                target.timeScale = t.FloatVal;
            }, t => (t.target as ReusableTween).timeScale.ToContainer());
            Assert.IsTrue(result.isAlive);
            result.tween.intParam = tween.id;
            return result;
        }

        public static Tween TweenTimeScale(Sequence sequence, TweenSettings<float> settings) => TweenTimeScale(sequence.root, settings);

        #if PRIME_TWEEN_EXPERIMENTAL
        /// <summary>Similar to position animation with Ease.OutBounce, but gives the ability to customize the bounce behaviour by specifying the exact bounce amplitude, number of bounces, and bounce stiffness.</summary>
        public static Sequence PositionOutBounce([NotNull] Transform target, Vector3 endValue, float duration, float bounceAmplitude, int numBounces = 2, float stiffness = 0.5f, bool useUnscaledTime = false)
            => CustomOutBounce(target, (t, s) => Position(t, s), endValue, duration, bounceAmplitude, numBounces, stiffness, t => t.position, useUnscaledTime);

        /// <summary>Similar to position animation with Ease.OutBounce, but gives the ability to customize the bounce behaviour by specifying the exact bounce amplitude, number of bounces, and bounce stiffness.</summary>
        public static Sequence LocalPositionOutBounce([NotNull] Transform target, Vector3 endValue, float duration, float bounceAmplitude, int numBounces = 2, float stiffness = 0.5f, bool useUnscaledTime = false)
            => CustomOutBounce(target, (t, s) => LocalPosition(t, s), endValue, duration, bounceAmplitude, numBounces, stiffness, t => t.localPosition, useUnscaledTime);

        static Sequence CustomOutBounce([NotNull] Transform target, [NotNull] Func<Transform, TweenSettings<Vector3>, Tween> animFunc, Vector3 endValue, float duration, float bounceAmplitude, int numBounces, float stiffness, [NotNull] Func<Transform, Vector3> getPos, bool useUnscaledTime) {
            if (bounceAmplitude == 0) {
                Debug.LogWarning("Please provide non-zero " + nameof(bounceAmplitude) + ".");
            }
            if (numBounces < 0 || numBounces > 10) {
                Debug.LogError("'" + nameof(numBounces) + "' should be >= 0 and <= 10.");
                numBounces = Mathf.Clamp(numBounces, 0, 10);
            }
            if (stiffness < 0 || stiffness > 5) {
                Debug.LogError("'" + nameof(stiffness) + "' should be >= 0 and <= 5.");
                stiffness = Mathf.Clamp(stiffness, 0, 5);
            }
            stiffness += 1;
            // move durations: x + x/stiffness + x/stiffness^2 + x/stiffness^3...
            float x = 1;
            for (int i = 1; i <= numBounces; i++) {
                x += 1 / Mathf.Pow(stiffness, i);
            }
            var curDur = duration / x;
            var sequence = Sequence.Create(animFunc(target, new TweenSettings<Vector3>(endValue, curDur, Ease.InSine, useUnscaledTime: useUnscaledTime)));
            var bounceVector = (getPos(target) - endValue).normalized * bounceAmplitude;
            for (int i = 1; i <= numBounces; i++) {
                curDur /= stiffness;
                sequence.Chain(animFunc(target, new TweenSettings<Vector3>(endValue + bounceVector, curDur / 2f, Ease.OutSine, 2, CycleMode.Rewind, useUnscaledTime: useUnscaledTime)));
                bounceVector /= stiffness * 2;
            }
            return sequence;
        }
        #endif // PRIME_TWEEN_EXPERIMENTAL
    }
}