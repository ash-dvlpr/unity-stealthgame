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
    public bool Grounded { get; private set; } = true;
    public bool Wet { get; private set; } = false;

    bool IsCurrentDeviceMouse => PlayerInput.currentControlScheme == "KeyboardMouse";

    // Physics
    LayerMask _groundMask;
    LayerMask _waterMask;
    float _groundCheckRadius = .28f;

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

        // Cache some configs
        _groundMask = Config.GroundLayers;
        _waterMask = Config.WaterLayers;
        _groundCheckRadius = CharacterController.radius;

        base.Awake();
    }

    protected override void Update() {
        UpdateSensors();
        base.Update();
    }

    protected override void LateUpdate() {
        UpdateCameraRotation();
        base.LateUpdate();
    }

    void OnDrawGizmosSelected() {
        Color green = new Color(0.0f, 1.0f, 0.0f, 0.5f);
        Color blue  = new Color(0.0f, 0.0f, 1.0f, 0.5f);
        Color red   = new Color(1.0f, 0.0f, 0.0f, 0.5f);

        // Set the gizmo color depending on the sensors
        Gizmos.color = (Grounded, Wet) switch {
            (true, false) => green,
            (_, true) => blue,
            _ => red,
        };

        // When the object is selected, draw a gizmo in the position of, and matching radius of, the grounded collider
        Gizmos.DrawSphere(transform.position, _groundCheckRadius);
    }

    // ===================== Custom Code =====================
    protected override void InitializeStates() {
        states[State.IDLE] = new PlayerControllerState_Idle(this);
        states[State.MOVING] = new PlayerControllerState_Moving(this);
        states[State.JUMPING] = new PlayerControllerState_Jumping(this);
        // TODO: Swimming

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

    void UpdateSensors() {
        Grounded = CheckOverlap(_groundMask);
        Wet = CheckOverlap(_waterMask);
    }

    bool CheckOverlap(LayerMask mask) { 
        return Physics.CheckSphere(transform.position, _groundCheckRadius, mask, QueryTriggerInteraction.Ignore);
    }

    static float ClampAngle(float angle, float min, float max) {
        if (angle < -360f) angle += 360f;
        if (angle > 360f) angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }
}
