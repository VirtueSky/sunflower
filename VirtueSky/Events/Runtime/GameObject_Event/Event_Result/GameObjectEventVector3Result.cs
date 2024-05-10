using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/GameObject Event/Vector3 Result",
        fileName = "gameobject_event_vector3_result")]
    [EditorIcon("scriptable_event")]
    public class GameObjectEventVector3Result : BaseEvent<GameObject, Vector3>
    {
    }
}