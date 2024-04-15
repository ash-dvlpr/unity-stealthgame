using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIState_Attack : EnemyAIState {
    // ====================== Variables ======================
    public override EnemyAI.EState Key => EnemyAI.EState.ATTACK;

    // ===================== Constructor =====================
    public EnemyAIState_Attack(EnemyAI context) : base(context) { }

    // ===================== Custom Code =====================
    public override void Enter() { }
    public override void Exit() { }
    public override void Tick() { }
    public override void FixedTick() { }
    public override EnemyAI.EState NextState() { 
        return Key; 
    }
}
