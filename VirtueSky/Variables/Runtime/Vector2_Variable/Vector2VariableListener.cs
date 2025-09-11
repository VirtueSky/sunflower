using UnityEngine;
using VirtueSky.Events;
using VirtueSky.Inspector;

namespace VirtueSky.Variables
{
    [EditorIcon("scriptable_event_listener")]
    public class Vector2VariableListener : BaseVariableListener<Vector2, Vector2Variable, Vector2EventResponse>
    {
    }
}