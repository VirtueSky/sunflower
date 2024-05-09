using System;

namespace VirtueSky.Audio
{
    [Serializable]
    public class SoundCache
    {
        internal int key;
        internal SoundData soundData;

        public SoundCache(int _key, SoundData _soundData)
        {
            key = _key;
            soundData = _soundData;
        }
    }
}