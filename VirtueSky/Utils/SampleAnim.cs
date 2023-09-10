using UnityEditor;
using UnityEngine;
using VirtueSky.Core;

namespace VirtueSky.Utils
{
    public class SampleAnim : BaseMono
    {
        [SerializeField] public AnimationClip clip;

        public override void Initialize()
        {
            Destroy(this);
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(SampleAnim))]
    public class SampleAnimEditor : Editor
    {
        SampleAnim sampleAnim;
        int frame;

        void OnEnable()
        {
            sampleAnim = target as SampleAnim;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (sampleAnim.clip != null)
            {
                var pos = sampleAnim.transform.position;
                var rot = sampleAnim.transform.rotation;
                var scale = sampleAnim.transform.localScale;

                var frames = (int)(sampleAnim.clip.length * sampleAnim.clip.frameRate);
                frame = EditorGUILayout.IntSlider("Frame", frame, 0, frames);
                var t = frame / sampleAnim.clip.frameRate;
                sampleAnim.clip.SampleAnimation(sampleAnim.gameObject, t);

                sampleAnim.transform.position = pos;
                sampleAnim.transform.rotation = rot;
                sampleAnim.transform.localScale = scale;
            }
        }
    }
#endif
}