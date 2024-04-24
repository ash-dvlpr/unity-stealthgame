using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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

    [Header("Player")]
    [SerializeField] public PlayerInput player;

    // ====================== Variables ======================
    public HashSet<int> objectiveIds = new();
    int remainingObjectives = 0;
    
    [Header("Game State")]
    [Disabled, SerializeField] float remainingTime = 0f;
    GameState gameState;

    public int TotalObjectives { get => objectiveIds.Count; }
    public int RemainingObjectives { get => remainingObjectives; }

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
        RestartState();
    }

    void Update() {
        switch (gameState) {
            case GameState.NONE: {
                if (Input.GetKeyDown(KeyCode.Return)) {
                    gameState = GameState.GAME_STARTED;
                    player.enabled = true;
                }
                break;
            } 
            case GameState.GAME_ENDED: {
                if (Input.GetKeyDown(KeyCode.Return)) {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
                break;
            } 
            case GameState.GAME_STARTED: { 
                remainingTime -= Time.deltaTime;
                CheckWinCondition();
                break;
            } 
        }
    }
    // ===================== Custom Code =====================
    void Restore() {
        objectiveIds.Clear();
        remainingTime = Config.MaxTime;
    }

    void RestartState() {
        gameState = GameState.NONE;
        remainingObjectives = TotalObjectives;
        player.enabled = false;
    }

    void OnGameplayEvent(GameplayEvent e) {
        // Do event specific logic
        switch (e.data) {
            case EventMetadata.NONE: return;

            // Register Objectives
            case EventMetadata.SETUP: {
                objectiveIds.Add(e.id);
                break;
            }

            // Objective advanced
            case EventMetadata.OBJECTIVE: {
                if (objectiveIds.Contains(e.id)) {
                    // Decrease the number of remaining objectives
                    remainingObjectives--;
                    // Give back some time to the player
                    remainingTime += Config.TimePerObjective;
                }
                break;
            }

            default: break;
        }
    }

    void CheckWinCondition() {
        //Debug.Log($"{RemainingObjectives}/{TotalObjectives}");
        if (RemainingObjectives == 0) TriggerWin();
        else if (remainingTime <= float.Epsilon) TriggerDefeat();
    }

    void TriggerWin() {
        Debug.Log("VICTORY!");
        gameState = GameState.GAME_ENDED;
        player.enabled = false;

        // Trigger win fireworks
        EventBus.Raise<CinematicEvent>(new() { id = CinematicID.VICTORY });
    }

    void TriggerDefeat() {
        Debug.Log("DEFEAT!");
        gameState = GameState.GAME_ENDED;
        player.enabled = false;

        // TODO: Reload Scene
    }
}
