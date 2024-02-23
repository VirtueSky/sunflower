using UnityEngine;
using VirtueSky.Events;
using VirtueSky.Inspector;

namespace VirtueSky.Variables
{
    [EditorIcon("scriptable_event_listener")]
    public class Vector3VariableListener : BaseVariableListener<Vector3, Vector3Variable, Vector3EventResponse>
    {
    }
}