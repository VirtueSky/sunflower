﻿#if VIRTUESKY_IAP
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using VirtueSky.Inspector;

namespace VirtueSky.Iap
{
    [EditorIcon("icon_scriptable")]
    public class IapSetting : ScriptableObject
    {
        [SerializeField] private List<IapData> skusData = new List<IapData>();
        [SerializeField] private List<IapDataVariable> products = new List<IapDataVariable>();

        [Space, SerializeField] private bool isValidatePurchase;
        [SerializeField] private bool isCustomValidatePurchase;
#if UNITY_EDITOR
        //[ShowIf(nameof(isValidatePurchase), true)] 
        [SerializeField, TextArea] private string googlePlayStoreKey;
        public string GooglePlayStoreKey => googlePlayStoreKey;
#endif
        public List<IapData> SkusData => skusData;
        public List<IapDataVariable> Products => products;
        public bool IsValidatePurchase => isValidatePurchase;
        public bool IsCustomValidatePurchase => isCustomValidatePurchase;
    }

    [Serializable]
    public class IapData
    {
        public string id;
        public ProductType productType;
    }
}
#endif