using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VirtueSky.Core;
using VirtueSky.UtilsEditor;

namespace VirtueSky.Variables
{
    internal class VariableGenerateGuid : AssetPostprocessor
    {
        private static readonly HashSet<string> GuidsVariableCache = new HashSet<string>();
        private const string Key_Init_Session = "Key_Init_Session";

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            bool isInit = SessionState.GetBool(Key_Init_Session, false);
            if (!isInit)
            {
                CacheAllGuids();
                SessionState.SetBool(Key_Init_Session, true);
            }
            else
            {
                OnImportAsset(importedAssets);
                OnAssetDeleted(deletedAssets);
                OnAssetMoved(movedFromAssetPaths, movedAssets);
            }
        }

        static void CacheAllGuids()
        {
            var baseVariables = FileExtension.FindAll<BaseSO>();
            foreach (var variable in baseVariables)
            {
                if (variable is IGuidVariable iGuidVariable)
                {
                    iGuidVariable.Guid = GenerateGuid(variable);
                    GuidsVariableCache.Add(iGuidVariable.Guid);
                }
            }
        }

        static void OnImportAsset(string[] importedAssets)
        {
            foreach (var path in importedAssets)
            {
                if (GuidsVariableCache.Contains(path)) continue;
                var asset = AssetDatabase.LoadAssetAtPath<BaseSO>(path);
                if (asset == null || asset is not IGuidVariable iGuidVariable) continue;
                iGuidVariable.Guid = GenerateGuid(asset);
                GuidsVariableCache.Add(iGuidVariable.Guid);
            }
        }

        static void OnAssetDeleted(string[] deletedAssets)
        {
            foreach (var path in deletedAssets)
            {
                if (!GuidsVariableCache.Contains(path)) continue;
                GuidsVariableCache.Remove(path);
            }
        }

        static void OnAssetMoved(string[] movedFromAssetPaths, string[] movedAssets)
        {
            OnAssetDeleted(movedFromAssetPaths);
            OnImportAsset(movedAssets);
        }

        private static string GenerateGuid(ScriptableObject scriptableObject)
        {
            return AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(scriptableObject));
        }
    }
}