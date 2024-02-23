using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Variables
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Variables/Rect", fileName = "rect_variable")]
    [EditorIcon("scriptable_variable")]
    public class RectVariable : BaseVariable<Rect>
    {
    }
}