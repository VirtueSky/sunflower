using System.Collections.Generic;
using UnityEngine;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "ScriptableObject Event/DictionaryEvent")]
    public class DictionaryEvent : BaseEvent<Dictionary<string, object>>
    {
    }
}