using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VirtueSky.Audio;
using VirtueSky.UtilsEditor;

namespace VirtueSky.AudioEditor
{
    [CustomEditor(typeof(SoundData))]
    public class SoundDataEditor : Editor
    {
        SerializedProperty clipsProp;

        // State preview
        int selectedIndex = 0;
        float playheadSec = 0f;
        float loopA = -1f, loopB = -1f;
        bool selectingLoop = false;

        AudioClip activeClip;
        AudioClip lastPlayed;
        double lastTickTime;
        bool isPlaying = false;
        int lastKnownSamplePosition = 0;

        const float TimebarH = 22f;
        const float LaneH = 38f;

        void OnEnable()
        {
            clipsProp = serializedObject.FindProperty("audioClips");
            EditorApplication.update += OnEditorUpdate;
        }

        void OnDisable()
        {
            EditorApplication.update -= OnEditorUpdate;

            isPlaying = false;
            EditorAudioPreview.Stop();
            lastPlayed = null;
            activeClip = null;
        }

        GUIStyle StyleLabel()
        {
            var style = new GUIStyle();
            style.fontSize = 14;
            style.normal.textColor = Color.white;
            return style;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space(10);
            Uniform.DrawGroupFoldout("preview_sound_data", "Editor Preview", DrawPreview, true);
            serializedObject.Update();
            serializedObject.ApplyModifiedProperties();
        }

        void DrawPreview()
        {
            int total = clipsProp != null ? clipsProp.arraySize : 0;
            if (total <= 0)
            {
                EditorGUILayout.HelpBox("Add AudioClip to preview.", MessageType.Info);
                return;
            }

            List<int> indexMap = new List<int>(total);
            List<string> options = new List<string>(total);

            for (int i = 0; i < total; i++)
            {
                var el = clipsProp.GetArrayElementAtIndex(i);
                var clip = el.objectReferenceValue as AudioClip;
                if (clip != null)
                {
                    indexMap.Add(i);
                    options.Add($"{i} - {clip.name}");
                }
            }

            if (indexMap.Count == 0)
            {
                EditorGUILayout.HelpBox(
                    "The AudioClips list has elements but they are all null. Drag and drop AudioClips into it or delete the null elements.",
                    MessageType.Info
                );

                if (isPlaying)
                {
                    isPlaying = false;
                    EditorAudioPreview.Stop();
                    lastPlayed = null;
                    activeClip = null;
                }

                return;
            }

            selectedIndex = Mathf.Clamp(selectedIndex, 0, indexMap.Count - 1);

            int newSel = EditorGUILayout.Popup("Preview Clip", selectedIndex, options.ToArray());

            int propIndex = indexMap[newSel];
            var selEl = clipsProp.GetArrayElementAtIndex(propIndex);
            var sel = selEl.objectReferenceValue as AudioClip;

            if (newSel != selectedIndex || sel != activeClip)
            {
                isPlaying = false;
                EditorAudioPreview.Stop(activeClip);
                selectedIndex = newSel;
                activeClip = sel;
                playheadSec = 0f;
                lastPlayed = null;
            }

            using (new EditorGUI.DisabledScope(activeClip == null))
            {
                DrawTimeline(activeClip);
                DrawTransport(activeClip);
            }
        }

        /// <summary>
        /// Update per frame when editor is active
        /// </summary>
        void OnEditorUpdate()
        {
            if (!activeClip || !isPlaying) return;

            // Sync playhead with actual audio playback position
            float actualPosition = GetAudioPreviewPosition(activeClip);
            
            if (actualPosition >= 0f)
            {
                playheadSec = actualPosition;
            }
            else
            {
                // Fallback to time-based calculation if position can't be retrieved
                double now = EditorApplication.timeSinceStartup;
                double dt = System.Math.Max(0, now - lastTickTime);
                lastTickTime = now;
                playheadSec += (float)dt;
            }

            if (HasLoop())
            {
                float a = Mathf.Min(loopA, loopB);
                float b = Mathf.Max(loopA, loopB);
                if (playheadSec > b)
                {
                    playheadSec = a;
                    if (isPlaying)
                    {
                        int startSample = Mathf.Clamp(Mathf.RoundToInt(a * activeClip.frequency), 0,
                            activeClip.samples - 1);
                        EditorAudioPreview.Play(activeClip, false, startSample);
                        lastTickTime = EditorApplication.timeSinceStartup;
                    }
                }
            }
            else if (playheadSec > activeClip.length)
            {
                // Auto-return to 0 and stop
                isPlaying = false;
                EditorAudioPreview.Stop(activeClip);
                lastPlayed = null;
                playheadSec = 0f;
            }

            // Always repaint when playing to ensure smooth animation
            Repaint();
        }
        
        /// <summary>
        /// Get current audio preview position using reflection
        /// </summary>
        float GetAudioPreviewPosition(AudioClip clip)
        {
            if (clip == null) return -1f;
            
            try
            {
                var unityEditorAssembly = typeof(AudioImporter).Assembly;
                var audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
                
                // Try to get the playing sample position
                var method = audioUtilClass.GetMethod("GetClipSamplePosition",
                    System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
                
                if (method != null)
                {
                    var result = method.Invoke(null, new object[] { clip });
                    if (result != null)
                    {
                        int samplePosition = (int)result;
                        if (samplePosition >= 0 && clip.frequency > 0)
                        {
                            return (float)samplePosition / clip.frequency;
                        }
                    }
                }
            }
            catch
            {
                // Fallback to time-based calculation
            }
            
            return -1f;
        }

        // ====== Timeline UI ======
        void DrawTimeline(AudioClip clip)
        {
            // Timebar (ticks)
            var timeRect = GUILayoutUtility.GetRect(0, 1000, TimebarH, TimebarH);
            EditorGUI.DrawRect(timeRect, new Color(0.11f, 0.11f, 0.11f));
            DrawTicks(timeRect, clip.length);

            // Lane (playhead + loop A–B)
            var laneRect = GUILayoutUtility.GetRect(0, 1000, LaneH, LaneH);
            EditorGUI.DrawRect(laneRect, new Color(0.09f, 0.09f, 0.09f));

            if (HasLoop())
            {
                float a = Mathf.Clamp(Mathf.Min(loopA, loopB), 0, clip.length);
                float b = Mathf.Clamp(Mathf.Max(loopA, loopB), 0, clip.length);
                var r = TimeRangeToRect(laneRect, 0, clip.length, a, b);
                EditorGUI.DrawRect(r, new Color(0.2f, 0.6f, 0.3f, 0.20f));
                DrawBorder(r, new Color(0.3f, 0.9f, 0.5f, 0.85f));
            }

            // Playhead
            float px = TimeToPixel(laneRect, 0, clip.length, playheadSec);
            EditorGUI.DrawRect(new Rect(px, laneRect.y, 2f, laneRect.height), new Color(1f, 0.85f, 0.2f));

            HandleLaneInput(laneRect, clip.length);
        }

        void DrawTransport(AudioClip clip)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                string playLabel = isPlaying ? "Restart" : "Play from Head";
                if (GUILayout.Button(playLabel, GUILayout.Height(22)))
                {
                    if (clip)
                    {
                        if (isPlaying)
                        {
                            // Dừng clip đang phát trước khi phát lại
                            EditorAudioPreview.Stop(clip);
                        }

                        if (lastPlayed && lastPlayed != clip) EditorAudioPreview.Stop(lastPlayed);

                        lastPlayed = clip;
                        isPlaying = true;

                        int startSample = Mathf.Clamp(Mathf.RoundToInt(playheadSec * clip.frequency), 0,
                            clip.samples - 1);
                        lastKnownSamplePosition = startSample;
                        EditorAudioPreview.Play(clip, false, startSample);
                        lastTickTime = EditorApplication.timeSinceStartup;
                    }
                }

                if (GUILayout.Button("Stop", GUILayout.Height(22)))
                {
                    if (clip)
                    {
                        isPlaying = false;
                        EditorAudioPreview.Stop();
                        lastPlayed = null;
                        playheadSec = 0f; // Reset playhead to beginning when stopped
                        Repaint();
                    }
                }

                if (GUILayout.Button("To Start", GUILayout.Height(22)))
                {
                    if (clip)
                    {
                        playheadSec = 0f;
                        lastKnownSamplePosition = 0;
                        if (isPlaying)
                        {
                            EditorAudioPreview.Play(clip, false, 0);
                            lastTickTime = EditorApplication.timeSinceStartup;
                        }
                        else
                        {
                            Repaint(); // Only repaint if not playing to update the UI
                        }
                    }
                }

                if (GUILayout.Button("↺ Set Loop A", GUILayout.Height(22)))
                {
                    if (clip) loopA = Mathf.Clamp(playheadSec, 0, clip.length);
                }

                if (GUILayout.Button("↻ Set Loop B", GUILayout.Height(22)))
                {
                    if (clip) loopB = Mathf.Clamp(playheadSec, 0, clip.length);
                }

                if (GUILayout.Button("Set Loop Full", GUILayout.Height(22)))
                {
                    if (clip)
                    {
                        loopA = 0f;
                        loopB = clip.length;
                        if (isPlaying)
                        {
                            playheadSec = 0f;
                            lastKnownSamplePosition = 0;
                            EditorAudioPreview.Play(clip, false, 0);
                            lastTickTime = EditorApplication.timeSinceStartup;
                        }
                        else
                        {
                            Repaint(); // Only repaint if not playing to update the UI
                        }
                    }
                }

                if (GUILayout.Button("Clear Loop", GUILayout.Height(22)))
                {
                    loopA = loopB = -1f;
                    Repaint(); // Repaint to update the UI immediately
                }
            }

            EditorGUILayout.LabelField(
                $"Clip: {clip.name} • Length: {clip.length:0.000}s • Head: {playheadSec:0.000}s" +
                (HasLoop() ? $" • Loop [{Mathf.Min(loopA, loopB):0.000} – {Mathf.Max(loopA, loopB):0.000}]s" : "")
            );

            EditorGUILayout.HelpBox(
                "Loop preview is controlled only by Set Loop A/B/Full or Clear Loop. When there is no A–B: playback will automatically return to 0s (auto-return).",
                MessageType.Info
            );
        }

        // ====== Helpers ======
        bool HasLoop() => loopA >= 0f && loopB >= 0f && !Mathf.Approximately(loopA, loopB);

        void HandleLaneInput(Rect r, float len)
        {
            var e = Event.current;
            if (!r.Contains(e.mousePosition) || len <= 0f) return;

            if (e.type == EventType.MouseDown && e.button == 0)
            {
                playheadSec = Mathf.Clamp(PixelToTime(r, 0, len, e.mousePosition.x), 0, len);
                // When user manually sets the playhead, update the audio preview
                if (isPlaying && activeClip)
                {
                    int startSample = Mathf.Clamp(Mathf.RoundToInt(playheadSec * activeClip.frequency), 0,
                        activeClip.samples - 1);
                    lastKnownSamplePosition = startSample;
                    EditorAudioPreview.Play(activeClip, false, startSample);
                    lastTickTime = EditorApplication.timeSinceStartup;
                }
                e.Use();
                Repaint(); // Force repaint immediately to update the UI
            }
            else if (e.type == EventType.MouseDrag && e.button == 0)
            {
                playheadSec = Mathf.Clamp(PixelToTime(r, 0, len, e.mousePosition.x), 0, len);
                // When dragging the playhead, update the audio preview to match the position
                if (isPlaying && activeClip)
                {
                    int startSample = Mathf.Clamp(Mathf.RoundToInt(playheadSec * activeClip.frequency), 0,
                        activeClip.samples - 1);
                    lastKnownSamplePosition = startSample;
                    EditorAudioPreview.Play(activeClip, false, startSample);
                    lastTickTime = EditorApplication.timeSinceStartup;
                }
                e.Use();
                Repaint(); // Force repaint immediately to update the UI
            }

            if (e.type == EventType.MouseDown && e.button == 1)
            {
                loopA = loopB = Mathf.Clamp(PixelToTime(r, 0, len, e.mousePosition.x), 0, len);
                selectingLoop = true;
                e.Use();
            }
            else if (e.type == EventType.MouseDrag && e.button == 1 && selectingLoop)
            {
                loopB = Mathf.Clamp(PixelToTime(r, 0, len, e.mousePosition.x), 0, len);
                e.Use();
            }
            else if (e.type == EventType.MouseUp && e.button == 1 && selectingLoop)
            {
                selectingLoop = false;
                e.Use();
            }
        }

        void DrawTicks(Rect r, float total)
        {
            if (total <= 0f) return;

            float step = NiceStep(total);
            Handles.BeginGUI();
            Handles.color = new Color(0.6f, 0.6f, 0.6f, 0.7f);

            for (float t = 0f; t <= total + 0.0001f; t += step)
            {
                float x = Mathf.Lerp(r.x, r.xMax, t / total);
                Handles.DrawLine(new Vector2(x, r.y), new Vector2(x, r.yMax));
            }

            Handles.EndGUI();
            
            // Vẽ nhãn sau để tránh chồng chéo với các đường kẻ
            for (float t = 0f; t <= total + 0.0001f; t += step)
            {
                float x = Mathf.Lerp(r.x, r.xMax, t / total);
                var label = $"{t:0.00}s";
                var size = GUI.skin.label.CalcSize(new GUIContent(label));
                GUI.Label(new Rect(x + 2, r.y - 1, size.x, r.height), label);
            }
        }

        float NiceStep(float total)
        {
            float[] steps = { 0.05f, 0.1f, 0.2f, 0.5f, 1f, 2f, 5f, 10f, 15f, 30f, 60f };
            foreach (var s in steps)
                if (total / s <= 12f)
                    return s;
            return 120f;
        }

        float TimeToPixel(Rect r, float start, float end, float t)
        {
            float u = Mathf.InverseLerp(start, end, t);
            return Mathf.Lerp(r.x, r.xMax, u);
        }

        float PixelToTime(Rect r, float start, float end, float px)
        {
            float u = Mathf.InverseLerp(r.x, r.xMax, px);
            return Mathf.Lerp(start, end, u);
        }

        Rect TimeRangeToRect(Rect r, float start, float end, float a, float b)
        {
            float ax = TimeToPixel(r, start, end, a);
            float bx = TimeToPixel(r, start, end, b);
            return Rect.MinMaxRect(Mathf.Min(ax, bx), r.y, Mathf.Max(ax, bx), r.yMax);
        }

        void DrawBorder(Rect r, Color c)
        {
            EditorGUI.DrawRect(new Rect(r.x, r.y, r.width, 2), c);
            EditorGUI.DrawRect(new Rect(r.x, r.yMax - 2, r.width, 2), c);
            EditorGUI.DrawRect(new Rect(r.x, r.y, 2, r.height), c);
            EditorGUI.DrawRect(new Rect(r.xMax - 2, r.y, 2, r.height), c);
        }
    }
}