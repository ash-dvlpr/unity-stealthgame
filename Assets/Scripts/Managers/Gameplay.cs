using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

using CinderUtils.Events;
using CinderUtils.Attributes;


public enum GameState : byte {
    NONE            = 0,
    GAME_STARTED    = 1,
    GAME_ENDED      = 2,
}

public class Gameplay : MonoBehaviour {
    // ======================= Events ========================
    EventBinding<GameplayEvent> gameplayEvents = new();

    // ==================== Configuration ====================
    [field: SerializeField] public GameplayConfig Config { get; private set; }

    // ====================== Variables ======================
    public HashSet<int> objectiveIds = new();
    int remainingObjectives = 0;
    [Disabled, SerializeField] float remainingTime = 0f;
    GameState gameState;

    public int TotalObjectives { get => objectiveIds.Count; }
    public int RemainingObjectives { get => remainingObjectives; }
    public bool TimerStarted { get => gameState != GameState.NONE; }

    // ===================== Unity Stuff =====================
    void Awake() {
        gameplayEvents.OnEvent += OnGameplayEvent;
        Restore();
    }

    void OnEnable() {
        EventBus.Register(gameplayEvents);
    }

    void OnDisable() {
        EventBus.Deregister(gameplayEvents);
    }

    void Start() {
        Restart();
    }

    void Update() {
        if (!TimerStarted) {
            if (Input.anyKeyDown) gameState = GameState.GAME_STARTED;
        }
        else if (gameState == GameState.GAME_STARTED) {
            remainingTime -= Time.deltaTime;
            CheckWinCondition();
        }
    }
    // ===================== Custom Code =====================
    void Restore() {
        objectiveIds.Clear();
        remainingTime = Config.MaxTime;
    }

    void Restart() {
        gameState = GameState.NONE;
        remainingObjectives = TotalObjectives;
    }

    void OnGameplayEvent(GameplayEvent e) {
        if (e.kind == EventKind.NONE) return;

        // Do event specific logic
        switch (e.kind) {
            // Register Objectives
            case EventKind.SETUP: {
                objectiveIds.Add(e.id);
                break;
            }

            case EventKind.OBJECTIVE: {
                if (objectiveIds.Contains(e.id)) {
                    remainingObjectives--;
                }
                break;
            }

            default: break;
        }
    }

    void CheckWinCondition() {
        if (RemainingObjectives == 0) TriggerWin();
        else if (remainingTime <= float.Epsilon) TriggerDefeat();
    }

    void TriggerWin() {
        gameState = GameState.GAME_ENDED;
        Debug.Log("WIN!");
    }

    void TriggerDefeat() {
        gameState = GameState.GAME_ENDED;
        Debug.Log("DEFEAT!");

        // TODO: Reload Scene
    }
}
