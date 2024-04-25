using System;
using UnityEngine;

public abstract class PlayerControllerState : BaseState<PlayerController.State> {
    protected PlayerControllerState(PlayerController context) {
        this.Context = context;
    }

    // ====================== Variables ======================
    protected PlayerController Context { get; set; }
    protected PlayerConfig.StateConfig StateConfig => Context.Config[Key];
    protected PlayerConfig Config => Context.Config;

    // ===================== Custom Code =====================
    public override void Enter() {
        ApplyConfig();
    }
    public override void Tick() {
        //UpdateAnimator();
    }

    //protected virtual void UpdateAnimator() {
    //    Context.Animator.SetFloat("Speed", Context.Agent.velocity.magnitude);
    //}

    protected void ApplyConfig() {
        //if (StateConfig.State != 0) {
        //    // Navigation
        //}
    }
}
