using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event/Transform Event", fileName = "transform_event")]
    [EditorIcon("scriptable_event")]
    public class TransformEvent : BaseEvent<Transform>
    {
    }
}