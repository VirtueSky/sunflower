using System.Collections.Generic;
using UnityEngine;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Event/DictionaryEvent")]
    public class DictionaryEvent : BaseEvent<Dictionary<string, object>>
    {
    }
}