/*// ReSharper disable UnusedMemberInSuper.Global
using System;
using JetBrains.Annotations;

namespace PrimeTween {
    // ReSharper disable once TypeParameterCanBeVariant
    internal interface ITween<TResult> {
        bool isAlive { get; }
        void Stop();
        void Complete();
        TResult OnComplete([NotNull] Action onComplete, bool warnIfTargetDestroyed = true);
        TResult OnComplete<T>([NotNull] T target, [NotNull] Action<T> onComplete, bool warnIfTargetDestroyed = true) where T : class;
        Sequence Group(Tween tween);
        Sequence Chain(Tween tween);
        Sequence Group(Sequence sequence);
        Sequence Chain(Sequence sequence);
        void SetRemainingCycles(int cycles);
        void SetRemainingCycles(bool stopAtEndValue);
        
        int cyclesDone { get; }
        int cyclesTotal { get; }
        bool isPaused { get; set; }
        float timeScale { get; set; }
        float duration { get; }
        float durationTotal { get; }
        float elapsedTime { get; set; }
        float elapsedTimeTotal { get; set; }
        float progress { get; set; }
        float progressTotal { get; set; }
        // TResult OnUpdate<T>(T target, Action<T, Tween> onUpdate) where T : class; // Sequence doesn't support OnUpdate because its root updates before all children tweens, but it's reasonable that OnUpdate() should be called AFTER all sequence children are updated   
    }
}*/