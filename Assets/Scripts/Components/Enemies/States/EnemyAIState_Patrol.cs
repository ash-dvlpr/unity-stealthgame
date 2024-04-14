using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIState_Patrol : EnemyAIState {
    // ====================== Variables ======================
    public override EnemyAI.EState Key => EnemyAI.EState.PATROL;
    IEnumerator<Transform> sequence;

    // ===================== Constructor =====================
    public EnemyAIState_Patrol(EnemyAI context) : base(context) { }

    // ===================== Custom Code =====================
    public override void Enter() {
        RestartSequence();
        CyclePatrol();
    }
    public override void Exit() {
        Context.CurrentTarget = Context.transform;

        sequence.Dispose();
        sequence = null;
    }

    public override void Tick() {
        //? Go to the CurrentTarget if we are not there yet.
        // If we aren't waiting for the path to be calculated, we are moving
        if (!Context.Agent.pathPending) {
            // If we are within a "step" of our target, we may be almost done
            if (Context.Agent.remainingDistance <= Context.Agent.stoppingDistance) {
                // We are done if: there's no path to the exact position of our destination, or we're stopped.
                if (!Context.Agent.hasPath || Context.Agent.velocity.sqrMagnitude <= float.Epsilon) {
                    CyclePatrol();
                }
            }
        }
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
            Context.CurrentTarget = sequence.Current;
            Context.Agent.SetDestination(Context.CurrentTarget.position);
        }
    }

    public override EnemyAI.EState NextState() {
        // TODO: Try detect player to change states

        return Key;
    }
}
