using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event/GameObject Event", fileName = "gameobject_event")]
    [EditorIcon("scriptable_event")]
    public class GameObjectEvent : BaseEvent<GameObject>
    {
    }
}