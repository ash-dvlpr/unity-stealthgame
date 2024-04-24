using System;
using System.Collections.Generic;
using UnityEngine;


public abstract class BaseStateMachine<EState> : MonoBehaviour where EState : Enum {
    // ====================== Variables ======================
    protected Dictionary<EState, BaseState<EState>> states = new();
    protected BaseState<EState> ActiveState;

    // ===================== Unity Stuff =====================
    protected virtual void Awake() {
        InitializeStates();
    }

    protected virtual void Start() {
        ActiveState.Enter();
    }

    protected virtual void Update() {
        EState nextState = ActiveState.NextState();

        // If still on the same state, update the staet
        if (nextState.Equals(ActiveState.Key)) {
            ActiveState.Tick();
        }
        // If not, transition to the next state; 
        else {
            TransitionState(nextState);
        }
    }

    protected virtual void FixedUpdate() {
        ActiveState.FixedTick();
    }

    // ===================== Custom Code =====================
    protected abstract void InitializeStates();
    void TransitionState(EState newState) {
        ActiveState.Exit();
        ActiveState = states[newState];
        ActiveState.Enter();
    }
}
