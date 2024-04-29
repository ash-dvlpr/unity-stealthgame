using System;
using UnityEngine;
using UnityEngine.AI;

using CinderUtils.Events;


[RequireComponent(typeof(NavMeshAgent), typeof(FieldOfView))]
public class EnemyAI : BaseStateMachine<EnemyAI.EState> {
    public enum EState : int {
        START   = 0, // default
        PATROL  = 1,
        CHASE   = 2,
        ATTACK  = 3,
    }

    // ==================== Configuration ====================
    [field: SerializeField] public AIPatrol Patrol { get; private set; }
    [field: SerializeField] public AIConfig Config { get; private set; }

    // ======================= Context =======================
    public Transform CurrentTarget { get; set; } = null;
    public Animator Animator { get; private set; }
    public NavMeshAgent Agent { get; private set; }
    public FieldOfView FOV { get; private set; }

    // ====================== Variables ======================
    public bool _notified;

    // ===================== Unity Stuff =====================
    protected override void Awake() {
        Agent = GetComponent<NavMeshAgent>();
        FOV = GetComponent<FieldOfView>();
        Animator = GetComponentInChildren<Animator>();

        FOV.filter = FOVFilter;
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

    public event Action OnPatrolChanged;
    public void SwapPatrol(AIPatrol newPatrol) { 
        Patrol = newPatrol;
        OnPatrolChanged?.Invoke();
    }

    public void NotifyPlayerDetected() {
        if (!_notified) {
            _notified = true;
            EventBus.Raise<GameplayEvent>(new() { data = EventMetadata.PLAYER_DETECTED });    
        }
    }
    public void NotifyPlayerLost() { 
        if (_notified) { 
            _notified = false;
            EventBus.Raise<GameplayEvent>(new() { data = EventMetadata.PLAYER_LOST });    
        }
    }

    public bool FOVFilter(GameObject go) {
        return go.TryGetComponent<Health>(out var hp) && hp.IsAlive;
    }
}
