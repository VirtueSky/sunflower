using VirtueSky.Core;
using System;
using UnityEditor;
using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    public class EventNoParamResult<TResult> : BaseSO, IEventNoParamResult<TResult>
    {
        private Func<TResult> onRaised = null;
#if UNITY_EDITOR
        [Space(10)] [ShowIf(nameof(ConditionShow))] [ReadOnly, SerializeField]
        private TResult valueResult = default(TResult);

        [ShowIf(nameof(ConditionShow))]
        [Button("Raise")]
        private void DebugRaiseEvent()
        {
            valueResult = Raise();
        }

        private bool ConditionShow => EditorApplication.isPlaying;
#endif
        public TResult Raise()
        {
            TResult result = default;
            if (!Application.isPlaying) return result;

            if (onRaised != null) result = onRaised.Invoke();
            return result;
        }

        public event Func<TResult> OnRaised
        {
            add { onRaised += value; }
            remove { onRaised -= value; }
        }

        public void AddListener(Func<TResult> func)
        {
            onRaised += func;
        }

        public void RemoveListener(Func<TResult> func)
        {
            onRaised -= func;
        }

        public void RemoveAll()
        {
            onRaised = null;
        }
    }
}