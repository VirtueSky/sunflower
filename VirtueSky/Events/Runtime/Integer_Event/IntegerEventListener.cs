using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [EditorIcon("scriptable_event_listener")]
    public class IntegerEventListener : BaseEventListener<int, IntegerEvent, IntegerEventResponse>
    {
    }
}