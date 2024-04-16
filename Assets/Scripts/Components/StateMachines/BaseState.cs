using System;


public abstract class BaseState<EState> where EState : Enum {
    // ====================== Variables ======================
    public abstract EState Key { get; }

    // ===================== Custom Code =====================
    public virtual void Enter() {}
    public virtual void Exit() {}
    public virtual void Tick() {}
    public virtual void FixedTick() { }
    public virtual EState NextState() => Key;

}
