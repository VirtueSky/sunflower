﻿using UnityEngine;

namespace VirtueSky.Iap
{
    public abstract class IapPurchaseSuccess : ScriptableObject
    {
        public abstract void Raise();
    }
}