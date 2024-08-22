using UnityEditor;
using UnityEngine;
using VirtueSky.Events;
using VirtueSky.UtilsEditor;

namespace VirtueSky.ControlPanel.Editor
{
    public static class CPSoEventDrawer
    {
        static Vector2 scroll = Vector2.zero;
        private static bool isShowFielEvent = true;
        private static bool isShowFielEventResult;

        public static void OnDrawSoEvent()
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            CPUtility.DrawHeaderIcon(StatePanelControl.SO_Event, "Scriptable Events");
            GUILayout.Space(10);
            scroll = GUILayout.BeginScrollView(scroll);
            CPUtility.DrawToggle(ref isShowFielEvent, "Scriptable Event", DrawButtonEvent);
            GUILayout.Space(10);
            CPUtility.DrawToggle(ref isShowFielEventResult, "Scriptable Event-Result", DrawButtonEventResult);
            GUILayout.EndScrollView();
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

            if (GUILayout.Button("Create GameObject Event"))
            {
                EventWindowEditor.CreateEventGameObject();
            }

            if (GUILayout.Button("Create Transform Event"))
            {
                EventWindowEditor.CreateEventTransform();
            }
        }

        static void DrawButtonEventResult()
        {
            GUILayout.Space(10);
            Uniform.DrawGroupFoldout("cp_draw_bool_event_result", "Bool Event-Result", () =>
            {
                if (GUILayout.Button("Bool Event - Bool Result"))
                {
                    EventWindowEditor.CreateBoolEventBoolResult();
                }

                if (GUILayout.Button("Bool Event - Float Result"))
                {
                    EventWindowEditor.CreateBoolEventFloatResult();
                }

                if (GUILayout.Button("Bool Event - Int Result"))
                {
                    EventWindowEditor.CreateBoolEventIntResult();
                }

                if (GUILayout.Button("Bool Event - Object Result"))
                {
                    EventWindowEditor.CreateBoolEventObjectResult();
                }

                if (GUILayout.Button("Bool Event - String Result"))
                {
                    EventWindowEditor.CreateBoolEventStringResult();
                }

                if (GUILayout.Button("Bool Event - Vector3 Result"))
                {
                    EventWindowEditor.CreateBoolEventVector3Result();
                }

                if (GUILayout.Button("Bool Event - GameObject Result"))
                {
                    EventWindowEditor.CreateBoolEventGameObjectResult();
                }

                if (GUILayout.Button("Bool Event - Transform Result"))
                {
                    EventWindowEditor.CreateBoolEventTransformResult();
                }
            }, false);
            GUILayout.Space(10);
            Uniform.DrawGroupFoldout("cp_draw_float_event_result", "Float Event-Result", () =>
            {
                if (GUILayout.Button("Float Event - Bool Result"))
                {
                    EventWindowEditor.CreateFloatEventBoolResult();
                }

                if (GUILayout.Button("Float Event - Float Result"))
                {
                    EventWindowEditor.CreateFloatEventFloatResult();
                }

                if (GUILayout.Button("Float Event - Int Result"))
                {
                    EventWindowEditor.CreateFloatEventIntResult();
                }

                if (GUILayout.Button("Float Event - Object Result"))
                {
                    EventWindowEditor.CreateFloatEventObjectResult();
                }

                if (GUILayout.Button("Float Event - String Result"))
                {
                    EventWindowEditor.CreateFloatEventStringResult();
                }

                if (GUILayout.Button("Float Event - Vector3 Result"))
                {
                    EventWindowEditor.CreateFloatEventVector3Result();
                }

                if (GUILayout.Button("Float Event - GameObject Result"))
                {
                    EventWindowEditor.CreateFloatEventGameObjectResult();
                }

                if (GUILayout.Button("Float Event - Transform Result"))
                {
                    EventWindowEditor.CreateFloatEventTransformResult();
                }
            }, false);
            GUILayout.Space(10);
            Uniform.DrawGroupFoldout("cp_draw_int_event_result", "Int Event-Result", () =>
            {
                if (GUILayout.Button("Int Event - Bool Result"))
                {
                    EventWindowEditor.CreateIntEventBoolResult();
                }

                if (GUILayout.Button("Int Event - Float Result"))
                {
                    EventWindowEditor.CreateIntEventFloatResult();
                }

                if (GUILayout.Button("Int Event - Int Result"))
                {
                    EventWindowEditor.CreateIntEventIntResult();
                }

                if (GUILayout.Button("Int Event - Object Result"))
                {
                    EventWindowEditor.CreateIntEventObjectResult();
                }

                if (GUILayout.Button("Int Event - String Result"))
                {
                    EventWindowEditor.CreateIntEventStringResult();
                }

                if (GUILayout.Button("Int Event - Vector3 Result"))
                {
                    EventWindowEditor.CreateIntEventVector3Result();
                }

                if (GUILayout.Button("Int Event - GameObject Result"))
                {
                    EventWindowEditor.CreateIntEventGameObjectResult();
                }

                if (GUILayout.Button("Int Event - Transform Result"))
                {
                    EventWindowEditor.CreateIntEventTransformResult();
                }
            }, false);
            GUILayout.Space(10);
            Uniform.DrawGroupFoldout("cp_draw_object_event_result", "Object Event-Result", () =>
            {
                if (GUILayout.Button("Object Event - Bool Result"))
                {
                    EventWindowEditor.CreateObjectEventBoolResult();
                }

                if (GUILayout.Button("Object Event - Float Result"))
                {
                    EventWindowEditor.CreateObjectEventFloatResult();
                }

                if (GUILayout.Button("Object Event - Int Result"))
                {
                    EventWindowEditor.CreateObjectEventIntResult();
                }

                if (GUILayout.Button("Object Event - Object Result"))
                {
                    EventWindowEditor.CreateObjectEventObjectResult();
                }

                if (GUILayout.Button("Object Event - String Result"))
                {
                    EventWindowEditor.CreateObjectEventStringResult();
                }

                if (GUILayout.Button("Object Event - Vector3 Result"))
                {
                    EventWindowEditor.CreateObjectEventVector3Result();
                }

                if (GUILayout.Button("Object Event - GameObject Result"))
                {
                    EventWindowEditor.CreateObjectEventGameObjectResult();
                }

                if (GUILayout.Button("Object Event - Transform Result"))
                {
                    EventWindowEditor.CreateObjectEventTransformResult();
                }
            }, false);
            GUILayout.Space(10);
            Uniform.DrawGroupFoldout("cp_draw_string_event_result", "String Event-Result", () =>
            {
                if (GUILayout.Button("String Event - Bool Result"))
                {
                    EventWindowEditor.CreateStringEventBoolResult();
                }

                if (GUILayout.Button("String Event - Float Result"))
                {
                    EventWindowEditor.CreateStringEventFloatResult();
                }

                if (GUILayout.Button("String Event - Int Result"))
                {
                    EventWindowEditor.CreateStringEventIntResult();
                }

                if (GUILayout.Button("String Event - Object Result"))
                {
                    EventWindowEditor.CreateStringEventObjectResult();
                }

                if (GUILayout.Button("String Event - String Result"))
                {
                    EventWindowEditor.CreateStringEventStringResult();
                }

                if (GUILayout.Button("String Event - Vector3 Result"))
                {
                    EventWindowEditor.CreateStringEventVector3Result();
                }

                if (GUILayout.Button("String Event - GameObject Result"))
                {
                    EventWindowEditor.CreateStringEventGameObjectResult();
                }

                if (GUILayout.Button("String Event - Transform Result"))
                {
                    EventWindowEditor.CreateStringEventTransformResult();
                }
            }, false);
            GUILayout.Space(10);
            Uniform.DrawGroupFoldout("cp_draw_vector3_event_result", "Vector3 Event-Result", () =>
            {
                if (GUILayout.Button("Vector3 Event - Bool Result"))
                {
                    EventWindowEditor.CreateVector3EventBoolResult();
                }

                if (GUILayout.Button("Vector3 Event - Float Result"))
                {
                    EventWindowEditor.CreateVector3EventFloatResult();
                }

                if (GUILayout.Button("Vector3 Event - Int Result"))
                {
                    EventWindowEditor.CreateVector3EventIntResult();
                }

                if (GUILayout.Button("Vector3 Event - Object Result"))
                {
                    EventWindowEditor.CreateVector3EventObjectResult();
                }

                if (GUILayout.Button("Vector3 Event - String Result"))
                {
                    EventWindowEditor.CreateVector3EventStringResult();
                }

                if (GUILayout.Button("Vector3 Event - Vector3 Result"))
                {
                    EventWindowEditor.CreateVector3EventVector3Result();
                }

                if (GUILayout.Button("Vector3 Event - GameObject Result"))
                {
                    EventWindowEditor.CreateVector3EventGameObjectResult();
                }

                if (GUILayout.Button("Vector3 Event - Transform Result"))
                {
                    EventWindowEditor.CreateVector3EventTransformResult();
                }
            }, false);
            GUILayout.Space(10);
            Uniform.DrawGroupFoldout("cp_draw_event_no_param_result", "Event No Param-Result", () =>
            {
                if (GUILayout.Button("Event No Param - Bool Result"))
                {
                    EventWindowEditor.CreateEventNoParamBoolResult();
                }

                if (GUILayout.Button("Event No Param - Float Result"))
                {
                    EventWindowEditor.CreateEventNoParamFloatResult();
                }

                if (GUILayout.Button("Event No Param - Int Result"))
                {
                    EventWindowEditor.CreateEventNoParamIntResult();
                }

                if (GUILayout.Button("Event No Param - Object Result"))
                {
                    EventWindowEditor.CreateEventNoParamObjectResult();
                }

                if (GUILayout.Button("Event No Param - String Result"))
                {
                    EventWindowEditor.CreateEventNoParamStringResult();
                }

                if (GUILayout.Button("Event No Param - Vector3 Result"))
                {
                    EventWindowEditor.CreateEventNoParamVector3Result();
                }

                if (GUILayout.Button("Event No Param - GameObject Result"))
                {
                    EventWindowEditor.CreateEventNoParamGameObjectResult();
                }

                if (GUILayout.Button("Event No Param - Transform Result"))
                {
                    EventWindowEditor.CreateEventNoParamTransformResult();
                }
            }, false);
            GUILayout.Space(10);
            Uniform.DrawGroupFoldout("cp_draw_gameobject_event_result", "GameObject Event-Result", () =>
            {
                if (GUILayout.Button("GameObject Event - Bool Result"))
                {
                    EventWindowEditor.CreateGameObjectEventBoolResult();
                }

                if (GUILayout.Button("GameObject Event - Float Result"))
                {
                    EventWindowEditor.CreateGameObjectEventFloatResult();
                }

                if (GUILayout.Button("GameObject Event - GameObject Result"))
                {
                    EventWindowEditor.CreateGameObjectEventGameObjectResult();
                }

                if (GUILayout.Button("GameObject Event - Int Result"))
                {
                    EventWindowEditor.CreateGameObjectEventIntResult();
                }

                if (GUILayout.Button("GameObject Event - Object Result"))
                {
                    EventWindowEditor.CreateGameObjectEventObjectResult();
                }

                if (GUILayout.Button("GameObject Event - String Result"))
                {
                    EventWindowEditor.CreateGameObjectEventStringResult();
                }

                if (GUILayout.Button("GameObject Event - Transform Result"))
                {
                    EventWindowEditor.CreateGameObjectEventTransformResult();
                }

                if (GUILayout.Button("GameObject Event - Vector3 Result"))
                {
                    EventWindowEditor.CreateGameObjectEventVector3Result();
                }
            }, false);
            GUILayout.Space(10);
            Uniform.DrawGroupFoldout("cp_draw_transform_event_result", "Transform Event-Result", () =>
            {
                if (GUILayout.Button("Transform Event - Bool Result"))
                {
                    EventWindowEditor.CreateTransformEventBoolResult();
                }

                if (GUILayout.Button("Transform Event - Float Result"))
                {
                    EventWindowEditor.CreateTransformEventFloatResult();
                }

                if (GUILayout.Button("Transform Event - GameObject Result"))
                {
                    EventWindowEditor.CreateTransformEventGameObjectResult();
                }

                if (GUILayout.Button("Transform Event - Int Result"))
                {
                    EventWindowEditor.CreateTransformEventIntResult();
                }

                if (GUILayout.Button("Transform Event - Object Result"))
                {
                    EventWindowEditor.CreateTransformEventObjectResult();
                }

                if (GUILayout.Button("Transform Event - String Result"))
                {
                    EventWindowEditor.CreateTransformEventStringResult();
                }

                if (GUILayout.Button("Transform Event - Transform Result"))
                {
                    EventWindowEditor.CreateTransformEventTransformResult();
                }

                if (GUILayout.Button("Transform Event - Vector3 Result"))
                {
                    EventWindowEditor.CreateTransformEventVector3Result();
                }
            }, false);
        }
    }
}