using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event/Boolean Event", fileName = "bool_event")]
    [EditorIcon("scriptable_event")]
    public class BooleanEvent : BaseEvent<bool>
    {
    }
}