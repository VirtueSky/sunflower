using UnityEngine;
using VirtueSky.Events;
using VirtueSky.Inspector;

namespace VirtueSky.TouchInput
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Touch Input/Event Touch Begin",
        fileName = "input_event_touch_begin")]
    [EditorIcon("scriptable_event")]
    public class InputEventTouchBegin : Vector3Event
    {
    }
}