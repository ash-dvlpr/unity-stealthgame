using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

using CinderUtils.Events;
using CinderUtils.Attributes;
using CinderUtils.Extensions;


public enum GameState : byte {
    NONE            = 0,
    GAME_STARTED    = 1,
    GAME_ENDED      = 2,
}

public class Gameplay : MonoBehaviour {
    // ======================= Events ========================
    EventBinding<GameplayEvent> gameplayEvents = new();

    // ==================== Configuration ====================
    [field: SerializeField] GameplayConfig Config;

    [Header("Player")]
    [SerializeField] PlayerInput player;

    [Header("Collectibles")]
    [SerializeField] HealthPack healthPackPrefab;
    [SerializeField] List<Transform> spawnPositions = new();

    // ====================== Variables ======================
    HashSet<int> objectiveIds = new();
    int remainingObjectives = 0;
    
    [Header("Game State")]
    [Disabled, SerializeField] float remainingTime = 0f;
    GameState gameState;

    int TotalObjectives { get => objectiveIds.Count; }
    int RemainingObjectives { get => remainingObjectives; }

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

            // Game setup. Register Objectives.
            case EventMetadata.OBJECTIVE_SETUP: {
                objectiveIds.Add(e.id);
                break;
            }

            // Progress was made towards the objective
            case EventMetadata.OBJECTIVE_COMPLETED: {
                // Verify the objective
                if (objectiveIds.Contains(e.id)) {
                    // Decrease the number of remaining objectives
                    remainingObjectives--;
                    // Give back some time to the player
                    remainingTime += Config.TimePerObjective;
                    // Spawn a HealthPack
                    SpawnHeathPack();
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

    void SpawnHeathPack() {
        if (!healthPackPrefab || spawnPositions.NullOrEmpty()) return;

        var position = spawnPositions.GetRandom().position;
        Instantiate(healthPackPrefab.gameObject, position, Quaternion.identity);
    }
}
