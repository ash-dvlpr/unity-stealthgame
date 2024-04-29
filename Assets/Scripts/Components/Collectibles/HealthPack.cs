using System;
using UnityEngine;


public class HealthPack : Collectible<Health> {
    // ==================== Configuration ====================
    [Header("On Collect")]
    [SerializeField] float healthAmount = 10f;

    // ====================== Variables ======================
    public override float Amount => healthAmount;
}
