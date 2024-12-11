using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;
#pragma warning disable CS0618

namespace PrimeTween {
    public partial struct Tween {
        /// <summary>This method is needed for async/await support. Don't use it directly.</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public TweenAwaiter GetAwaiter() {
            return new TweenAwaiter(this);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This struct is needed for async/await support, you should not use it directly.")]
        public readonly struct TweenAwaiter : INotifyCompletion {
            readonly Tween tween;

            internal TweenAwaiter(Tween tween) {
                if (tween.isAlive && !tween.tryManipulate()) {
                    this.tween = default;
                } else {
                    this.tween = tween;
                }
            }

            public bool IsCompleted => !tween.isAlive;

            public void OnCompleted([NotNull] Action continuation) {
                // try-catch is needed here because any exception that is thrown inside the OnCompleted will be silenced
                // probably because this try in UnitySynchronizationContext.cs has no exception handling:
                // https://github.com/Unity-Technologies/UnityCsReference/blob/dd0d959800a675836a77dbe188c7dd55abc7c512/Runtime/Export/Scripting/UnitySynchronizationContext.cs#L157
                try {
                    Assert.IsTrue(tween.isAlive);
                    var infiniteSettings = new TweenSettings<float>(0, 0, float.MaxValue, Ease.Linear, -1);
                    var wait = animate(tween.tween, ref infiniteSettings, t => {
                        if (t._isAlive) {
                            var target = t.target as ReusableTween;
                            if (t.longParam != target.id || !target._isAlive) {
                                t.ForceComplete();
                            }
                        }
                    }, null, TweenType.Callback);
                    Assert.IsTrue(wait.isAlive);
                    wait.tween.longParam = tween.id;
                    wait.tween.OnComplete(continuation, true);
                } catch (Exception e) {
                    Debug.LogException(e);
                    throw;
                }
            }

            public void GetResult() {
            }
        }
    }

    public partial struct Sequence {
        /// <summary>This method is needed for async/await support. Don't use it directly.</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Tween.TweenAwaiter GetAwaiter() {
            return new Tween.TweenAwaiter(root);
        }
    }
}
