using System.Collections;
using VirtueSky.Core;

namespace VirtueSky.ObjectPooling
{
    public class PooledParticleCallback : BaseMono
    {
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