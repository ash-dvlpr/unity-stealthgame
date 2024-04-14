using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIState_Chase : EnemyAIState {
    // ====================== Variables ======================
    public override EnemyAI.EState Key => EnemyAI.EState.CHASE;

    // ===================== Constructor =====================
    public EnemyAIState_Chase(EnemyAI context) : base(context) { }

    // ===================== Custom Code =====================
    public override void Enter() { }
    public override void Exit() { }
    public override void Tick() { }
    public override void FixedTick() { }
    public override EnemyAI.EState NextState() { 
        return Key; 
    }
}
