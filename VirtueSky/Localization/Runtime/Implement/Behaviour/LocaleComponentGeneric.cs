using System;
using UnityEngine;

namespace VirtueSky.Localization
{
    public class LocaleComponentGeneric<TVariable, T> : LocaleComponentGenericBase where TVariable : LocaleVariable<T> where T : class
    {
        [SerializeField] private TVariable variable;

        public TVariable Variable
        {
            get => variable;
            set
            {
                variable = value;
                ForceUpdate();
            }
        }

        protected override Type GetValueType() => typeof(T);

        protected override bool HasLocaleValue() => Variable != null;
        protected override object GetLocaleValue() => GetValueOrDefault(variable);
    }
}