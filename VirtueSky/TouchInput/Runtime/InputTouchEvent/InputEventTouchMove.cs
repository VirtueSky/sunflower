using UnityEngine;
using VirtueSky.Events;
using VirtueSky.Inspector;

namespace VirtueSky.TouchInput
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Touch Input/Event Touch Move",
        fileName = "input_event_touch_move")]
    [EditorIcon("scriptable_event")]
    public class InputEventTouchMove : BaseEvent<Touch>
    {
    }
}