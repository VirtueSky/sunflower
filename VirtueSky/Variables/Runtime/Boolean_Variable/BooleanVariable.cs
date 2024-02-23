using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Variables
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Variables/Boolean", fileName = "bool_variables")]
    [EditorIcon("scriptable_variable")]
    public class BooleanVariable : BaseVariable<bool>
    {
    }
}