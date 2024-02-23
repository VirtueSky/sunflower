using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Variables
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Variables/Transform", fileName = "transform_variable")]
    [EditorIcon("scriptable_variable")]
    public class TransformVariable : BaseVariable<Transform>
    {
    }
}