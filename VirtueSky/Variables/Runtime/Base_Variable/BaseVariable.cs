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

        [Tooltip("Set initial value for scriptable variable"), SerializeField]
        protected TType initializeValue;

        [Tooltip("Set data into dictionary, if not set data then scriptable variable will action as runtime variable"),
         SerializeField]
        protected bool isSetData;

        [Tooltip(
             "Save data from dictionary to file when value is changed. If not saved, data will be saved to file when game is paused or quit"),
         ShowIf(nameof(isSetData)), SerializeField]
        protected bool isSaveData;

        [Tooltip("Raise event when value is changed"), SerializeField]
        protected bool isRaiseEvent;

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