using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event/Float Event", fileName = "float_event")]
    [EditorIcon("scriptable_event")]
    public class FloatEvent : BaseEvent<float>
    {
    }
}