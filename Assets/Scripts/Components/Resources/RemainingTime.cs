using System;
using UnityEngine;


public class RemainingTime : Resource {
    // ==================== Configuration ====================
    public override ResourceKind Kind => ResourceKind.Plentiful;

    // ====================== Variables ======================
    public bool TimesUp => Amount <= float.Epsilon;
    public override string ValuesString => $"{Amount:0}s";


    // ===================== Custom Code =====================
    public void SetMax(float max) { 
        Max = max;
    }
    public void Tick() {
        Amount -= Math.Max(0, Time.deltaTime);
    }

    // ================== Outside Facing API =================
    public event Action OnTimesUp;
    protected override void TriggerOnChange(float prev, float next) {
        base.TriggerOnChange(prev, next);

        if (TimesUp) {
            OnTimesUp?.Invoke();
        }
    }
}
