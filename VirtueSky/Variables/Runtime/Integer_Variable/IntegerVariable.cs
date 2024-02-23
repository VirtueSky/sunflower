using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Variables
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Variables/Integer", fileName = "int_variable")]
    [EditorIcon("scriptable_variable")]
    public class IntegerVariable : BaseVariable<int>
    {
    }
}