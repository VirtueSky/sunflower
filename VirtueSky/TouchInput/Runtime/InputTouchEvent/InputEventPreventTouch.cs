using UnityEngine;
using VirtueSky.Events;
using VirtueSky.Inspector;

namespace VirtueSky.TouchInput
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Touch Input/Event Prevent Touch",
        fileName = "input_event_prevent_touch")]
    [EditorIcon("scriptable_event")]
    public class InputEventPreventTouch : BooleanEvent
    {
    }
}