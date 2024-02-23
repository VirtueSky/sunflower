using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event/Integer Event", fileName = "int_event")]
    [EditorIcon("scriptable_event")]
    public class IntegerEvent : BaseEvent<int>
    {
    }
}