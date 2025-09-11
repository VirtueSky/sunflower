using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Variables
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Variables/Vector3", fileName = "vector3_variable")]
    [EditorIcon("scriptable_variable")]
    public class Vector3Variable : BaseVariable<Vector3>
    {
        public Vector2 ToVector2()
        {
            return new Vector2(Value.x, Value.y);
        }
    }
}