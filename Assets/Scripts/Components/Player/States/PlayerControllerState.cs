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
    protected bool OnFallCooldown => Context.OnFallCooldown;
    protected bool OnJumpCooldown => Context.OnJumpCooldown;
    protected bool CanJump => Context.CanJump;

    // ===================== Custom Code =====================
    //public override void Enter() {
    //    ApplyConfig();
    //}
    
}
