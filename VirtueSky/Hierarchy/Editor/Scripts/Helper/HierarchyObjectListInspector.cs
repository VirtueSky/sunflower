using UnityEngine;
using UnityEditor;
using VirtueSky.Hierarchy.Data;

namespace VirtueSky.Hierarchy.Helper
{
    [CustomEditor(typeof(ObjectList))]
    public class HierarchyObjectListInspector : Editor
    {
    	public override void OnInspectorGUI()
    	{
    		EditorGUILayout.HelpBox("\nThis is an auto created GameObject that managed by QHierarchy.\n\n" + 
                                    "It stores references to some GameObjects in the current scene. This object will not be included in the application build.\n\n" + 
                                    "You can safely remove it, but lock / unlock / visible / etc. states will be reset. Delete this object if you want to remove the QHierarchy.\n\n" +
                                    "This object can be hidden if you uncheck \"Show QHierarchy GameObject\" in the settings of the QHierarchy.\n"
                                    , MessageType.Info, true);

            if (HierarchySettings.getInstance().get<bool>(HierarchySetting.AdditionalShowObjectListContent))
            {
                if (GUI.Button(EditorGUILayout.GetControlRect(GUILayout.ExpandWidth(true), GUILayout.Height(20)), "Hide content"))
                {
                    HierarchySettings.getInstance().set(HierarchySetting.AdditionalShowObjectListContent, false);
                }
                base.OnInspectorGUI();
            }
            else
            {
                if (GUI.Button(EditorGUILayout.GetControlRect(GUILayout.ExpandWidth(true), GUILayout.Height(20)), "Show content"))
                {
                    HierarchySettings.getInstance().set(HierarchySetting.AdditionalShowObjectListContent, true);
                }
            }
    	}
    }
}