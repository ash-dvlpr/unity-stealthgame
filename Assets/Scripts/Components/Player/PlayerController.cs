using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using Cinemachine;

using GameExtensions;
using CinderUtils.Extensions;

[RequireComponent(typeof(CharacterController), typeof(Animator), typeof(PlayerInputHandle))]
public class PlayerController : BaseStateMachine<PlayerController.State> {
    public enum State : int {
        IDLE     = 0, // default
        MOVING   = 1,
        JUMPING  = 2,
    }

    // ==================== Configuration ====================
    [field: SerializeField] public PlayerConfig Config { get; private set; }

    [field: Header("Cinemachine")]
    [field: SerializeField] public GameObject CameraRoot;
    [field: SerializeField] public CinemachineVirtualCamera VirtualCamera;
    [field: SerializeField] public Volume PostProcessingVolume;

    // ======================= Context =======================
    public GameObject MainCamera { get; private set; }
    public CharacterController CharacterController { get; private set; }
    public PlayerInput PlayerInput { get; private set; }
    public PlayerInputHandle Input { get; private set; }
    public Animator Animator { get; private set; }
#if UNITY_EDITOR
    public State _currentState;
#endif

    // ====================== Variables ======================
    [field: NonSerialized] public bool Grounded { get; private set; } = true;
    [field: NonSerialized] public bool Wet { get; private set; } = false;
    public bool OnJumpCooldown => JumpCooldownTimer > 0f; // TODO: Update timer
    public bool OnFallCooldown => false; // TODO: timer
    public bool CanJump => Grounded;// && !Wet;

    bool IsCurrentDeviceMouse => PlayerInput.currentControlScheme == "KeyboardMouse";

    // Camera
    float _targetYaw;
    float _targetPitch;
    CinemachineBasicMultiChannelPerlin _cameraNoise;
    //Vignette _hitVignette;

    // Jumping
    float _groundCheckRadius = .28f;
    float _groundCheckOffset = .1f;

    Vector3 _groundCheckPosition => transform.position.OffsetY(_groundCheckOffset);
    LayerMask _groundMask;
    LayerMask _waterMask;

    // Movement
    [NonSerialized] public float VerticalVelocity = 0f;
    [NonSerialized] public float TargetSpeed = 0f;
    [NonSerialized] public float TargetRotation = 0f;

    float _speed;
    float _animationBlend;
    float _rotationVelocity;
    float _terminalVelocity = 53.0f;

    // Timers
    [NonSerialized] public float JumpCooldownTimer = 0f;
    [NonSerialized] public float FallCooldownDelta = 0f;

    // ===================== Unity Stuff =====================
    protected override void Awake() {
        MainCamera = Camera.main.gameObject;
        CharacterController = GetComponent<CharacterController>();
        PlayerInput = GetComponent<PlayerInput>();
        Input = GetComponent<PlayerInputHandle>();
        Animator = GetComponentInChildren<Animator>();

        _cameraNoise = VirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        // Cache some configs
        _groundCheckRadius = CharacterController.radius;
        _groundMask = Config.GroundLayers;
        _waterMask = Config.WaterLayers;

        base.Awake();
    }

    protected override void Start() {
        // Reset timers
        //JumpCooldownTimer = Config.JumpTimeout;
        //FallCooldownDelta = Config.FallTimeout;

        base.Start();
    }

    protected override void Update() {
        // TODO: Update timers
        ResetExcessiveGravity();
        UpdateSensors();

        base.Update();
#if UNITY_EDITOR
        _currentState = ActiveState.Key;
#endif

        ApplyGravity();
        ApplyMovement();
    }

    protected override void LateUpdate() {
        UpdateCameraRotation();
        UpdateCameraEffects();
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
        Gizmos.DrawSphere(_groundCheckPosition, _groundCheckRadius);
    }

    private void OnFootstep(AnimationEvent animationEvent) {
        if (animationEvent.animatorClipInfo.weight > 0.5f) {
            var clip = Wet ? Config.WetFootstepAudioClips.GetRandom(): Config.FootstepAudioClips.GetRandom();

            if (clip) AudioSource.PlayClipAtPoint(
                clip, transform.TransformPoint(CharacterController.center), Config.FootstepAudioVolume
            );
        }
    }

    private void OnLand(AnimationEvent animationEvent) {
        if (animationEvent.animatorClipInfo.weight > 0.5f) {
            var clip = Config.LandingAudioClip;
            if (clip) AudioSource.PlayClipAtPoint(
                clip, transform.TransformPoint(CharacterController.center), Config.FootstepAudioVolume
            );
        }
    }

    private void OnPlayerHit(AnimationEvent animationEvent) {
        ShakePlayerCameraAndApplyVignette();
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

    static float ClampAngle(float angle, float min, float max) {
        if (angle < -360f) angle += 360f;
        if (angle > 360f) angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }

    void UpdateCameraEffects() {
        _cameraNoise.m_AmplitudeGain -= Time.deltaTime;

        _cameraNoise.m_AmplitudeGain = Mathf.Clamp(_cameraNoise.m_AmplitudeGain, 0, Config.MaxCameraShakeIntensity);
        PostProcessingVolume.weight = Mathf.Clamp(_cameraNoise.m_AmplitudeGain, 0f, 1f);
    }

    void ShakePlayerCameraAndApplyVignette() {
        _cameraNoise.m_AmplitudeGain += Config.CameraShakeIntensity;
        PostProcessingVolume.weight = 1.0f;
    }

    void UpdateSensors() {
        Grounded = CheckOverlap(_groundMask);
        Wet = CheckOverlap(_waterMask);

        //? Update animator values
        Animator.SetBool(AnimatorID.Grounded, Grounded);
        Animator.SetBool(AnimatorID.Wet, Wet);
    }

    bool CheckOverlap(LayerMask mask) {
        return Physics.CheckSphere(_groundCheckPosition, _groundCheckRadius, mask, QueryTriggerInteraction.Ignore);
    }

    private void ResetExcessiveGravity() {
        if (Grounded || Wet) {
            //? Update animator values
            Animator.SetBool(AnimatorID.Jump, false);
            Animator.SetBool(AnimatorID.FreeFall, false);

            // reset the fall timeout timer
            // FallTimeoutDelta = FallTimeout

            // Avoid forces building up infinitelly when grounded
            if (VerticalVelocity <= float.Epsilon) {
                VerticalVelocity = -2f;
            }
        }
    }

    private void ApplyGravity() {
        // Apply gravity over time if under terminal.
        // We multiply by deltaTime twice to linearly speed up over time. (A = D/T^2)
        if (VerticalVelocity < _terminalVelocity) {
            VerticalVelocity += Config.Gravity * Time.deltaTime;
        }
    }

    private void ApplyMovement() {
        var currentVelocity = CharacterController.velocity; currentVelocity.y = 0;
        float currentHorizontalSpeed = currentVelocity.magnitude;

        //? Fix controller vs keyboard input differences
        float inputMagnitude = Input.analogMovement ? Input.move.magnitude : 1f;

        //? If we are not at target speed, accelerate to target speed
        if (currentHorizontalSpeed != TargetSpeed - .1f) {
            // Smooth out the acceleration, for a more organic speed change
            _speed = Mathf.Lerp(
                currentHorizontalSpeed,
                TargetSpeed * inputMagnitude,
                Time.deltaTime * Config.Acceleration
            );

            // Round speed to 3 decimal places
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else _speed = TargetSpeed;


        // Calculate the animationBlend value for the Animator blendTree
        _animationBlend = Mathf.Lerp(_animationBlend, TargetSpeed, Time.deltaTime * Config.Acceleration);
        if (_animationBlend <= float.Epsilon) _animationBlend = 0f;

        //? Apply the character's rotation
        if (Input.move != Vector2.zero) {
            float rotation = Mathf.SmoothDampAngle(
                transform.eulerAngles.y, TargetRotation, ref _rotationVelocity, Config.RotationSmoothTime
            );

            // Rotate to face input direction relative to camera position
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

        //? Apply calculated forces
        Vector3 targetDirection = Quaternion.Euler(0.0f, TargetRotation, 0.0f) * Vector3.forward;
        CharacterController.Move(
            targetDirection.normalized * ( _speed * Time.deltaTime )
            + new Vector3(0.0f, VerticalVelocity, 0.0f) * Time.deltaTime
        );

        //? Update animator values
        Animator.SetFloat(AnimatorID.Speed, _animationBlend);
        Animator.SetFloat(AnimatorID.MotionSpeed, inputMagnitude);
    }
}
