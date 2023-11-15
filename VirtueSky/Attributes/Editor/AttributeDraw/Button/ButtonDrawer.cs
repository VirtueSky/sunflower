using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Collections.Generic;

namespace VirtueSky.Attributes
{
    public class ButtonDrawer
    {
        private static bool s_ReverseAttributesOrder;

        [Button, Button.BeginHorizontal, InitializeOnLoadMethod]
        public static void CheckAttributesOrder()
        {
            var method = typeof(ButtonDrawer).GetMethod("CheckAttributesOrder");
            var attributes = method.GetCustomAttributes(false);
            s_ReverseAttributesOrder = attributes[0].GetType() != typeof(ButtonAttribute);
        }

        private UIAction[] m_Actions;

        public ButtonDrawer(object target)
        {
            BindingFlags flags =
                BindingFlags.InvokeMethod |
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.Static |
                BindingFlags.Instance;

            var type = target.GetType();
            var methods = type.GetMethods(flags);

            Stack<ScopeType> scopeTypeStack = new Stack<ScopeType>();
            List<UIAction> actionList = new List<UIAction>();

            for (int i = 0; i < methods.Length; i++)
            {
                MethodInfo methodInfo = methods[i];
                var attributes = methodInfo.GetCustomAttributes(false);
                int attributeCount = attributes.Length;

                for (int j = 0; j < attributeCount; j++)
                {
                    var attribute = attributes[s_ReverseAttributesOrder ? attributeCount - j - 1 : j];
                    var attributeType = attribute.GetType();

                    if (attributeType == typeof(ButtonAttribute))
                    {
                        var eButtonAttribute = (ButtonAttribute)attribute;
                        string text = (eButtonAttribute.text == null) ? methodInfo.Name : eButtonAttribute.text;

                        actionList.Add(new DrawButton(text, methodInfo, target));
                    }
                    else if (attributeType == typeof(Button.BeginHorizontalAttribute))
                    {
                        var scopeAttribute = (Button.BeginHorizontalAttribute)attribute;
                        actionList.Add(new BeginHorizontalScope(scopeAttribute.text));
                        scopeTypeStack.Push(ScopeType.Horizontal);
                    }
                    else if (attributeType == typeof(Button.EndHorizontalAttribute))
                    {
                        actionList.Add(new EndHorizontalScope());
                        scopeTypeStack.Pop();
                    }
                    else if (attributeType == typeof(Button.BeginVerticalAttribute))
                    {
                        var scopeAttribute = (Button.BeginVerticalAttribute)attribute;
                        actionList.Add(new BeginVerticalScope(scopeAttribute.text));
                        scopeTypeStack.Push(ScopeType.Vertical);
                    }
                    else if (attributeType == typeof(Button.EndVerticalAttribute))
                    {
                        actionList.Add(new EndVerticalScope());
                        scopeTypeStack.Pop();
                    }
                }
            }

            foreach (var scopeType in scopeTypeStack)
            {
                if (scopeType == ScopeType.Horizontal)
                {
                    actionList.Add(new EndHorizontalScope());
                }
                else
                {
                    actionList.Add(new EndVerticalScope());
                }
            }

            m_Actions = actionList.ToArray();
        }

        public void Draw()
        {
            foreach (var action in m_Actions)
            {
                action.Execute();
            }
        }


        enum ScopeType
        {
            Horizontal,
            Vertical
        }

        abstract class UIAction
        {
            public abstract void Execute();
        }

        class DrawButton : UIAction
        {
            private GUIContent m_GUIContent;
            private MethodInfo m_MethodInfo;
            private object m_Target;

            public DrawButton(string text, MethodInfo methodInfo, object target)
            {
                m_GUIContent = new GUIContent(text);
                m_MethodInfo = methodInfo;
                m_Target = target;
            }

            public override void Execute()
            {
                if (GUILayout.Button(m_GUIContent))
                {
                    m_MethodInfo.Invoke(m_Target, null);
                }
            }
        }

        class BeginHorizontalScope : UIAction
        {
            private GUIContent m_GUIContent;

            public BeginHorizontalScope(string text)
            {
                if (text != null) m_GUIContent = new GUIContent(text);
            }

            public override void Execute()
            {
                if (m_GUIContent == null)
                {
                    GUILayout.BeginHorizontal();
                }
                else
                {
                    GUILayout.BeginHorizontal(m_GUIContent, GUI.skin.window, GUILayout.MaxHeight(1));
                }
            }
        }

        class EndHorizontalScope : UIAction
        {
            public override void Execute()
            {
                GUILayout.EndHorizontal();
            }
        }

        class BeginVerticalScope : UIAction
        {
            private GUIContent m_GUIContent;

            public BeginVerticalScope(string text)
            {
                if (text != null) m_GUIContent = new GUIContent(text);
            }

            public override void Execute()
            {
                if (m_GUIContent == null)
                {
                    GUILayout.BeginVertical();
                }
                else
                {
                    GUILayout.BeginVertical(m_GUIContent, GUI.skin.window, GUILayout.MaxHeight(1));
                }
            }
        }

        class EndVerticalScope : UIAction
        {
            public override void Execute()
            {
                GUILayout.EndHorizontal();
            }
        }
    }
}