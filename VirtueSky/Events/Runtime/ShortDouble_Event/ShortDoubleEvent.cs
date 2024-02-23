using UnityEngine;
using VirtueSky.DataType;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event/ShortDouble Event", fileName = "short_double_event")]
    [EditorIcon("scriptable_event")]
    public class ShortDoubleEvent : BaseEvent<ShortDouble>
    {
    }
}