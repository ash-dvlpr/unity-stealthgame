using System;
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
    public event Action OnHit;
    protected override void TriggerOnChange(float prev, float next) {
        base.TriggerOnChange(prev, next);

        if (prev < next) OnHit?.Invoke();
        if (next.Equals(0)) OnDeath?.Invoke();
    }
}
