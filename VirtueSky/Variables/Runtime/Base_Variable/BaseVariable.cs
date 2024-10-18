using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using VirtueSky.DataStorage;
using VirtueSky.Events;
using VirtueSky.Inspector;
using VirtueSky.Utils;

namespace VirtueSky.Variables
{
    public class BaseVariable<TType> : BaseEvent<TType>, IVariable<TType>, ISerializationCallbackReceiver, IGuidVariable
    {
        [TitleColor("Id", CustomColor.Gold, CustomColor.Aqua), ShowIf(nameof(isSetData)), SerializeField]
        private TypeId typeId;

        [ShowIf(nameof(IsShowGuid)), ReadOnly, SerializeField]
        protected string guid;

        [ShowIf(nameof(IsShowCustomId)), SerializeField]
        private string customId;


        [TitleColor("Init value", CustomColor.Chartreuse, CustomColor.OrangeVariant), Tooltip("Set initial value for scriptable variable"), SerializeField]
        protected TType initializeValue;

        [TitleColor("Save Data", CustomColor.Tomato, CustomColor.MediumSpringGreen),
         Tooltip("Set data into dictionary, if not set data then scriptable variable will action as runtime variable"),
         SerializeField]
        protected bool isSetData;

        [Tooltip(
             "Save data from dictionary to file when value is changed. If not saved, data will be saved to file when game is paused or quit"),
         ShowIf(nameof(isSetData)), SerializeField]
        protected bool isSaveData;

        [TitleColor("Raise event", CustomColor.DeepSkyBlue, CustomColor.Magenta), Tooltip("Raise event when value is changed"), SerializeField]
        protected bool isRaiseEvent;

        [NonSerialized] protected TType runtimeValue;
#if UNITY_EDITOR
        [ShowIf(nameof(ConditionShow))] [ReadOnly, SerializeField]
        protected TType currentValue;
#endif
        public TType InitializeValue => initializeValue;

        public string Guid
        {
            get => guid;
            set => guid = value;
        }

        public string CustomId
        {
            get => customId;
            set => customId = value;
        }

        public string Id => typeId switch
        {
            TypeId.Guid => guid,
            _ => customId,
        };

        private void OnEnable()
        {
#if UNITY_EDITOR
            currentValue = Value;
#endif
        }

        public virtual TType Value
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

        private bool IsShowGuid => isSetData && typeId == TypeId.Guid;
        private bool IsShowCustomId => isSetData && typeId == TypeId.CustomId;
    }

    public enum TypeId
    {
        Guid,
        CustomId
    }
}