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



[RequireComponent(typeof(RemainingTime))]
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
    RemainingTime remainingTime;
    State gameState;

    HashSet<int> objectiveIds = new();
    int _remainingObjectives = 0;
    int _detectionCount = 0;

    List<AIPatrol> enemyPatrols = new();
    int patrolOffset = 0;

    // Player stuff
    PlayerInput playerInput;
    // Player GUI
    PlayerGUI GUI;
    GameOverMenu gameOverUI;
    ResourceBar playerHPBar;
    ResourceBar remainingTimeBar;

    int TotalObjectives { get => objectiveIds.Count; }
    int RemainingObjectives { 
        get => _remainingObjectives;
        set {
            GUI.UpdateRemainingIbjectives(value);
            _remainingObjectives = value;
        }
    }
    int DetectionCount { 
        get => _detectionCount;
        set {
            if (gameState != State.GAME_ENDED) GUI.UpdateBackgroundMusic(value != 0);
            _detectionCount = value;
        }
    }

    // ===================== Unity Stuff =====================
    void Awake() {
        gameplayEvents.OnEvent += OnGameplayEvent;
        remainingTime = GetComponent<RemainingTime>();

        Restore();
    }

    void OnEnable() {
        EventBus.Register(gameplayEvents);
    }

    void OnDisable() {
        EventBus.Deregister(gameplayEvents);
        playerHPBar?.SwapTrackedResource();
        remainingTimeBar?.SwapTrackedResource();
        StopAllCoroutines();
    }

    void Start() {
        RestartState();
    }

    void Update() {
        if (gameState == State.GAME_STARTED) {
            remainingTime.Tick();
            CheckWinCondition();
        }
    }

    // ===================== Custom Code =====================
    void Restore() {
        objectiveIds.Clear();
        remainingTime.SetMax(Config.SecondsMaxTime);
        InitEnemyPatrols();
    }

    void RestartState() {
        StopAllCoroutines();

        // Setup UI
        GUI = (PlayerGUI) MenuManager.Get(MenuID.PlayerGUI);
        gameOverUI = (GameOverMenu) MenuManager.Get(MenuID.GameOverUI);
        
        playerHPBar = GUI.HPBar;
        remainingTimeBar = GUI.TimeBar;

        playerHPBar.SwapTrackedResource(player.HP);
        remainingTimeBar.SwapTrackedResource(remainingTime);

        // Setup Events
        player.HP.OnDeath += TriggerDefeat;
        remainingTime.OnTimesUp += TriggerDefeat;

        // Show UI
        MenuManager.OpenMenu(MenuID.PlayerGUI);

        // Reload stuff
        playerInput = player.PlayerController.PlayerInput;
        gameState = State.NONE;
        RemainingObjectives = TotalObjectives;
        DetectionCount = 0;

        // Start stuff
        gameState = State.GAME_STARTED;
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
                    RemainingObjectives--;
                    // Give back some time to the player
                    remainingTime.Add(Config.SecondsPerObjective);
                    // Spawn a HealthPack
                    SpawnHeathPack();
                }
                break;
            }

            // Track how many enemies have detected the player
            case EventMetadata.PLAYER_DETECTED: {
                DetectionCount++;
                break;
            }
            case EventMetadata.PLAYER_LOST: {
                DetectionCount--;
                break;
            }

            default: break;
        }
    }

    void CheckWinCondition() {
        if (RemainingObjectives == 0) TriggerWin();
    }

    void TriggerWin() {
        gameState = State.GAME_ENDED;
        playerInput.enabled = false;

        // Trigger win fireworks
        EventBus.Raise<CinematicEvent>(new() { id = CinematicID.VICTORY });

        gameOverUI.GameOverText(true);
        MenuManager.OpenMenu(MenuID.GameOverUI);
        gameOverUI.UpdateBackgroundMusic(true);
    }

    void TriggerDefeat() {
        gameState = State.GAME_ENDED;
        playerInput.enabled = false;

        gameOverUI.GameOverText(false);
        MenuManager.OpenMenu(MenuID.GameOverUI);
        gameOverUI.UpdateBackgroundMusic(false, remainingTime.TimesUp);
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
