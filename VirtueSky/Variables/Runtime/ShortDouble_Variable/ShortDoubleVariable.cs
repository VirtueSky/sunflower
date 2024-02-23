using UnityEngine;
using VirtueSky.DataType;
using VirtueSky.Inspector;

namespace VirtueSky.Variables
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Variables/ShortDouble", fileName = "short_double_variable")]
    [EditorIcon("scriptable_variable")]
    public class ShortDoubleVariable : BaseVariable<ShortDouble>
    {
    }
}