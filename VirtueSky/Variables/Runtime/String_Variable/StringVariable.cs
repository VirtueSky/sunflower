using UnityEngine;
using VirtueSky.Inspector;


namespace VirtueSky.Variables
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Variables/String", fileName = "string_variable")]
    [EditorIcon("scriptable_variable")]
    public class StringVariable : BaseVariable<string>
    {
    }
}