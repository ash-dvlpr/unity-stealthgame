using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Health : Resource {
    // ==================== Configuration ====================
    public override ResourceKind Kind => ResourceKind.Plentiful;

    // ====================== Variables ======================
    public bool IsAlive => Amount > 0;

    // ===================== Custom Code =====================
    public void Heal(float amount) {
        Amount += Math.Max(0, amount);
    }

    public void Damage(float amount) {
        Amount -= Math.Max(0, amount);
    }

    // ================== Outside Facing API =================
    public event Action OnDeath;
    protected override void TriggerOnChange(float prev, float next) {
        base.TriggerOnChange(prev, next);

        if (Amount.Equals(0)) {
            OnDeath?.Invoke();
        }
    }
}
