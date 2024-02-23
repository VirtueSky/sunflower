using System.Collections.Generic;
using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event/DictionaryEvent", fileName = "dictionary_event")]
    [EditorIcon("scriptable_event")]
    public class DictionaryEvent : BaseEvent<Dictionary<string, object>>
    {
    }
}