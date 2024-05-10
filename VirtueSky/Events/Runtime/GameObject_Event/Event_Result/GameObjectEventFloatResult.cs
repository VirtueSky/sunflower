using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/GameObject Event/Float Result",
        fileName = "gameobject_event_float_result")]
    [EditorIcon("scriptable_event")]
    public class GameObjectEventFloatResult : BaseEvent<GameObject, float>
    {
    }
}