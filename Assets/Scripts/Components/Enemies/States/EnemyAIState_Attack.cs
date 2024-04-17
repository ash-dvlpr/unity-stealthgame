using System;
using System.Collections;
using System.Collections.Generic;
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
    }

    public override void Tick() {
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
                Debug.Log($"Hit: {health.name}");
            }
        }
    }

    public override EnemyAI.EState NextState() {
        // If not on attack animation
        if (timer <= 0f) { 
            // TODO: If target dies, PATROL

            // Distance becomes too great, return to chasing
            if (DistanceToTarget > StateConfig.StoppingDistance) {
                return EnemyAI.EState.CHASE;
            }
        }

        return Key;
    }
}
