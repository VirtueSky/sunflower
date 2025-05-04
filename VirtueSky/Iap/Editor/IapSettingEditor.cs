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
        private SerializedProperty _googlePlayStoreKey;

        void Initialize()
        {
            _iapSetting = target as IapSetting;
            _skusData = serializedObject.FindProperty("skusData");
            _products = serializedObject.FindProperty("products");
            _isValidatePurchase = serializedObject.FindProperty("isValidatePurchase");
            _isCustomValidatePurchase = serializedObject.FindProperty("isCustomValidatePurchase");
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

            EditorGUILayout.PropertyField(_isValidatePurchase);
            if (_isValidatePurchase.boolValue)
            {
                EditorGUILayout.PropertyField(_isCustomValidatePurchase);
                EditorGUILayout.PropertyField(_googlePlayStoreKey);
                if (GUILayout.Button("Obfuscator Key"))
                {
                    ObfuscatorKey();
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        void GenerateProduct()
        {
            _iapSetting.Products.Clear();
            for (int i = 0; i < _iapSetting.SkusData.Count; i++)
            {
                string itemName = _iapSetting.SkusData[i].id.Split('.').Last();
                var itemDataVariable =
                    CreateAsset.CreateAndGetScriptableAssetByName<IapDataVariable>("/Iap/Products",
                        $"iap_{itemName.ToLower()}");
                itemDataVariable.id = _iapSetting.SkusData[i].id;
                itemDataVariable.productType = _iapSetting.SkusData[i].productType;
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
    }
}
#endif