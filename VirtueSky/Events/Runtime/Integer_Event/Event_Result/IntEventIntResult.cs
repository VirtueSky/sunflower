using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Int Event/Int Result",
        fileName = "int_event_int_result")]
    [EditorIcon("scriptable_event")]
    public class IntEventIntResult : BaseEvent<int, int>
    {
    }
}