using UnityEngine;
using VirtueSky.Events;
using VirtueSky.Inspector;

namespace VirtueSky.TouchInput
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Touch Input/Event Touch Cancel",
        fileName = "input_event_touch_cancel")]
    [EditorIcon("scriptable_event")]
    public class InputEventTouchCancel : BaseEvent<Touch>
    {
    }
}