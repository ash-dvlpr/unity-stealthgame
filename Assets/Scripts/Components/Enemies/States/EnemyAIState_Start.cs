using System;
using UnityEngine;

public class EnemyAIState_Start : EnemyAIState {
    // ====================== Variables ======================
    public override EnemyAI.EState Key => EnemyAI.EState.START;

    // ===================== Constructor =====================
    public EnemyAIState_Start(EnemyAI context) : base(context) { }

    // ===================== Custom Code =====================
    public override EnemyAI.EState NextState() { 
        return EnemyAI.EState.PATROL; 
    }
}
