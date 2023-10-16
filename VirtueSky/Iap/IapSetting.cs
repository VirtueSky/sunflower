using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Purchasing;

#if UNITY_EDITOR
using UnityEditor;
#endif
using VirtueSky.Utils;


namespace VirtueSky.Iap
{
    [HideMonoScript]
    public class IapSetting : ScriptableObject
    {
        [SerializeField] private List<IapData> skusData = new List<IapData>();
        [ReadOnly] [SerializeField] private List<IapDataVariable> products = new List<IapDataVariable>();

        public List<IapDataVariable> Products => products;
#if UNITY_EDITOR
        private string path = CreateAsset.DefaultResourcesPath("/Iap/Products");
        [Button("Generate Product From Sku")]
        public void GenerateProduct()
        {
            products.Clear();
            for (int i = 0; i < skusData.Count; i++)
            {
                string itemName = skusData[i].id.Split('.').Last();
                Debug.Log(itemName);
                AssetDatabase.DeleteAsset($"{path}/iap_{itemName.ToLower()}.asset");
                var itemDataVariable = CreateInstance<IapDataVariable>();
                itemDataVariable.id = skusData[i].id;
                itemDataVariable.productType = skusData[i].productType;
                products.Add(itemDataVariable);
                AssetDatabase.CreateAsset(itemDataVariable, $"{path}/iap_{itemName.ToLower()}.asset");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
#endif
    }

    [Serializable]
    public class IapData
    {
        public string id;
        public ProductType productType;
    }
}