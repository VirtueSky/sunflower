using System.Collections.Generic;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [EditorIcon("scriptable_event_listener")]
    public class
        DictionaryEventListener : BaseEventListener<Dictionary<string, object>, DictionaryEvent,
        DictionaryEventResponse>
    {
    }
}