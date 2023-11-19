using UnityEngine;
using System;


namespace VirtueSky.Tween
{
    /// <summary>
    /// The Tween class stores the animation information;
    /// </summary>
    public class Tween
    {
        public float from, to, restTime, originalTime, progress;
        public Vector2 fromVector2, toVector2;
        public Vector3 fromVector3, toVector3;
        public Quaternion fromQuaternion, toQuaternion;
        public Color fromColor, toColor;
        public Func<float, float, float, float> easeFunctionDelegate;
        private Action<float> valueSetter;
        private Action<Vector2> valueSetterVector2;
        private Action<Vector3> valueSetterVector3;
        private Action<Quaternion> valueSetterQuaternion;
        private Action<Color> valueSetterColor;
        public Func<float> deltaTimeDelegate;
        public Action OnComplete;
        public const Action doNothing = null;
        public TweenType type = TweenType.f;
        public TweenRepeat repeat = TweenRepeat.Once;
        public Coroutine animationRoutine;

        public enum TweenType
        {
            delay,
            f,
            v2,
            v3,
            col,
            quat
        }

        #region Tween Constructors

        /// <summary>
        /// Use this for animating float values. The first parameter has to a setter for the float
        /// </summary>
        /// <param name="valueSetter">Value setter.</param>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="length">Length.</param>
        /// <param name="easeType">Ease type.</param>
        /// <param name="unscaled">If set to <c>true</c> unscaled.</param>
        /// <param name="repeat">Repeat.</param>
        /// <param name="OnComplete">On complete.</param>
        public Tween(Action<float> valueSetter, float from, float to, float length, EasingTypes easeType = EasingTypes.Linear, bool unscaled = false,
            TweenRepeat repeat = TweenRepeat.Once, Action OnComplete = doNothing)
        {
            this.valueSetter = valueSetter;
            this.from = from;
            this.to = to;
            this.easeFunctionDelegate = EasingFunctions.Function(easeType);
            this.restTime = this.originalTime = length;
            this.OnComplete = OnComplete;
            this.type = TweenType.f;
            this.repeat = repeat;
            this.deltaTimeDelegate = TimeFunction(unscaled);
        }

        /// <summary>
        /// Use this for animating a Vector2
        /// </summary>
        /// <param name="valueSetter">Value setter.</param>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="length">Length.</param>
        /// <param name="easeType">Ease type.</param>
        /// <param name="unscaled">If set to <c>true</c> unscaled.</param>
        /// <param name="repeat">Repeat.</param>
        /// <param name="OnComplete">On complete.</param>
        public Tween(Action<Vector2> valueSetter, Vector2 from, Vector2 to, float length, EasingTypes easeType = EasingTypes.Linear, bool unscaled = false,
            TweenRepeat repeat = TweenRepeat.Once, Action OnComplete = doNothing)
        {
            this.valueSetterVector2 = valueSetter;
            this.fromVector2 = from;
            this.toVector2 = to;
            this.easeFunctionDelegate = EasingFunctions.Function(easeType);
            this.restTime = this.originalTime = length;
            this.OnComplete = OnComplete;
            this.type = TweenType.v2;
            this.repeat = repeat;
            this.deltaTimeDelegate = TimeFunction(unscaled);
        }

        /// <summary>
        /// Use this for animating a Vector3
        /// </summary>
        /// <param name="valueSetter">Value setter.</param>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="length">Length.</param>
        /// <param name="easeType">Ease type.</param>
        /// <param name="unscaled">If set to <c>true</c> unscaled.</param>
        /// <param name="repeat">Repeat.</param>
        /// <param name="OnComplete">On complete.</param>
        public Tween(Action<Vector3> valueSetter, Vector3 from, Vector3 to, float length, EasingTypes easeType = EasingTypes.Linear, bool unscaled = false,
            TweenRepeat repeat = TweenRepeat.Once, Action OnComplete = doNothing)
        {
            this.valueSetterVector3 = valueSetter;
            this.fromVector3 = from;
            this.toVector3 = to;
            this.easeFunctionDelegate = EasingFunctions.Function(easeType);
            this.restTime = this.originalTime = length;
            this.OnComplete = OnComplete;
            this.type = TweenType.v3;
            this.repeat = repeat;
            this.deltaTimeDelegate = TimeFunction(unscaled);
        }

        /// <summary>
        /// Use this for animating rotations
        /// </summary>
        /// <param name="valueSetter">Value setter.</param>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="length">Length.</param>
        /// <param name="easeType">Ease type.</param>
        /// <param name="unscaled">If set to <c>true</c> unscaled.</param>
        /// <param name="repeat">Repeat.</param>
        /// <param name="OnComplete">On complete.</param>
        public Tween(Action<Quaternion> valueSetter, Quaternion from, Quaternion to, float length, EasingTypes easeType = EasingTypes.Linear, bool unscaled = false,
            TweenRepeat repeat = TweenRepeat.Once, Action OnComplete = doNothing)
        {
            this.valueSetterQuaternion = valueSetter;
            this.fromQuaternion = from;
            this.toQuaternion = to;
            this.easeFunctionDelegate = EasingFunctions.Function(easeType);
            this.restTime = this.originalTime = length;
            this.OnComplete = OnComplete;
            this.type = TweenType.quat;
            this.repeat = repeat;
            this.deltaTimeDelegate = TimeFunction(unscaled);
        }

        /// <summary>
        /// Use this for animating colours
        /// </summary>
        /// <param name="valueSetter">Value setter.</param>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="length">Length.</param>
        /// <param name="easeType">Ease type.</param>
        /// <param name="unscaled">If set to <c>true</c> unscaled.</param>
        /// <param name="repeat">Repeat.</param>
        /// <param name="OnComplete">On complete.</param>
        public Tween(Action<Color> valueSetter, Color from, Color to, float length, EasingTypes easeType = EasingTypes.Linear, bool unscaled = false,
            TweenRepeat repeat = TweenRepeat.Once, Action OnComplete = doNothing)
        {
            this.valueSetterColor = valueSetter;
            this.fromColor = from;
            this.toColor = to;
            this.easeFunctionDelegate = EasingFunctions.Function(easeType);
            this.restTime = this.originalTime = length;
            this.OnComplete = OnComplete;
            this.type = TweenType.col;
            this.repeat = repeat;
            this.deltaTimeDelegate = TimeFunction(unscaled);
        }

        public Tween(float seconds)
        {
            this.originalTime = seconds;
            this.type = TweenType.delay;
        }

        #endregion

        #region Animation

        public void Play()
        {
            TweenManager.PlayTween(this);
        }

        public void Stop()
        {
            if (animationRoutine != null)
                TweenManager.StopTween(animationRoutine);
        }

        public void Reset()
        {
            this.restTime = this.originalTime;
        }

        #endregion

        #region Tween Value Setters

        public Vector2 Vector2Value
        {
            set { this.valueSetterVector2(value); }
        }

        public Vector3 Vector3Value
        {
            set { this.valueSetterVector3(value); }
        }

        public Quaternion QuaternionValue
        {
            set { this.valueSetterQuaternion(value); }
        }

        public Color ColorValue
        {
            set { this.valueSetterColor(value); }
        }

        public float FloatValue
        {
            set { this.valueSetter(value); }
        }

        #endregion

        #region Tween Helper Functions

        public void SwitchTargets()
        {
            Swap<float>(ref from, ref to);
            Swap<Vector2>(ref fromVector2, ref toVector2);
            Swap<Vector3>(ref fromVector3, ref toVector3);
            Swap<Color>(ref fromColor, ref toColor);
        }

        public static void Swap<T>(ref T param1, ref T param2)
        {
            T tmp = param1;
            param1 = param2;
            param2 = tmp;
        }

        public static Func<float> TimeFunction(bool unscaled)
        {
            return unscaled ? (Func<float>)(() => Time.unscaledDeltaTime) : (Func<float>)(() => Time.deltaTime);
        }

        #endregion
    }
}