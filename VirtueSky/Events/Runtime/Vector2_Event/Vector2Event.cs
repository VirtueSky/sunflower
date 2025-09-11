using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event/Vector2 Event", fileName = "vector2_event")]
    [EditorIcon("scriptable_event")]
    public class Vector2Event : BaseEvent<Vector2>
    {
    }
}