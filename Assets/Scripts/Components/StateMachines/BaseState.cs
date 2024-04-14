using System;


public abstract class BaseState<EState> where EState : Enum {
    // ====================== Variables ======================
    public abstract EState Key { get; }

    // ===================== Custom Code =====================
    public abstract void Enter();
    public abstract void Exit();
    public abstract void Tick();
    public abstract void FixedTick();
    public abstract EState NextState();

}
