using UnityEngine;
using VirtueSky.DataStorage;
using VirtueSky.Inspector;

namespace VirtueSky.Variables
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Variables/Integer", fileName = "int_variable")]
    [EditorIcon("scriptable_variable")]
    public class IntegerVariable : BaseVariable<int>
    {
        [Tooltip("Clamps the value of this variable to a minimum and maximum.")] [SerializeField]
        private bool isClamped;

        public bool IsClamped => isClamped;

        [Tooltip("If clamped, sets the minimum and maximum")] [SerializeField, ShowIf(nameof(isClamped)), Indent]
        private Vector2Int minMax = new(0, 100);

        public Vector2Int MinMax
        {
            get => minMax;
            set => minMax = value;
        }

        public int Min
        {
            get => minMax.x;
            set => minMax.x = value;
        }

        public int Max
        {
            get => minMax.y;
            set => minMax.y = value;
        }

        public void Add(int value)
        {
            Value += value;
        }

        public override int Value
        {
            get => isSetData ? GameData.Get(Id, initializeValue) : runtimeValue;
            set
            {
                var clampedValue = IsClamped ? Mathf.Clamp(value, minMax.x, minMax.y) : value;
                if (isSetData)
                {
                    GameData.Set(Id, clampedValue);
                    if (isSaveData)
                    {
                        GameData.Save();
                    }
                }
                else
                {
                    runtimeValue = clampedValue;
                }
#if UNITY_EDITOR
                currentValue = clampedValue;
#endif
                if (isRaiseEvent)
                {
                    Raise(clampedValue);
                }
            }
        }
    }
}