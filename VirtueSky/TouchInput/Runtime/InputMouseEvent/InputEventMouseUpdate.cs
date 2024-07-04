using UnityEngine;
using VirtueSky.Events;
using VirtueSky.Inspector;

namespace VirtueSky.TouchInput
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Touch Input/Event Mouse Update",
        fileName = "input_event_mouse_update")]
    [EditorIcon("scriptable_event")]
    public class InputEventMouseUpdate : Vector3Event
    {
    }
}