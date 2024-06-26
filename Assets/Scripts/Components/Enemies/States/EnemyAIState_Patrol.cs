using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIState_Patrol : EnemyAIState {
    // ====================== Variables ======================
    public override EnemyAI.EState Key => EnemyAI.EState.PATROL;
    IEnumerator<Transform> sequence;

    // ===================== Constructor =====================
    public EnemyAIState_Patrol(EnemyAI context) : base(context) { }

    // ===================== Custom Code =====================
    public override void Enter() {
        base.Enter();

        Context.OnPatrolChanged += OnPatrolChanged;
        
        RestartSequence();
        CyclePatrol();

        Context.NotifyPlayerLost();
    }
    public override void Exit() {
        CurrentTarget = Context.transform;

        Context.OnPatrolChanged -= OnPatrolChanged;

        sequence.Dispose();
        sequence = null;
    }

    public override void Tick() {
        base.Tick();

        // If we reached the target, continue with our patrol.
        if (ReachedCurrentTarget()) { 
            CyclePatrol();
        }
    }

    void OnPatrolChanged() { 
        RestartSequence();
        CyclePatrol();
    }

    void RestartSequence() {
        sequence?.Dispose();
        sequence = Context.Patrol.GetEnumerator();
    }

    void CyclePatrol() {
        if (!sequence.MoveNext()) {
            RestartSequence();
            CyclePatrol();
        }
        else {
            CurrentTarget = sequence.Current;
        }
    }

    public override EnemyAI.EState NextState() {
        // TODO: Try detect player to change states
        if (Context.FOV.SeenAny) {
            return EnemyAI.EState.CHASE;
        }

        return Key;
    }
}
