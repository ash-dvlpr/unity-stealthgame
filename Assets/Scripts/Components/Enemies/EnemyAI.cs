using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent), typeof(FieldOfView))]
public class EnemyAI : BaseStateMachine<EnemyAI.EState> {
    public enum EState : int {
        START   = 0,
        PATROL  = 1,
        CHASE   = 2,
        ATTACK  = 3,
    }

    // ==================== Configuration ====================
    [field: SerializeField] public AIPatrol Patrol { get; private set; }
    [field: SerializeField] public AIConfig Config { get; private set; }

    // ======================= Context =======================
    public Transform CurrentTarget { get; set; } = null;
    public NavMeshAgent Agent { get; private set; }
    public FieldOfView FOV { get; private set; }

    // ===================== Unity Stuff =====================
    protected override void Awake() {
        Agent = GetComponent<NavMeshAgent>();
        FOV = GetComponent<FieldOfView>();

        base.Awake();
    }

    // ===================== Custom Code =====================
    protected override void InitializeStates() {
        states[EState.START] = new EnemyAIState_Start(this);
        states[EState.PATROL] = new EnemyAIState_Patrol(this);
        states[EState.CHASE] = new EnemyAIState_Chase(this);
        states[EState.ATTACK] = new EnemyAIState_Attack(this);
        
        ActiveState = states[EState.START];
    }
}
