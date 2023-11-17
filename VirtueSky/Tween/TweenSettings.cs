namespace VirtueSky.Tween
{
    using UnityEngine;


    /// <summary>
    /// Editing and Displaying Tween information in the Editor 
    /// </summary>
    [System.Serializable]
    public class TweenSettings
    {
        [Tooltip("Wheter easing is enabled. Whether this has any effect depends on the enclosing class.")]
        public bool eased = true;

        public EasingTypes easing = EasingTypes.QuadraticOut;
        public float animationLength = 1f;

        [Tooltip("Whether timescale affects the easing")]
        public bool unscaled = false;
    }
}