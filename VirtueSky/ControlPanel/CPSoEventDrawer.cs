using UnityEditor;
using UnityEngine;
using VirtueSky.Events;
using VirtueSky.UtilsEditor;

namespace VirtueSky.ControlPanel.Editor
{
    public static class CPSoEventDrawer
    {
        public static void OnDrawSoEvent()
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            GUILayout.Label("SCRIPTABLE OBJECT EVENT", EditorStyles.boldLabel);
            GUILayout.Space(10);
            Uniform.DrawGroupFoldout("cp_draw_button_create_event", "Scriptable Event", () => DrawButtonEvent());
            GUILayout.Space(10);
            Uniform.DrawGroupFoldout("cp_draw_button_create_event_result", "Scriptable Event-Result",
                () => DrawButtonEventResult(), false);
            GUILayout.EndVertical();
        }

        static void DrawButtonEvent()
        {
            if (GUILayout.Button("Create Boolean Event"))
            {
                EventWindowEditor.CreateEventBoolean();
            }

            if (GUILayout.Button("Create Dictionary Event"))
            {
                EventWindowEditor.CreateEventDictionary();
            }

            if (GUILayout.Button("Create No Param Event"))
            {
                EventWindowEditor.CreateEventNoParam();
            }

            if (GUILayout.Button("Create Float Event"))
            {
                EventWindowEditor.CreateEventFloat();
            }

            if (GUILayout.Button("Create Int Event"))
            {
                EventWindowEditor.CreateEventInt();
            }

            if (GUILayout.Button("Create Object Event"))
            {
                EventWindowEditor.CreateEventObject();
            }

            if (GUILayout.Button("Create Short Double Event"))
            {
                EventWindowEditor.CreateEventShortDouble();
            }

            if (GUILayout.Button("Create String Event"))
            {
                EventWindowEditor.CreateEventString();
            }

            if (GUILayout.Button("Create Vector3 Event"))
            {
                EventWindowEditor.CreateEventVector3();
            }
        }

        static void DrawButtonEventResult()
        {
        }
    }
}