using System;
using UnityEngine;


public class PlayerControllerState_Idle : PlayerControllerState {
    // ====================== Variables ======================
    public override PlayerController.State Key => PlayerController.State.IDLE;

    // ===================== Constructor =====================
    public PlayerControllerState_Idle(PlayerController context) : base(context) { }

    // ===================== Custom Code =====================
    public override void Enter() {
        Context.TargetSpeed = 0;
    }
    public override PlayerController.State NextState() {
        // If the user has submitted movement, go to moving
        if (Input.move != Vector2.zero) return PlayerController.State.MOVING;
        
        // If jump was pressed, go to the jumping state
        if (Input.jump && CanJump) return PlayerController.State.JUMPING;

        // Otherwise stay on this state
        return Key; 
    }
}
