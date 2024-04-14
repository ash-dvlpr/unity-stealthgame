using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : BaseStateMachine<EnemyAI.EState> {
    public enum EState : int {
        START   = 0,
        PATROL  = 1,
        CHASE   = 2,
        ATACK   = 3,
    }

    // ==================== Configuration ====================
    [field: SerializeField] public AIPatrol Patrol { get; private set; }
    [field: SerializeField] public NavigationConfig Config { get; private set; }

    // ======================= Context =======================
    [field: SerializeField] public Transform CurrentTarget { get; set; } = null;
    [field: SerializeField] public NavMeshAgent Agent { get; private set; }

    // ===================== Unity Stuff =====================
    protected override void Awake() {
        Agent = GetComponent<NavMeshAgent>();

        base.Awake();
    }

    // ===================== Custom Code =====================
    protected override void InitializeStates() {
        states[EState.START] = new EnemyAIState_Start(this);
        states[EState.PATROL] = new EnemyAIState_Patrol(this);
        states[EState.CHASE] = new EnemyAIState_Chase(this);
        states[EState.ATACK] = new EnemyAIState_Atack(this);
        
        ActiveState = states[EState.START];
    }
}
