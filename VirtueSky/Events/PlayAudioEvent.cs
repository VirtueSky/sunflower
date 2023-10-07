using System.Collections.Generic;
using UnityEngine;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Event/Play Audio Event")]
    public class PlayAudioEvent : BaseEvent<AudioClip>, ISerializationCallbackReceiver
    {
        Dictionary<AudioClip, float> lastTimePlayDict = new Dictionary<AudioClip, float>();

        public override void Raise(AudioClip value)
        {
            if (!lastTimePlayDict.ContainsKey(value))
            {
                lastTimePlayDict.Add(value, 0);
            }

            if (Time.unscaledTime - lastTimePlayDict[value] < 0.1f)
            {
                return;
            }

            lastTimePlayDict[value] = Time.unscaledTime;
            base.Raise(value);
        }

        public void RaiseRandom(AudioClip[] audioClips)
        {
            Raise(audioClips[Random.Range(0, audioClips.Length)]);
        }

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            lastTimePlayDict = new Dictionary<AudioClip, float>();
        }
    }
}