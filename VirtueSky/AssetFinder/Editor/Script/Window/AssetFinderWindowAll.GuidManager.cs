using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace VirtueSky.AssetFinder.Editor
{
    internal partial class AssetFinderWindowAll
    {
        private Dictionary<string, UnityObject> guidObjs;
        private Vector2 scrollPos;
        private string tempGUID;
        private string tempFileID;
        private UnityObject tempObject;

        private void DrawGUIDs()
        {
            GUILayout.Label("GUID to Object", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
            {
                string guid = EditorGUILayout.TextField(tempGUID ?? string.Empty);
                string fileId = EditorGUILayout.TextField(tempFileID ?? string.Empty);
                EditorGUILayout.ObjectField(tempObject, typeof(UnityObject), false, GUI2.GLW_160);

                if (GUILayout.Button("Paste", EditorStyles.miniButton, GUI2.GLW_70))
                {
                    string[] split = EditorGUIUtility.systemCopyBuffer.Split('/');
                    guid = split[0];
                    fileId = split.Length == 2 ? split[1] : string.Empty;
                }

                if ((guid != tempGUID || fileId != tempFileID) && !string.IsNullOrEmpty(guid))
                {
                    tempGUID = guid;
                    tempFileID = fileId;
                    string fullId = string.IsNullOrEmpty(fileId) ? tempGUID : tempGUID + "/" + tempFileID;

                    tempObject = AssetFinderUnity.LoadAssetAtPath<UnityObject>
                    (
                        AssetDatabase.GUIDToAssetPath(fullId)
                    );
                }

                if (GUILayout.Button("Set FileID"))
                {
                    var newDict = new Dictionary<string, UnityObject>();
                    foreach (KeyValuePair<string, UnityObject> kvp in guidObjs)
                    {
                        string key = kvp.Key.Split('/')[0];
                        if (!string.IsNullOrEmpty(fileId)) key = key + "/" + fileId;

                        var value = AssetFinderUnity.LoadAssetAtPath<UnityObject>
                        (
                            AssetDatabase.GUIDToAssetPath(key)
                        );
                        newDict.Add(key, value);
                    }

                    guidObjs = newDict;
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(10f);
            if (guidObjs == null)
            {
                GUILayout.FlexibleSpace();
                return;
            }

            scrollPos = GUILayout.BeginScrollView(scrollPos);
            {
                foreach (KeyValuePair<string, UnityObject> item in guidObjs)
                {
                    GUILayout.BeginHorizontal();
                    {
                        UnityObject obj = item.Value;

                        EditorGUILayout.ObjectField(obj, typeof(UnityObject), false, GUI2.GLW_150);
                        string idi = item.Key;
                        GUILayout.TextField(idi, GUI2.GLW_320);
                        if (GUILayout.Button(AssetFinderGUIContent.FromString("Copy"), EditorStyles.miniButton, GUI2.GLW_50))
                        {
                            tempObject = obj;

                            string[] arr = item.Key.Split('/');
                            tempGUID = arr[0];
                            tempFileID = arr[1];
                        }

                    }
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndScrollView();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Merge Selection To"))
            {
                string fullId = string.IsNullOrEmpty(tempFileID) ? tempGUID : tempGUID + "/" + tempFileID;
                AssetFinderExport.MergeDuplicate(fullId);
            }

            EditorGUILayout.ObjectField(tempObject, typeof(UnityObject), false, GUI2.GLW_120);
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
        }
    }
} 