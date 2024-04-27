using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

using CinderUtils.Events;
using CinderUtils.Attributes;
using CinderUtils.Extensions;




public class Gameplay : MonoBehaviour {
    public enum State : byte {
        NONE            = 0,
        GAME_STARTED    = 1,
        GAME_ENDED      = 2,
    }

    // ======================= Events ========================
    EventBinding<GameplayEvent> gameplayEvents = new();

    // ==================== Configuration ====================
    [field: SerializeField] GameplayConfig Config;

    [Header("Player")]
    [SerializeField] Player player;

    [Header("Collectibles")]
    [SerializeField] HealthPack healthPackPrefab;
    [SerializeField] List<Transform> spawnPositions = new();

    [Header("Enemies")]
    [SerializeField] List<EnemyAI> enemies = new();

    // ====================== Variables ======================
    [Header("Game State")]
    [Disabled, SerializeField] float remainingTime = 0f;
    State gameState;
    
    HashSet<int> objectiveIds = new();
    int remainingObjectives = 0;

    List<AIPatrol> enemyPatrols = new();
    int patrolOffset = 0;

    // Player stuff
    PlayerInput playerInput;
    // Player GUI
    PlayerGUI GUI;
    ResourceBar playerHPBar;
    

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
        playerHPBar?.SwapTrackedResource();
        StopAllCoroutines();
    }

    void Start() {
        RestartState();
    }

    void Update() {
        switch (gameState) {
            case State.NONE: {
                if (Input.GetKeyDown(KeyCode.Return)) {
                    gameState = State.GAME_STARTED;
                    playerInput.enabled = true;
                }
                break;
            }
            case State.GAME_ENDED: {
                if (Input.GetKeyDown(KeyCode.Return)) {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
                break;
            }
            case State.GAME_STARTED: {
                remainingTime -= Time.deltaTime;
                CheckWinCondition();
                break;
            }
        }
    }
    // ===================== Custom Code =====================
    void Restore() {
        objectiveIds.Clear();
        remainingTime = Config.SecondsMaxTime;
        InitEnemyPatrols();
    }

    void RestartState() {
        StopAllCoroutines();

        // Setup UI
        playerHPBar = (GUI = (PlayerGUI) MenuManager.Get(MenuID.PlayerGUI)).HPBar;
        playerHPBar.SwapTrackedResource(player.HP);

        // Show UI
        MenuManager.OpenMenu(MenuID.PlayerGUI);
        
        
        // Reload stuff
        playerInput = player.PlayerController.PlayerInput;
        gameState = State.NONE;
        remainingObjectives = TotalObjectives;
        playerInput.enabled = false;
        StartCoroutine(RotateEnemyPatrols());
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
                    remainingTime += Config.SecondsPerObjective;
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
        gameState = State.GAME_ENDED;
        playerInput.enabled = false;

        // Trigger win fireworks
        EventBus.Raise<CinematicEvent>(new() { id = CinematicID.VICTORY });
    }

    void TriggerDefeat() {
        StopAllCoroutines();

        Debug.Log("DEFEAT!");
        gameState = State.GAME_ENDED;
        playerInput.enabled = false;

        // TODO: Reload Scene
    }

    void SpawnHeathPack() {
        if (!healthPackPrefab || spawnPositions.NullOrEmpty()) return;

        var position = spawnPositions.GetRandom().position;
        Instantiate(healthPackPrefab.gameObject, position, Quaternion.identity);
    }

    void InitEnemyPatrols() {
        // Clear previous patrols
        enemyPatrols.Clear();

        // List all of the enemies's patrols
        foreach (var enemy in enemies) {
            enemyPatrols.Add(enemy.Patrol);
        }
    }

    IEnumerator RotateEnemyPatrols() {
        while (true) {
            // Increase the offset for the next time, and wait before changing again.
            yield return new WaitForSeconds(Config.SecondsPerPatrol);
            patrolOffset++;

            // Assign patrol to enemies
            for (int i = 0 ; i < enemies.Count ; i++) {
                var enemy = enemies[i];
                // Offset the index and loop (%)
                int _i = ( i + patrolOffset ) % enemyPatrols.Count;

                enemy.SwapPatrol(enemyPatrols[_i]);
            }
        }
    }
}
