using System;

#if VIRTUESKY_ANIMANCER
using Animancer;

namespace VirtueSky.Misc
{
    public static partial class Common
    {
        public static AnimancerComponent PlayClip(this AnimancerComponent animancerComponent, ClipTransition clip,
            Action _endAnim = null, float _durationFade = .2f, bool isCheckPlayingClip = true,
            FadeMode mode = default)
        {
            if (isCheckPlayingClip)
            {
                if (!animancerComponent.IsPlaying(clip))
                {
                    Handle();
                }
            }
            else
            {
                Handle();
            }

            void Handle()
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

            return animancerComponent;
        }

        // Freeze a single animation on its current frame:
        public static AnimancerComponent PauseClip(this AnimancerComponent animancerComponent, ClipTransition clip)
        {
            animancerComponent.States[clip].IsPlaying = false;
            return animancerComponent;
        }

        // Freeze all animations on their current frame:
        public static AnimancerComponent PauseAll(this AnimancerComponent animancerComponent)
        {
            animancerComponent.Playable.PauseGraph();
            return animancerComponent;
        }

        // Stop a single animation from affecting the character and rewind it to the start:
        public static AnimancerComponent StopClip(this AnimancerComponent animancerComponent, ClipTransition clip)
        {
            animancerComponent.Stop(clip);

            // Or you can call it on the state directly:
            var state = animancerComponent.States[clip];
            state.Stop();
            return animancerComponent;
        }

        // Stop all animations from affecting the character and rewind them to the start:
        public static AnimancerComponent StopAll(this AnimancerComponent animancerComponent)
        {
            animancerComponent.Stop();
            return animancerComponent;
        }
    }
}
#endif