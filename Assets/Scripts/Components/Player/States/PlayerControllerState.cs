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
    protected bool Grounded => Context.Grounded;
    protected bool Wet => Context.Wet;
    protected bool OnJumpCooldown => false; // TODO: timer
    protected bool CanJump => Context.Grounded && !Context.Wet && !OnJumpCooldown;





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
