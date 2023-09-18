using System;
using UnityEngine;
using VirtueSky.DataStorage;
using VirtueSky.Events;

namespace VirtueSky.Variables
{
    public class BaseVariable<TType> : BaseEvent<TType>, IVariable<TType>, ISerializationCallbackReceiver
    {
        [SerializeField] TType initializeValue;
        [SerializeField] bool isSavable;
        [SerializeField] private bool isSaveInStorage;
        [SerializeField] bool isRaiseEvent;
        [NonSerialized] TType runtimeValue;

        public TType Value
        {
            get => isSavable ? GameData.Get(Id, initializeValue) : runtimeValue;
            set
            {
                if (isSavable)
                {
                    GameData.Set(Id, value);
                    if (isSaveInStorage)
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