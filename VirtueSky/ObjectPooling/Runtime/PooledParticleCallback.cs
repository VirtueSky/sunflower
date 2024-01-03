using System.Collections;
using UnityEngine;
using VirtueSky.Core;

namespace VirtueSky.ObjectPooling
{
    public class PooledParticleCallback : BaseMono
    {
        [SerializeField] private Pools pools;

        void OnParticleSystemStopped()
        {
            StartCoroutine(IEDespawn());
        }

        IEnumerator IEDespawn()
        {
            yield return null;
            pools.Despawn(gameObject);
        }
    }
}