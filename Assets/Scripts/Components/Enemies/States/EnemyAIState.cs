using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAIState : BaseState<EnemyAI.EState> {
    protected EnemyAIState(EnemyAI context) {
        this.context = context;
    }

    // ====================== Variables ======================
    protected EnemyAI context;
}
