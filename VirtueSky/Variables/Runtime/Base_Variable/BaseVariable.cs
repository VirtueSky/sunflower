using System;
using UnityEditor;
using UnityEngine;
using VirtueSky.DataStorage;
using VirtueSky.Events;
using VirtueSky.Inspector;

namespace VirtueSky.Variables
{
    public class BaseVariable<TType> : BaseEvent<TType>, IVariable<TType>, ISerializationCallbackReceiver, IGuidVariable
    {
        [ShowIf(nameof(isSetData)), ReadOnly, SerializeField]
        protected string id;

        [SerializeField] protected TType initializeValue;
        [SerializeField] protected bool isSetData;

        [ShowIf(nameof(isSetData))] [SerializeField]
        protected bool isSaveData;

        [SerializeField] protected bool isRaiseEvent;

        [NonSerialized] protected TType runtimeValue;
#if UNITY_EDITOR
        [ShowIf(nameof(ConditionShow))] [ReadOnly, SerializeField]
        protected TType currentValue;
#endif
        public TType InitializeValue => initializeValue;

        public string Guid
        {
            get => id;
            set => id = value;
        }

        private void OnEnable()
        {
#if UNITY_EDITOR
            currentValue = Value;
#endif
        }

        public virtual TType Value
        {
            get => isSetData ? GameData.Get(id, initializeValue) : runtimeValue;
            set
            {
                if (isSetData)
                {
                    GameData.Set(id, value);
                    if (isSaveData)
                    {
                        GameData.Save();
                    }
                }
                else
                {
                    runtimeValue = value;
                }
#if UNITY_EDITOR
                currentValue = value;
#endif
                if (isRaiseEvent)
                {
                    Raise(value);
                }
            }
        }

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            runtimeValue = initializeValue;
        }

        public void ResetValue()
        {
            Value = initializeValue;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}