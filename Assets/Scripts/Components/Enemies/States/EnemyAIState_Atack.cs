using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIState_Atack : EnemyAIState {
    // ====================== Variables ======================
    public override EnemyAI.EState Key => EnemyAI.EState.ATACK;

    // ===================== Constructor =====================
    public EnemyAIState_Atack(EnemyAI context) : base(context) { }

    // ===================== Custom Code =====================
    public override void Enter() { }
    public override void Exit() { }
    public override void Tick() { }
    public override void FixedTick() { }
    public override EnemyAI.EState NextState() { 
        return Key; 
    }
}
