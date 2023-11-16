using System.Collections.Generic;
using UnityEngine;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Event/DictionaryEvent", fileName = "dictionary_event")]
    public class DictionaryEvent : BaseEvent<Dictionary<string, object>>
    {
    }
}