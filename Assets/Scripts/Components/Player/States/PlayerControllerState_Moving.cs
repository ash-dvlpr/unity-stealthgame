using System;
using UnityEngine;


public class PlayerControllerState_Moving : PlayerControllerState {
    // ====================== Variables ======================
    public override PlayerController.State Key => PlayerController.State.MOVING;

    // ===================== Constructor =====================
    public PlayerControllerState_Moving(PlayerController context) : base(context) { }

    // ===================== Custom Code =====================
    public override void Tick() {
        // Set target speed based on if sprint is pressed and if it's on water
        Context.TargetSpeed = Context.Wet
            ? Input.sprint ? Config.WetRunSpeed : Config.WetWalkSpeed
            : Input.sprint ? Config.RunSpeed : Config.WalkSpeed;

        // If there are no inputs, the speed is none
        if (Input.move == Vector2.zero) Context.TargetSpeed = 0.0f;

        // Calculate the new rotation
        Vector2 inputDirection = Input.move.normalized;
        Context.TargetRotation = Mathf.Atan2(inputDirection.x, inputDirection.y)
            * Mathf.Rad2Deg + Context.MainCamera.transform.eulerAngles.y;
    }

    public override PlayerController.State NextState() {
        // If jump was pressed, go to the jumping state
        if (Input.jump && CanJump) return PlayerController.State.JUMPING;

        // If ther's no movement to process, go to idle
        if (Input.move == Vector2.zero) return PlayerController.State.IDLE;

        // Otherwise stay on this state
        return Key;
    }
}
