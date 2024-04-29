using System;
using UnityEngine;

using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviourService<GameManager> {
    public const int MAIN_MENU_SCENEID = 1;
    public const int GAME_SCENEID = 2;

    // ======================= Events ========================

    // ==================== Configuration ====================

    // ====================== Variables ======================

    // ===================== Unity Stuff =====================
    void Start() {
        MenuManager.Init();
        SceneManager.LoadScene(MAIN_MENU_SCENEID);
    }

    // ===================== Custom Code =====================
    public static void CloseGame() {
        Application.Quit();
    }
}
