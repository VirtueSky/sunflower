using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/GameObject Event/Int Result",
        fileName = "gameobject_event_int_result")]
    [EditorIcon("scriptable_event")]
    public class GameObjectEventIntResult : BaseEvent<GameObject, int>
    {
    }
}