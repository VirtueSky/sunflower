using JetBrains.Annotations;
using UnityEngine;
using T = PrimeTween.TweenSettings<float>;

namespace PrimeTween
{
    internal static class Constants
    {
        internal const string onCompleteCallbackIgnored = "Tween's " + nameof(Tween.OnComplete) + " callback was ignored.";
        internal const string targetDestroyed = "Tween's target has been destroyed.";
        internal const string durationInvalidError = "Tween's duration is invalid.";

        internal const string setPauseOnTweenInsideSequenceError =
            "Setting Tween/Sequence.isPaused inside other sequence is not allowed because all tweens in a sequence should have the same isPaused. Consider using Sequence.isPaused instead.";

        [NotNull]
        internal static string buildWarningCanBeDisabledMessage(string settingName)
        {
            return $"To disable this warning, disable the '{nameof(PrimeTweenConfig)}.{settingName}' setting.";
        }

        internal const string isDeadMessage = "Tween/Sequence is not alive. Please check the 'isAlive' property before calling this API.";

        internal static bool validateIsAlive(bool _isAlive)
        {
            if (!_isAlive)
            {
                Debug.LogError(isDeadMessage);
            }

            return _isAlive;
        }

        internal const string unscaledTimeTooltip = "The tween will use real time, ignoring Time.timeScale.";
        internal const string cyclesTooltip = "Setting cycles to -1 will repeat the tween indefinitely.";

        internal const string defaultSequenceCtorError = "please use Sequence." + nameof(Sequence.Create) + "() or Sequence." + nameof(Sequence.Create) +
                                                         "(Tween firstTween) instead of parameterless constructor.\n" +
                                                         "Because Sequence is a struct, the parameterless contructor can't be used to create a valid empty Sequence.\n";

        internal const string startDelayTooltip = "Delays the start of a tween.";

        internal const string endDelayTooltip = "Delays the completion of a tween.\n\n" +
                                                "For example, can be used to add the delay between cycles.\n\n" +
                                                "Or can be used to postpone the execution of the onComplete callback.";

        internal const string infiniteTweenInSequenceError = "It's not allowed to have infinite tweens (cycles == -1) in a sequence. If you want the sequence to repeat forever, " +
                                                             nameof(Sequence.SetCycles) + "(-1) on the sequence instead.";

        internal const string customTweensDontSupportStartFromCurrentWarning =
            "Custom tweens don't support the '" + nameof(T.startFromCurrent) + "' because they don't know the current value of animated property.\n" +
            "This means that the animated value will be changed abruptly if a new tween is started mid-way.\n" +
            "Please pass the current value to the '" + nameof(T) + "." + nameof(T.WithDirection) +
            "(bool toEndValue, T currentValue)' method or use the constructor that accepts the '" + nameof(T.startValue) + "'.\n";

        internal const string startFromCurrentTooltip = "If true, the current value of an animated property will be used instead of the 'startValue'.\n\n" +
                                                        "This field typically should not be manipulated directly. Instead, it's set by TweenSettings(T endValue, TweenSettings settings) constructor or by " +
                                                        nameof(T.WithDirection) + "() method.";

        internal const string startValueTooltip = "Start value of a tween.\n\n" +
                                                  "For example, if you're animating a window, the 'startValue' can represent the closed (off-screen) position of the window.";

        internal const string endValueTooltip = "End value of a tween.\n\n" +
                                                "For example, if you're animating a window, the 'endValue' can represent the opened position of the window.";

        internal const string setTweensCapacityMethod = "'" + nameof(PrimeTweenConfig) + "." + nameof(PrimeTweenConfig.SetTweensCapacity) + "(int capacity)'";
        internal const string maxAliveTweens = "Max alive tweens";

#if UNITY_EDITOR
        internal const string editModeWarning = "Please don't call PrimeTween's API in Edit mode (while the scene is not playing).";

        internal static bool warnNoInstance
        {
            get
            {
                if (noInstance)
                {
                    Debug.LogWarning(editModeWarning);
                    return true;
                }

                return false;
            }
        }

        internal static bool noInstance => PrimeTweenManager.Instance == null;

        internal static bool isEditMode
        {
            get
            {
                try
                {
                    return !UnityEditor.EditorApplication.isPlaying;
                }
                catch
                {
                    return true;
                }
            }
        }
#endif // UNITY_EDITOR
    }
}