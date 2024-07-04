using UnityEngine;
using VirtueSky.Events;
using VirtueSky.Inspector;

namespace VirtueSky.TouchInput
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Touch Input/Event Touch Stationary",
        fileName = "input_event_touch_stationary")]
    [EditorIcon("scriptable_event")]
    public class InputEventTouchStationary : BaseEvent<Touch>
    {
    }
}