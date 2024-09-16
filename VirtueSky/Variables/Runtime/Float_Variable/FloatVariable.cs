using UnityEngine;
using VirtueSky.DataStorage;
using VirtueSky.Inspector;


namespace VirtueSky.Variables
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Variables/Float", fileName = "float_variables")]
    [EditorIcon("scriptable_variable")]
    public class FloatVariable : BaseVariable<float>
    {
        [Tooltip("Clamps the value of this variable to a minimum and maximum.")] [SerializeField]
        private bool isClamped;

        [Tooltip("If clamped, sets the minimum and maximum")] [SerializeField, ShowIf(nameof(isClamped)), Indent]
        private Vector2 minMax = new(0, 100);

        public bool IsClamped => isClamped;

        public Vector2 MinMax
        {
            get => minMax;
            set => minMax = value;
        }

        public float Min
        {
            get => minMax.x;
            set => minMax.x = value;
        }

        public float Max
        {
            get => minMax.y;
            set => minMax.y = value;
        }

        public void Add(float value)
        {
            Value += value;
        }

        public override float Value
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