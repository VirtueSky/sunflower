using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/String Event/Float Result",
        fileName = "string_event_float_result")]
    [EditorIcon("scriptable_event")]
    public class StringEventFloatResult : BaseEvent<string, float>
    {
    }
}