using UnityEngine;
using VirtueSky.Events;
using VirtueSky.Inspector;

namespace VirtueSky.TouchInput
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Touch Input/Event Mouse Up",
        fileName = "input_event_mouse_up")]
    [EditorIcon("scriptable_event")]
    public class InputEventMouseUp : Vector3Event
    {
    }
}