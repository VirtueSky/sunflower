using UnityEngine;
using VirtueSky.Events;
using VirtueSky.Inspector;

namespace VirtueSky.TouchInput
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Touch Input/Event Touch End",
        fileName = "input_event_touch_end")]
    [EditorIcon("scriptable_event")]
    public class InputEventTouchEnd : Vector3Event
    {
    }
}