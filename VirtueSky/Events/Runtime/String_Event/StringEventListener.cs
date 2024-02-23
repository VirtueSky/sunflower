using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [EditorIcon("scriptable_event_listener")]
    public class StringEventListener : BaseEventListener<string, StringEvent, StringEventResponse>
    {
    }
}