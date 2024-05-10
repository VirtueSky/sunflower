using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Float Event/GameObject Result",
        fileName = "float_event_gameobject_result")]
    [EditorIcon("scriptable_event")]
    public class FloatEventGameObjectResult : BaseEvent<float, GameObject>
    {
    }
}