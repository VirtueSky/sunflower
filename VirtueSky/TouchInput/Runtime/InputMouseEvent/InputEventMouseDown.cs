using UnityEngine;
using VirtueSky.Events;
using VirtueSky.Inspector;

namespace VirtueSky.TouchInput
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Touch Input/Event Mouse Down",
        fileName = "input_event_mouse_down")]
    [EditorIcon("scriptable_event")]
    public class InputEventMouseDown : Vector3Event
    {
    }
}