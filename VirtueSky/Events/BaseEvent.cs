using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VirtueSky.Core;

namespace VirtueSky.Events
{
    public class BaseEvent : BaseSO, IEvent
    {
        readonly List<IEventListener> listeners = new List<IEventListener>();
        private Action onRaised = null;

        public void Raise()
        {
#if UNITY_EDITOR
            // Debug.Log($"===> {name}");
#endif
            for (var i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised(this);
            }

            onRaised?.Invoke();
        }

        public event Action OnRaised
        {
            add => onRaised += value;
            remove => onRaised -= value;
        }

        public void AddListener(Action action)
        {
            onRaised += action;
        }

        public void RemoveListener(Action action)
        {
            onRaised -= action;
        }

        public void AddListener(IEventListener listener)
        {
            if (!listeners.Contains(listener))
            {
                listeners.Add(listener);
            }
        }

        public void RemoveListener(IEventListener listener)
        {
            if (listeners.Contains(listener))
            {
                listeners.Remove(listener);
            }
        }

        public void RemoveAll()
        {
            listeners.Clear();
            onRaised = null;
        }
    }

    public class BaseEvent<TType> : BaseSO, IEvent<TType>
    {
        readonly List<IEventListener<TType>> listeners = new List<IEventListener<TType>>();
        private Action<TType> onRaised = null;

        public virtual void Raise(TType value)
        {
#if UNITY_EDITOR
            //Debug.Log($"===> {name}");
#endif
            for (var i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised(this, value);
            }

            onRaised?.Invoke(value);
        }

        public event Action<TType> OnRaised
        {
            add => onRaised += value;
            remove => onRaised -= value;
        }

        public void AddListener(Action<TType> action)
        {
            onRaised += action;
        }

        public void RemoveListener(Action<TType> action)
        {
            onRaised -= action;
        }

        public void AddListener(IEventListener<TType> listener)
        {
            if (!listeners.Contains(listener))
            {
                listeners.Add(listener);
            }
        }

        public void RemoveListener(IEventListener<TType> listener)
        {
            if (listeners.Contains(listener))
            {
                listeners.Remove(listener);
            }
        }

        public void RemoveAll()
        {
            listeners.Clear();
            onRaised = null;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(BaseEvent), true)]
    public class BaseEventEditor : UnityEditor.Editor
    {
        BaseEvent baseEvent;

        void OnEnable()
        {
            baseEvent = target as BaseEvent;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Raise"))
            {
                baseEvent.Raise();
            }
        }
    }
    //
    // [CustomEditor(typeof(BaseEvent<>), true)]
    // public class TypedBaseEventEditor : Editor
    // {
    //     public override void OnInspectorGUI()
    //     {
    //         base.OnInspectorGUI();
    //
    //         var debugProperty = serializedObject.FindProperty("debugValue");
    //
    //         if (debugProperty != null)
    //         {
    //             using (new EditorGUIUtils.VerticalHelpBox())
    //             {
    //                 using (var scope = new EditorGUI.ChangeCheckScope())
    //                 {
    //                     EditorGUILayout.PropertyField(debugProperty, GUIContent.none);
    //
    //                     if (scope.changed)
    //                     {
    //                         serializedObject.ApplyModifiedProperties();
    //                     }
    //                 }
    //
    //                 if (GUILayout.Button("Raise"))
    //                 {
    //                     var targetType = target.GetType();
    //                     var targetField = targetType.GetFieldRecursive("debugValue",
    //                         BindingFlags.Instance | BindingFlags.NonPublic);
    //                     var debugValue = targetField.GetValue(target);
    //
    //                     var raiseMethod = targetType.BaseType.GetMethod("Raise",
    //                         BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
    //                     raiseMethod?.Invoke(target, new[] {debugValue});
    //                 }
    //             }
    //         }
    //     }
    // }
#endif
}