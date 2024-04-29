using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameOverMenu : AMenu {
    public override MenuID MenuKey { get => MenuID.GameOverUI; }

    // ====================== References =====================
    [SerializeField] GameObject victoryText;
    [SerializeField] GameObject gameOverText;

    // ===================== Custom Code =====================
    public override void OpenMenu() {
        base.UnlockCursor();
        base.OpenMenu();
    }

    public override void CloseMenu() {
        base.CloseMenu();
    }

    public void GameOverText(bool gameOver) {
        gameOverText?.SetActive(gameOver);
        victoryText?.SetActive(!gameOver);
    }

    // ===================== UI Actions ======================
    public void OnClick_Restart() {
        MenuManager.OpenMenu(MenuID.PlayerGUI);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void OnClick_MainMenu() {
        SceneManager.LoadScene(GameManager.MAIN_MENU_SCENEID);
    }
}
