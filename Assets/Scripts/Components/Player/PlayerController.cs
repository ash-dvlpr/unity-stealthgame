using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController), typeof(Animator), typeof(PlayerInputHandle))]
public class PlayerController : BaseStateMachine<PlayerController.State> {
    public enum State : int {
        IDLE     = 0, // default
        MOVING   = 1,
        JUMPING  = 2,
        //SWIMMING = 3,
    }

    // ==================== Configuration ====================
    [field: SerializeField] public PlayerConfig Config { get; private set; }

    [field: Header("Cinemachine")]
    [field: SerializeField] public GameObject CameraRoot;

    // ======================= Context =======================
    public GameObject MainCamera { get; private set; }
    public CharacterController CharacterController { get; private set; }
    public PlayerInput PlayerInput { get; private set; }
    public PlayerInputHandle Input { get; private set; }
    public Animator Animator { get; private set; }

    // ====================== Variables ======================
    bool IsCurrentDeviceMouse => PlayerInput.currentControlScheme == "KeyboardMouse";
    
    // Camera
    float _targetYaw;
    float _targetPitch;

    // ===================== Unity Stuff =====================
    protected override void Awake() {
        MainCamera = Camera.main.gameObject;
        CharacterController = GetComponent<CharacterController>();
        PlayerInput = GetComponent<PlayerInput>();
        Input = GetComponent<PlayerInputHandle>();
        Animator = GetComponentInChildren<Animator>();

        base.Awake();
    }

    protected override void LateUpdate() {
        UpdateCameraRotation();
        base.LateUpdate();
    }

    // ===================== Custom Code =====================
    protected override void InitializeStates() {
        states[State.IDLE] = new PlayerControllerState_Idle(this);
        // TODO: Add other states

        ActiveState = states[State.IDLE];
    }

    void UpdateCameraRotation() {
        // If there has been an input
        if (Input.look.sqrMagnitude >= .01f) {
            // Fix for controller input not behaving like 
            float deltaTimeFactor = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

            _targetYaw += Input.look.x * deltaTimeFactor;
            _targetPitch += Input.look.y * deltaTimeFactor;
        }

        // Clamp our rotations (0-360 degrees)
        _targetYaw = ClampAngle(_targetYaw, float.MinValue, float.MaxValue);
        _targetPitch = ClampAngle(_targetPitch, Config.LowerVerticalLimit, Config.UpperVerticalLimit);

        // Rotate the camera root that the cinemachine camera will follow
        CameraRoot.transform.rotation = Quaternion.Euler(_targetPitch, _targetYaw, 0.0f);
    }

    static float ClampAngle(float angle, float min, float max) {
        if (angle < -360f) angle += 360f;
        if (angle > 360f) angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }
}
