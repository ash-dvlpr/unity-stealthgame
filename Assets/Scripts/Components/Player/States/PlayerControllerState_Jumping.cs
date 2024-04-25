using System;
using UnityEngine;


public class PlayerControllerState_Jumping : PlayerControllerState {
    // ====================== Variables ======================
    public override PlayerController.State Key => PlayerController.State.JUMPING;

    // ===================== Constructor =====================
    public PlayerControllerState_Jumping(PlayerController context) : base(context) { }

    // ===================== Custom Code =====================
    public override void Enter() {
        // Apply jump forces
        Context.VerticalVelocity = Mathf.Sqrt(Config.JumpHeight * -2f * Config.Gravity);
        
        //? Update animator values
        Context.Animator.SetBool(AnimatorID.Jump, true);
    }
    public override void Tick() {
        //? Update animator values
        if (!OnFallCooldown) Context.Animator.SetBool(AnimatorID.FreeFall, true);
    }

    public override void Exit() {
        Context.Animator.SetBool(AnimatorID.Jump, false);
        Context.TargetSpeed = 0;
    }


    public override PlayerController.State NextState() {
        if (Grounded) {
            // If we requested to jump and are able to, stay in the state and run the logic
            if (Input.jump && CanJump) { 
                Enter(); return Key;
            }

            // On the other hand, id there's an movement request, we go to moving, else idle.
            if (Input.move != Vector2.zero) return PlayerController.State.MOVING;
            else return PlayerController.State.IDLE;
        }
        
        // Otherwise stay on this state
        return Key; 
    }
}
