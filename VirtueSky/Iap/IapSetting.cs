using System;
using System.Collections.Generic;
using UnityEngine;
using VirtueSky.Core;

namespace VirtueSky.Iap
{
    public class IapSetting : ScriptableObject
    {
        [SerializeField] private List<IapData> skusData = new List<IapData>();
        [SerializeField] private List<IapDataVariable> products = new List<IapDataVariable>();
#if UNITY_EDITOR
        [SerializeField, TextArea] private string googlePlayStoreKey;
#endif
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