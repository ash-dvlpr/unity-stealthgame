using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : BaseStateMachine<PlayerController.State> {
    public enum State : int {
        START    = 0, // default
        IDLE     = 1,
        MOVING   = 2,
        JUMPING  = 3,
        SWIMMING = 4,
    }

    // ==================== Configuration ====================
    [field: SerializeField] public PlayerConfig Config { get; private set; }

    // ======================= Context =======================
    public PlayerInput Input { get; private set; }
    public CharacterController CharacterController { get; private set; }
    //public Animator Animator { get; private set; }

    // ===================== Unity Stuff =====================
    protected override void Awake() {
        Input = GetComponent<PlayerInput>();
        CharacterController = GetComponent<CharacterController>();

        //Agent = GetComponent<NavMeshAgent>()
        //FOV = GetComponent<FieldOfView>()
        //Animator = GetComponentInChildren<Awnimator>();

        base.Awake();
    }

    // ===================== Custom Code =====================
    protected override void InitializeStates() {
        states[State.START] = new PlayerControllerState_Start(this);
        // TODO: Add other states

        ActiveState = states[State.START];
    }
}
