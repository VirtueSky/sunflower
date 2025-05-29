using System;
using JetBrains.Annotations;
using UnityEngine;
using T = PrimeTween.TweenSettings<float>;

namespace PrimeTween {
    internal static class Constants {
        internal const string onCompleteCallbackIgnored = "Tween's " + nameof(Tween.OnComplete) + " callback was ignored.";
        internal const string durationInvalidError = "Tween's duration is invalid.";

        internal const string cantManipulateNested = "It's not allowed to manipulate 'nested' tweens and sequences, please use the parent Sequence instead.\n" +
                                                                  "When a tween or sequence is added to another sequence, they become 'nested', and manipulating them directly is no longer allowed.\n" +
                                                                  "Use Stop()/Complete()/isPaused/timeScale/elapsedTime/etc. of their parent Sequence instead.\n";
        [NotNull]
        internal static string buildWarningCanBeDisabledMessage(string settingName) {
            return $"To disable this warning, set '{nameof(PrimeTweenConfig)}.{settingName} = false;'.";
        }

        internal const string isDeadMessage = "Tween/Sequence is not alive. Please check the 'isAlive' property before calling this API.\n";
        internal const string unscaledTimeTooltip = "The tween will use real time, ignoring Time.timeScale.";
        internal const string easeTooltip = "The easing curve of an animation.\n\n" +
                                              "Default is Ease." + nameof(Ease.OutQuad) + ". The Default ease can be modified via '" + nameof(PrimeTweenConfig) + "." + nameof(PrimeTweenConfig.defaultEase) + "' setting.\n\n" +
                                              "Set to " + nameof(Ease) + "." + nameof(Ease.Custom) + " to control the easing with custom " + nameof(AnimationCurve) + ".";
        internal const string cyclesTooltip = "Setting cycles to -1 will repeat the tween indefinitely.";
        internal const string cycleModeTooltip = "See the documentation of each cycle mode by hoovering the dropdown.";
        internal const string defaultCtorError = "Tween or Sequence is not created properly.\n" +
                                                 "- Use Sequence." + nameof(Sequence.Create) + "() to start a Sequence.\n" +
                                                 "- Use static 'Tween.' methods to start a Tween.\n";
        internal const string startDelayTooltip = "Delays the start of a tween.";
        internal const string endDelayTooltip = "Delays the completion of a tween.\n\n" +
                                                "For example, can be used to add the delay between cycles.\n\n" +
                                                "Or can be used to postpone the execution of the onComplete callback.";
        internal const string updateTypeTooltip = "Controls Unity's event function, which updates the animation.\n\n" +
                                                  "Default is MonoBehaviour.Update().";
        internal const string infiniteTweenInSequenceError = "It's not allowed to have infinite tweens (cycles == -1) in a sequence. If you want the sequence to repeat forever, " + nameof(Sequence.SetRemainingCycles) + "(-1) on the parent sequence instead."; // todo allow this as the last animation in the Sequence? it would still not be possible to Chain anything after the infinite animation, but will unlock use cases like adding startDelay to the Sequence. I can't do by adding startDelay support to Sequence because startDelay works differently for tweens: it's applied every tween loop
        internal const string customTweensDontSupportStartFromCurrentWarning =
            "Custom tweens don't support the '" + nameof(T.startFromCurrent) + "' because they don't know the current value of animated property.\n" +
            "This means that the animated value will be changed abruptly if a new tween is started mid-way.\n" +
            "Please pass the current value to the '" + nameof(T) + "." + nameof(T.WithDirection) + "(bool toEndValue, T currentValue)' method or use the constructor that accepts the '" + nameof(T.startValue) + "'.\n";
        internal const string startFromCurrentTooltip = "If true, the current value of an animated property will be used instead of the 'startValue'.\n\n" +
                                                        "This field typically should not be manipulated directly. Instead, it's set by TweenSettings(T endValue, TweenSettings settings) constructor or by " + nameof(T.WithDirection) + "() method.";
        internal const string startValueTooltip = "Start value of a tween.\n\n" +
                                                  "For example, if you're animating a window, the 'startValue' can represent the closed (off-screen) position of the window.";
        internal const string endValueTooltip = "End value of a tween.\n\n" +
                                                "For example, if you're animating a window, the 'endValue' can represent the opened position of the window.";
        internal const string setTweensCapacityMethod = "'" + nameof(PrimeTweenConfig) + "." + nameof(PrimeTweenConfig.SetTweensCapacity) + "(int capacity)'";
        internal const string maxAliveTweens = "Max alive tweens";
        internal const string animationAlreadyStarted = "Animation has already been started, it's not allowed to manipulate it anymore.";
        internal const string recursiveCallError = "Please don't call this API recursively from Tween.Custom() or tween.OnUpdate().";
        internal const string nestSequenceTwiceError = "Sequence can be nested in other sequence only once.";
        internal const string nestTweenTwiceError = "A tween can be added to a sequence only once and can only belong to one sequence.";
        internal const string addDeadTweenToSequenceError = "It's not allowed to add 'dead' tweens to a sequence.";
    }
}
