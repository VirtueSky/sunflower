using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/String Event/Vector3 Result",
        fileName = "string_event_vector3_result")]
    [EditorIcon("scriptable_event")]
    public class StringEventVector3Result : BaseEvent<string, Vector3>
    {
    }
}