using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CinderUtils.Events;


public class HealthPack : Collectible {
    // ==================== Configuration ====================
    [Header("On Collect")]
    [SerializeField] float healthAmount = 10f;

    // ===================== Custom Code =====================
    protected override void Collect(Collider other) {
        EventBus.Raise<CollectibleEvent>(new() { 
            collectible = this, 
            resourceType = typeof(Health),
            amount = healthAmount,
        });
    }
}
