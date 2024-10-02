namespace VirtueSky.Audio
{
    public static class AudioHelper
    {
        public static SoundCache PlaySfx(this SoundData soundData, PlaySfxEvent playSfxEvent) => playSfxEvent.Raise(soundData);
        public static void PauseSfx(this SoundCache soundCache, PauseSfxEvent pauseSfxEvent) => pauseSfxEvent.Raise(soundCache);
        public static void StopSfx(this SoundCache soundCache, StopSfxEvent stopSfxEvent) => stopSfxEvent.Raise(soundCache);
        public static void ResumeSfx(this SoundCache soundCache, ResumeSfxEvent resumeSfxEvent) => resumeSfxEvent.Raise(soundCache);
        public static void FinishSfx(this SoundCache soundCache, FinishSfxEvent finishSfxEvent) => finishSfxEvent.Raise(soundCache);
        public static void StopAllSfx(this StopAllSfxEvent stopAllSfxEvent) => stopAllSfxEvent.Raise();

        public static void PlayMusic(this SoundData soundData, PlayMusicEvent playMusicEvent) => playMusicEvent.Raise(soundData);
        public static void StopMusic(this StopMusicEvent stopMusicEvent) => stopMusicEvent.Raise();
        public static void PauseMusic(this PauseMusicEvent pauseMusicEvent) => pauseMusicEvent.Raise();
        public static void ResumeMusic(this ResumeMusicEvent resumeMusicEvent) => resumeMusicEvent.Raise();
    }
}