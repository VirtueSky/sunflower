#if VIRTUESKY_ANIMANCER
using Animancer;
using System;
using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Component
{
    [RequireComponent(typeof(AnimancerComponent))]
    [EditorIcon("icon_csharp")]
    public class HandleAnimancerComponent : MonoBehaviour
    {
        [SerializeField] private AnimancerComponent animancerComponent;

        public void PlayAnim(ClipTransition clip, Action _endAnim = null, float _durationFade = .2f,
            FadeMode mode = default)
        {
            if (!animancerComponent.IsPlaying(clip))
            {
                var state = animancerComponent.Play(clip, clip.Clip.length * _durationFade, mode);
                if (_endAnim != null)
                {
                    state.Events.OnEnd += OnEndAnim;

                    void OnEndAnim()
                    {
                        state.Events.OnEnd -= OnEndAnim;
                        _endAnim?.Invoke();
                    }
                }
            }
        }

        // Freeze a single animation on its current frame:
        public void PauseClip(ClipTransition clip)
        {
            animancerComponent.States[clip].IsPlaying = false;
        }

        // Freeze all animations on their current frame:
        public void PauseAll()
        {
            animancerComponent.Playable.PauseGraph();
        }

        // Stop a single animation from affecting the character and rewind it to the start:
        public void StopClip(ClipTransition clip)
        {
            animancerComponent.Stop(clip);

            // Or you can call it on the state directly:
            var state = animancerComponent.States[clip];
            state.Stop();
        }

        // Stop all animations from affecting the character and rewind them to the start:
        public void StopAll()
        {
            animancerComponent.Stop();
        }

#if UNITY_EDITOR
        private void Reset()
        {
            if (animancerComponent == null)
            {
                animancerComponent = GetComponent<AnimancerComponent>();
            }
        }
#endif
    }
}
#endif