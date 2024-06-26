using System;
using System.Linq;
using UnityEngine;

public class EnemyAIState_Chase : EnemyAIState {
    // ====================== Variables ======================
    public override EnemyAI.EState Key => EnemyAI.EState.CHASE;

    // ===================== Constructor =====================
    public EnemyAIState_Chase(EnemyAI context) : base(context) { }

    // ===================== Custom Code =====================
    public override void Enter() {
        base.Enter();
        if (Context.FOV.SeenAny) { 
            CurrentTarget = Context.FOV.VisibleTargets.First();
        }

        Context.NotifyPlayerDetected();
    }

    public override void Tick() {
        base.Tick();

        UpdateNavMeshAgent();
    }

    public override EnemyAI.EState NextState() {
        // If target was lost
        if (!Context.FOV.VisibleTargets.Contains(CurrentTarget)) {
            return EnemyAI.EState.PATROL;
        }

        // If the target enters the atack range
        else if (DistanceToTarget <= StateConfig.StoppingDistance) {
            return EnemyAI.EState.ATTACK;
        }

        return Key; 
    }
}
