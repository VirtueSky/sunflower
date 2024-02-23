using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Variables
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Variables/Object", fileName = "object_variables")]
    [EditorIcon("scriptable_variable")]
    public class ObjectVariable : BaseVariable<Object>
    {
    }
}