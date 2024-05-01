using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : AMenu {
    public override MenuID MenuKey { get => MenuID.Main; }

    // ==================== Configuration ====================
    [SerializeField] AudioClip mainTheme;

    // ====================== References =====================

    // ===================== Custom Code =====================
    public override void OpenMenu() {
        base.OpenMenu();
        AudioManager.PlayClip(mainTheme, false);
    }
    public override void CloseMenu() {
        base.CloseMenu();
    }

    // ===================== UI Actions ======================
    public void OnClick_Play() {
        SceneManager.LoadScene(GameManager.GAME_SCENEID);
        MenuManager.OpenMenu(MenuID.PlayerGUI);
    }
    public void OnClick_CloseGame() {
        GameManager.CloseGame();
    }
}
