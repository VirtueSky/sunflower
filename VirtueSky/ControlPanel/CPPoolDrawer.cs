using UnityEditor;
using UnityEngine;
using VirtueSky.ObjectPooling;

namespace VirtueSky.ControlPanel.Editor
{
    public static class CPPoolDrawer
    {
        public static void OnDrawPools()
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            GUILayout.Label("POOLS", EditorStyles.boldLabel);
            GUILayout.Space(10);
            if (GUILayout.Button("Create Pools"))
            {
                PoolWindowEditor.CreatePools();
            }

            GUILayout.EndVertical();
        }
    }
}