using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event/Vector3 Event", fileName = "vector3_event")]
    [EditorIcon("scriptable_event")]
    public class Vector3Event : BaseEvent<Vector3>
    {
    }
}