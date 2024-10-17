using System;
using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Localization
{
    [EditorIcon("icon_csharp")]
    public class LocaleTextAssetComponent : LocaleComponentGeneric<LocaleTextAsset, TextAsset>
    {
        protected override Type GetValueType() => typeof(string);

        protected override object GetLocaleValue() => Variable ? Variable.Value.text : null;
    }
}