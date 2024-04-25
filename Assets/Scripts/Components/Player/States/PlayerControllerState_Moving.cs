using System;
using UnityEngine;


public class PlayerControllerState_Moving : PlayerControllerState {
    // ====================== Variables ======================
    public override PlayerController.State Key => PlayerController.State.MOVING;

    // ===================== Constructor =====================
    public PlayerControllerState_Moving(PlayerController context) : base(context) { }

    // ===================== Custom Code =====================
    public override PlayerController.State NextState() {
        // If jump was pressed, go to the jumping state
        if (Input.jump && CanJump) return PlayerController.State.JUMPING;

        // If ther's no movement to process, go to idle
        if (Input.move != Vector2.zero) return PlayerController.State.IDLE;
        
        // Otherwise stay on this state
        return Key; 
    }
}
