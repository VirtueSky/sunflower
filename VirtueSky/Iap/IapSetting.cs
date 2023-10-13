using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VirtueSky.Iap
{
    [HideMonoScript]
    public class IapSetting : ScriptableObject
    {
        [SerializeField] private List<IapData> skusData = new List<IapData>();
        [ReadOnly] [SerializeField] private List<IapDataVariable> products = new List<IapDataVariable>();
#if UNITY_EDITOR
        [SerializeField, TextArea] private string googlePlayStoreKey;
#endif

        public List<IapDataVariable> Products => products;
    }

    [Serializable]
    public class IapData
    {
        public string id;
        public ProductType productType;
    }

    public enum ProductType
    {
        Consumable,
        NonConsumable,
        Subscription
    }
}