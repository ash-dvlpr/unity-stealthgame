using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyAIState : BaseState<EnemyAI.EState> {
    protected EnemyAIState(EnemyAI context) {
        this.Context = context;
    }

    // ====================== Variables ======================
    protected EnemyAI Context { get; set; }
    protected Transform CurrentTarget {
        get => Context.CurrentTarget;
        set {
            Context.CurrentTarget = value;
            UpdateNavMeshAgent();
        }
    }

    // ===================== Custom Code =====================
    protected bool ReachedCurrentTarget() { 
        // If we aren't waiting for the path to be calculated, we are moving
        if (!Context.Agent.pathPending) {
            // If we are within a "step" of our target, we may be almost done
            if (Context.Agent.remainingDistance <= Context.Agent.stoppingDistance) {
                // We are done if: there's no path to the exact position of our destination, or we're stopped.
                if (!Context.Agent.hasPath || Context.Agent.velocity.sqrMagnitude <= float.Epsilon) {
                    return true;
                }
            }
        }

        // Otherwise no
        return false;
    }

    protected void UpdateNavMeshAgent() { 
        Context.Agent.SetDestination(Context.CurrentTarget.position);
    }
}
