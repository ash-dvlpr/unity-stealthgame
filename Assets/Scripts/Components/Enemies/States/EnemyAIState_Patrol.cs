using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIState_Patrol : EnemyAIState {
    // ====================== Variables ======================
    public override EnemyAI.EState Key => EnemyAI.EState.PATROL;

    // ===================== Constructor =====================
    public EnemyAIState_Patrol(EnemyAI context) : base(context) { }

    // ===================== Custom Code =====================
    public override void Enter() { }
    public override void Exit() { }
    public override void Tick() { }
    public override void FixedTick() { }
    public override EnemyAI.EState NextState() { 
        return Key; 
    }
}
