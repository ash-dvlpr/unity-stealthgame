using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIState_Start : EnemyAIState {
    // ====================== Variables ======================
    public override EnemyAI.EState Key => EnemyAI.EState.START;

    // ===================== Constructor =====================
    public EnemyAIState_Start(EnemyAI context) : base(context) { }

    // ===================== Custom Code =====================
    public override void Enter() { }
    public override void Exit() { }
    public override void Tick() { }
    public override void FixedTick() { }
    public override EnemyAI.EState NextState() { 
        return Key; 
    }
}
