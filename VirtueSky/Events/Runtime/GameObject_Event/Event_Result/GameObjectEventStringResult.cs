using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/GameObject Event/String Result",
        fileName = "gameobject_event_string_result")]
    [EditorIcon("scriptable_event")]
    public class GameObjectEventStringResult : BaseEvent<GameObject, string>
    {
    }
}