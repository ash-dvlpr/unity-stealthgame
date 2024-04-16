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

        if (timer <= 0f) {
            DealAreaDamage();
            ResetTimer();
        }
    }

    void ResetTimer() {
        timer = Config.AttackDelay;
    }

    private void UpdateTimer() {
        timer -= Time.deltaTime;
        Debug.Log(timer);
    }

    void DealAreaDamage() {
        Collider[] hittedTargets = Physics.OverlapSphere(
            Context.transform.position, StateConfig.StoppingDistance, Context.FOV.TargetMask
        );

        foreach (Collider hit in hittedTargets) {
            Debug.Log($"Hit: {hit.name}");
        }
    }

    public override EnemyAI.EState NextState() {
        // TODO: If target dies, PATROL

        // Distance becomes too great, return to chasing
        if (DistanceToTarget > StateConfig.StoppingDistance) {
            return EnemyAI.EState.CHASE;
        }

        return Key;
    }
}
