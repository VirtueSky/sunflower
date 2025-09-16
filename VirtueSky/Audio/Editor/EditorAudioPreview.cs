using UnityEditor;
using UnityEngine;
using System;
using System.Reflection;

namespace VirtueSky.AudioEditor
{
    internal static class EditorAudioPreview
    {
        static readonly Type AudioUtilType;
        static readonly MethodInfo PlayMethod;
        static readonly MethodInfo StopAllMethod;
        static readonly MethodInfo StopOneMethod;

        // Hỗ trợ nhiều biến thể kiểm tra trạng thái phát
        static readonly MethodInfo IsPlayingClipMethod; // IsPreviewClipPlaying(AudioClip)
        static readonly MethodInfo IsPlayingNoArgMethod; // IsPreviewClipPlaying()
        static readonly MethodInfo GetPreviewClipMethod; // GetPreviewClip()

        static EditorAudioPreview()
        {
            AudioUtilType = typeof(AudioImporter).Assembly.GetType("UnityEditor.AudioUtil");
            var flags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

            // PlayPreviewClip(clip, startSample, loop[, honorPlayOneShot?])
            PlayMethod = AudioUtilType.GetMethod("PlayPreviewClip", flags, null,
                             new Type[] { typeof(AudioClip), typeof(int), typeof(bool) }, null)
                         ?? AudioUtilType.GetMethod("PlayPreviewClip", flags, null,
                             new Type[] { typeof(AudioClip), typeof(int), typeof(bool), typeof(bool) }, null);

            StopAllMethod = AudioUtilType.GetMethod("StopAllPreviewClips", flags);
            StopOneMethod = AudioUtilType.GetMethod("StopPreviewClip", flags, null,
                new Type[] { typeof(AudioClip) }, null);

            IsPlayingClipMethod = AudioUtilType.GetMethod("IsPreviewClipPlaying", flags, null,
                new Type[] { typeof(AudioClip) }, null);
            IsPlayingNoArgMethod = AudioUtilType.GetMethod("IsPreviewClipPlaying", flags, null,
                Type.EmptyTypes, null);
            GetPreviewClipMethod = AudioUtilType.GetMethod("GetPreviewClip", flags);
        }

        public static void Play(AudioClip clip, bool loop = false, int startSample = 0)
        {
            if (!clip || PlayMethod == null) return;
            var args = PlayMethod.GetParameters().Length == 3
                ? new object[] { clip, startSample, loop }
                : new object[] { clip, startSample, loop, false };
            PlayMethod.Invoke(null, args);
        }

        public static void Stop(AudioClip clip = null)
        {
            if (clip != null && StopOneMethod != null) StopOneMethod.Invoke(null, new object[] { clip });
            else if (StopAllMethod != null) StopAllMethod.Invoke(null, null);
        }

        public static bool IsPlaying(AudioClip clip = null)
        {
            try
            {
                if (IsPlayingClipMethod != null)
                    return (bool)IsPlayingClipMethod.Invoke(null, new object[] { clip });

                if (IsPlayingNoArgMethod != null)
                    return (bool)IsPlayingNoArgMethod.Invoke(null, null);

                if (GetPreviewClipMethod != null)
                {
                    var cur = GetPreviewClipMethod.Invoke(null, null) as AudioClip;
                    return clip ? cur == clip : cur != null;
                }
            }
            catch
            {
                /* ignore reflection issues */
            }

            return false;
        }
    }
}