using System;
using UnityEngine;


public class PlayerControllerState_Jumping : PlayerControllerState {
    // ====================== Variables ======================
    public override PlayerController.State Key => PlayerController.State.JUMPING;

    // ===================== Constructor =====================
    public PlayerControllerState_Jumping(PlayerController context) : base(context) { }

    // ===================== Custom Code =====================
    public override PlayerController.State NextState() {
        // If we are grounded

        // If jump was pressed, go to the jumping state
        if (Input.jump) return PlayerController.State.JUMPING;

        // If ther's no movement to process, go to idle
        if (Input.move != Vector2.zero) return PlayerController.State.IDLE;
        
        // Otherwise stay on this state
        return Key; 
    }
}
