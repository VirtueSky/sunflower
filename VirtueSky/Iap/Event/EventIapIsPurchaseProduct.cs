﻿using UnityEngine;
using VirtueSky.Events;

namespace VirtueSky.Iap
{
    [CreateAssetMenu(fileName = "iap_is_purchase_product.asset", menuName = "Iap/Is Purchase Product Event")]
    public class EventIapIsPurchaseProduct : BaseEvent<bool>
    {
    }
}