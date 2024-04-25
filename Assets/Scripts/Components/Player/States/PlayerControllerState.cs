using System;
using UnityEngine;
using UnityEngine.InputSystem;


public abstract class PlayerControllerState : BaseState<PlayerController.State> {
    protected PlayerControllerState(PlayerController context) {
        this.Context = context;
    }

    // ====================== Variables ======================
    protected PlayerController Context { get; set; }
    //protected PlayerConfig.StateConfig StateConfig => Context.Config[Key];
    protected PlayerConfig Config => Context.Config;
    protected PlayerInputHandle Input => Context.Input;





    // ===================== Custom Code =====================
    //public override void Enter() {
    //    ApplyConfig();
    //}

    //public override void Tick() {

    //    JumpAndGravity();
    //    GroundedCheck();
    //    Move();
    //}

    //protected void ApplyConfig() {
    //    if (StateConfig.State != 0) {
    //        // Navigation
    //    }
    //}




    
}
