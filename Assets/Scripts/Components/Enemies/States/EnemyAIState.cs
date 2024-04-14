using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAIState : BaseState<EnemyAI.EState> {
    protected EnemyAIState(EnemyAI context) {
        this.Context = context;
    }

    // ====================== Variables ======================
    protected EnemyAI Context { get; private set; }
}
