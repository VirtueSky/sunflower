using System;
using VirtueSky.Inspector;

namespace VirtueSky.Localization
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "Pancake/Localization/Material", fileName = "material_localizevalue", order = 1)]
    [EditorIcon("scriptable_yellow_material")]
    public class LocaleMaterial : LocaleVariable<Material>
    {
        [Serializable]
        private class MaterialLocaleItem : LocaleItem<Material>
        {
        };

        [SerializeField] private MaterialLocaleItem[] items = new MaterialLocaleItem[1];

        // ReSharper disable once CoVariantArrayConversion
        public override LocaleItemBase[] LocaleItems => items;
    }
}