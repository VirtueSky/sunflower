using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Float Event/Vector3 Result",
        fileName = "float_event_vector3_result")]
    [EditorIcon("scriptable_event")]
    public class FloatEventVector3Result : BaseEvent<float, Vector3>
    {
    }
}