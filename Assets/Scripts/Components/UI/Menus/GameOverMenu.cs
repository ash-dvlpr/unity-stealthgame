using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameOverMenu : AMenu {
    public override MenuID MenuKey { get => MenuID.GameOverUI; }

    // ===================== Custom Code =====================
    public override void OpenMenu() {
        base.UnlockCursor();
        base.OpenMenu();
    }

    // ===================== UI Actions ======================
    public void OnClick_Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void OnClick_MainMenu() {
        SceneManager.LoadScene(GameManager.MAIN_MENU_SCENEID);
        MenuManager.OpenMenu(MenuID.Main);
    }
}
