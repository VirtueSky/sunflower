#if VIRTUESKY_IAP
using UnityEngine;

namespace VirtueSky.Iap
{
    public abstract class IapPurchaseFailed : ScriptableObject
    {
        public abstract void Raise();
    }
}
#endif