using System;
using UnityEngine;


public class PlayerControllerState_Idle : PlayerControllerState {
    // ====================== Variables ======================
    public override PlayerController.State Key => PlayerController.State.IDLE;

    // ===================== Constructor =====================
    public PlayerControllerState_Idle(PlayerController context) : base(context) { }

    // ===================== Custom Code =====================
    public override PlayerController.State NextState() {

        // Otherwise stay on this state
        return Key; 
    }
}
