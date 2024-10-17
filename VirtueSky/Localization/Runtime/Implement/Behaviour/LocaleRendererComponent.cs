using UnityEngine;
using VirtueSky.Inspector;
using VirtueSky.Misc;

namespace VirtueSky.Localization
{
    [EditorIcon("icon_csharp")]
    public class LocaleRendererComponent : LocaleComponent
    {
        public int materialIndex;
        public string propertyName = "_MainTex";
        public LocaleTexture localeTexture;

        protected override bool TryUpdateComponentLocalization(bool isOnValidate)
        {
            if (localeTexture)
            {
                var materials = GetMaterials();
                if (materialIndex < materials.Length)
                {
                    materials[materialIndex].SetTexture(propertyName, GetValueOrDefault(localeTexture));
                    return true;
                }

                Debug.LogWarning("Index out of range : " + materialIndex.ToString());
            }


            return false;
        }

        private void OnValidate()
        {
            materialIndex = materialIndex.Max(0);
        }

        private Material[] GetMaterials()
        {
#if UNITY_EDITOR
            if (Application.isPlaying) return GetComponent<Renderer>().sharedMaterials;
#endif
            return GetComponent<Renderer>().materials;
        }
    }
}