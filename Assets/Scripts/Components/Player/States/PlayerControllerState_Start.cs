using System;
using UnityEngine;

public class PlayerControllerState_Start : PlayerControllerState {
    // ====================== Variables ======================
    public override PlayerController.State Key => PlayerController.State.START;

    // ===================== Constructor =====================
    public PlayerControllerState_Start(PlayerController context) : base(context) { }

    // ===================== Custom Code =====================
    public override PlayerController.State NextState() { 
        return PlayerController.State.IDLE; 
    }
}
