using UnityEngine;
using VirtueSky.Core;
using VirtueSky.ObjectPooling;

namespace VirtueSky.Misc
{
    public class AudioManager : BaseMono
    {
        [SerializeField] private Pools pool;
        [SerializeField] private AudioComponent audioComponentPrefab;

        private void Awake()
        {
            pool.Initialize();
        }

        private void PlaySfx(AudioData audioData)
        {
            var audioComponent = pool.Spawn(audioComponentPrefab);
        }
    }
}