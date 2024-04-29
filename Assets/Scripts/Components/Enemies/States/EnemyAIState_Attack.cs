using System;
using UnityEngine;


public class EnemyAIState_Attack : EnemyAIState {
    // ====================== Variables ======================
    public override EnemyAI.EState Key => EnemyAI.EState.ATTACK;
    float timer = 0f;

    // ===================== Constructor =====================
    public EnemyAIState_Attack(EnemyAI context) : base(context) { }

    // ===================== Custom Code =====================
    public override void Enter() {
        base.Enter();

        ResetTimer();
        Context.Animator.SetBool("IsAttacking", true);

        Context.NotifyPlayerDetected();
    }

    public override void Exit() {
        Context.Animator.SetBool("IsAttacking", false);
    }

    public override void Tick() {
        base.Tick();

        UpdateTimer();
        DealAreaDamage();
    }

    void ResetTimer() {
        timer = Config.AttackDelay;
    }

    private void UpdateTimer() {
        timer -= Time.deltaTime;
    }

    void DealAreaDamage() {
        Collider[] hittedTargets = Physics.OverlapSphere(
            Context.transform.position, StateConfig.StoppingDistance, Context.FOV.TargetMask
        );

        foreach (Collider hit in hittedTargets) {
            if (hit.TryGetComponent<Health>(out var health)) {
                health.Damage(Config.AttackDPS);
            }
        }
    }

    public override EnemyAI.EState NextState() {
        // If not on attack animation
        if (timer <= 0f) {
            // Target dies, PATROL
            if (!Context.FOV.VisibleTargets.Contains(CurrentTarget)) {
                return EnemyAI.EState.PATROL;
            }

            // Distance becomes too great, return to chasing
            if (DistanceToTarget > StateConfig.StoppingDistance) {
                return EnemyAI.EState.CHASE;
            }
        }

        return Key;
    }
}
