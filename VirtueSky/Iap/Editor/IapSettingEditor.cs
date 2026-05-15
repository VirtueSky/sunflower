#if VIRTUESKY_IAP
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using VirtueSky.UtilsEditor;

namespace VirtueSky.Iap
{
    [CustomEditor(typeof(IapSetting), true)]
    public class IapSettingEditor : Editor
    {
        private IapSetting _iapSetting;
        private SerializedProperty _skusData;
        private SerializedProperty _products;
        private SerializedProperty _isValidatePurchase;
        private SerializedProperty _isCustomValidatePurchase;
        private SerializedProperty _validatePurchase;
        private SerializedProperty _googlePlayStoreKey;

        void Initialize()
        {
            _iapSetting = target as IapSetting;
            _skusData = serializedObject.FindProperty("skusData");
            _products = serializedObject.FindProperty("products");
            _isValidatePurchase = serializedObject.FindProperty("isValidatePurchase");
            _isCustomValidatePurchase = serializedObject.FindProperty("isCustomValidatePurchase");
            _validatePurchase = serializedObject.FindProperty("validatePurchase");
            _googlePlayStoreKey = serializedObject.FindProperty("googlePlayStoreKey");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            Initialize();
            EditorGUILayout.PropertyField(_skusData);
            EditorGUILayout.PropertyField(_products);
            if (GUILayout.Button("Generate Product From SkusData"))
            {
                GenerateProduct();
            }
            GUILayout.Space(10);
            GuiLine(2);
            GUILayout.Space(10);
            EditorGUILayout.PropertyField(_isValidatePurchase);
            if (_isValidatePurchase.boolValue)
            {
               
                EditorGUILayout.PropertyField(_googlePlayStoreKey);
                GUILayout.Space(10);
                if (GUILayout.Button("Obfuscator Key"))
                {
                    ObfuscatorKey();
                }
                GUILayout.Space(10);
                EditorGUILayout.PropertyField(_isCustomValidatePurchase);
                if (_isCustomValidatePurchase.boolValue)
                {
                    EditorGUILayout.PropertyField(_validatePurchase);
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        void GenerateProduct()
        {
            _iapSetting.Products.Clear();
            for (int i = 0; i < _iapSetting.SkusData.Count; i++)
            {
                bool isCustomName = false;
                var data = _iapSetting.SkusData[i];
                string itemName = data.Id.Split('.').Last();
                if (!string.IsNullOrEmpty(data.customProductName))
                {
                    isCustomName = true;
                    itemName = data.customProductName;
                }

                var itemDataVariable =
                    CreateAsset.CreateAndGetScriptableAssetByName<IapDataVariable>("/Iap/Products",
                        isCustomName ? $"{itemName.ToLower()}" : $"iap_{itemName.ToLower()}");
                itemDataVariable.androidId = data.androidId;
                itemDataVariable.iosId = data.iosId;
                itemDataVariable.productType = data.productType;
                _iapSetting.Products.Add(itemDataVariable);
            }

            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        void ObfuscatorKey()
        {
            var googleError = "";
            var appleError = "";
            ObfuscationGenerator.ObfuscateSecrets(includeGoogle: true,
                appleError: ref googleError,
                googleError: ref appleError,
                googlePlayPublicKey: _iapSetting.GooglePlayStoreKey);
            string pathAsmdef =
                FileExtension.GetPathFileInCurrentEnvironment(
                    $"VirtueSky/Utils/Editor/TemplateAssembly/PurchasingGeneratedAsmdef.txt");
            string pathAsmdefMeta =
                FileExtension.GetPathFileInCurrentEnvironment(
                    $"VirtueSky/Utils/Editor/TemplateAssembly/PurchasingGeneratedAsmdefMeta.txt");
            var asmdef = (TextAsset)AssetDatabase.LoadAssetAtPath(pathAsmdef, typeof(TextAsset));
            var meta = (TextAsset)AssetDatabase.LoadAssetAtPath(pathAsmdefMeta, typeof(TextAsset));
            string path = Path.Combine(TangleFileConsts.k_OutputPath, "virtuesky.purchasing.generate.asmdef");
            string pathMeta = Path.Combine(TangleFileConsts.k_OutputPath, "virtuesky.purchasing.generate.asmdef.meta");
            if (!File.Exists(path))
            {
                var writer = new StreamWriter(path, false);
                writer.Write(asmdef.text);
                writer.Close();
            }

            if (!File.Exists(pathMeta))
            {
                var writer = new StreamWriter(pathMeta, false);
                writer.Write(meta.text);
                writer.Close();
            }

            AssetDatabase.ImportAsset(path);
        }
        void GuiLine(int i_height = 1)
        {
            Rect rect = EditorGUILayout.GetControlRect(false, i_height);

            rect.height = i_height;

            EditorGUI.DrawRect(rect, new Color32(0, 0, 0, 255));
        }
    }
}
#endif