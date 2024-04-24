using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CinderUtils.Events;


public class HealthPack : Collectible<Health> {
    // ==================== Configuration ====================
    [Header("On Collect")]
    [SerializeField] float healthAmount = 10f;

    // ====================== Variables ======================
    public override float Amount => healthAmount;
}
