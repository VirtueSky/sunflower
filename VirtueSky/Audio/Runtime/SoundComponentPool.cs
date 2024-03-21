using UnityEngine;
using VirtueSky.Inspector;
using VirtueSky.ObjectPooling;

namespace VirtueSky.Audio
{
    [CreateAssetMenu(menuName = "Sunflower/Audio/SoundComponentPool", fileName = "sound_component_pool")]
    [EditorIcon("scriptable_pool")]
    public class SoundComponentPool : GameObjectPool
    {
    }
}