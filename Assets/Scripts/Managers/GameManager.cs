using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CinderUtils.Events;


public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }

    // ======================= Events ========================

    // ==================== Configuration ====================

    // ====================== Variables ======================

    // ===================== Unity Stuff =====================
    void Awake() {
        // Mantain a single Instance
        if (Instance && Instance != this) DestroyImmediate(this);
        // If there were no instance, initialize singleton
        else {
            DontDestroyOnLoad(this);
            Instance = this;
        }
    }

    void OnEnable() {
    }

    void OnDisable() {
    }

    void Start() {
    }

}
