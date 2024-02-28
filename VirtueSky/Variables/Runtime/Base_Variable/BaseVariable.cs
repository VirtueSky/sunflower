using System;
using UnityEngine;
using UnityEngine.Serialization;
using VirtueSky.DataStorage;
using VirtueSky.Events;
using VirtueSky.Inspector;

namespace VirtueSky.Variables
{
    public class BaseVariable<TType> : BaseEvent<TType>, IVariable<TType>, ISerializationCallbackReceiver
    {
        [SerializeField] private TType initializeValue;
        [SerializeField] private bool isSetData;

        [ShowIf(nameof(isSetData))] [SerializeField]
        private bool isSaveData;

        [SerializeField] private bool isRaiseEvent;
        [NonSerialized] TType runtimeValue;

        public TType Value
        {
            get => isSetData ? GameData.Get(Id, initializeValue) : runtimeValue;
            set
            {
                if (isSetData)
                {
                    GameData.Set(Id, value);
                    if (isSaveData)
                    {
                        GameData.Save();
                    }
                }
                else
                {
                    runtimeValue = value;
                }

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