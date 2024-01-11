using UnityEditor;
using UnityEngine;
using VirtueSky.Variables;

namespace VirtueSky.ControlPanel.Editor
{
    public static class CPSoVariableDrawer
    {
        public static void OnDrawSoVariable()
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            GUILayout.Label("SCRIPTABLE OBJECT VARIABLE", EditorStyles.boldLabel);
            GUILayout.Space(10);
            if (GUILayout.Button("Create Boolean Variable"))
            {
                VariableWindowEditor.CreateVariableBoolean();
            }

            if (GUILayout.Button("Create Float Variable"))
            {
                VariableWindowEditor.CreateVariableFloat();
            }

            if (GUILayout.Button("Create Int Variable"))
            {
                VariableWindowEditor.CreateVariableInt();
            }

            if (GUILayout.Button("Create Object Variable"))
            {
                VariableWindowEditor.CreateVariableObject();
            }

            if (GUILayout.Button("Create Rect Variable"))
            {
                VariableWindowEditor.CreateVariableRect();
            }

            if (GUILayout.Button("Create Short Double Variable"))
            {
                VariableWindowEditor.CreateVariableShortDouble();
            }

            if (GUILayout.Button("Create String Variable"))
            {
                VariableWindowEditor.CreateVariableString();
            }

            if (GUILayout.Button("Create Transform Variable"))
            {
                VariableWindowEditor.CreateVariableTransform();
            }

            if (GUILayout.Button("Create Vector3 Variable"))
            {
                VariableWindowEditor.CreateVariableVector3();
            }

            GUILayout.EndVertical();
        }
    }
}