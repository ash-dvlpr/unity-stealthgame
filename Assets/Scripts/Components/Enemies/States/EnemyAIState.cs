using System;
using UnityEngine;

public abstract class EnemyAIState : BaseState<EnemyAI.EState> {
    protected EnemyAIState(EnemyAI context) {
        this.Context = context;
    }

    // ====================== Variables ======================
    protected EnemyAI Context { get; set; }
    protected AIConfig.StateConfig StateConfig => Context.Config[Key];
    protected AIConfig Config => Context.Config;
    protected Transform CurrentTarget {
        get => Context.CurrentTarget;
        set {
            Context.CurrentTarget = value;
            UpdateNavMeshAgent();
        }
    }
    protected float DistanceToTarget => Vector3.Distance(Context.transform.position, CurrentTarget.position);

    // ===================== Custom Code =====================
    public override void Enter() {
        ApplyConfig();
    }
    public override void Tick() {
        UpdateAnimator();
    }

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

    protected void UpdateAnimator() {
        Context.Animator.SetFloat("Speed", Context.Agent.velocity.magnitude);
    }

    protected void ApplyConfig() {
        if (StateConfig.State != 0) {
            // Navigation
            Context.Agent.speed = StateConfig.WalkSpeed;
            Context.Agent.stoppingDistance = StateConfig.StoppingDistance;

            // Sensing
            Context.FOV.Range = StateConfig.ViewRange;
            Context.FOV.ViewAngle = StateConfig.ViewAngle;
            Context.FOV.DetectionMode = StateConfig.DetectionMode;
        }
    }
}
