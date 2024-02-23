using UnityEngine;
using VirtueSky.Events;
using VirtueSky.Inspector;

namespace VirtueSky.Variables
{
    [EditorIcon("scriptable_event_listener")]
    public class ObjectVariableListener : BaseVariableListener<Object, ObjectVariable, ObjectEventResponse>
    {
    }
}