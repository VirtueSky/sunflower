using UnityEditor;
using UnityEngine;
using VirtueSky.AssetFinder.Editor;

[CustomEditor(typeof(AssetFinderCache))]
internal class AssetFinderCacheEditor : Editor
{
    private static string inspectGUID;
    private static int index;

    public override void OnInspectorGUI()
    {
        var c = (AssetFinderCache)target;

        GUILayout.Label("Total : " + c.AssetList.Count);
        AssetFinderCache.DrawPriorityGUI();

        Object s = Selection.activeObject;
        if (s == null)
        {
            return;
        }

        string guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(s));

        if (inspectGUID != guid)
        {
            inspectGUID = guid;
            index = c.AssetList.FindIndex(item => item.guid == guid);
        }

        if (index != -1)
        {
            if (index >= c.AssetList.Count)
            {
                index = 0;
            }

            serializedObject.Update();
            SerializedProperty prop = serializedObject.FindProperty("AssetList")
                .GetArrayElementAtIndex(index);
            EditorGUILayout.PropertyField(prop, true);
        }
    }
}