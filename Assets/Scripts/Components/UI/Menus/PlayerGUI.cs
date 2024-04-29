using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class PlayerGUI : AMenu {
    public override MenuID MenuKey { get => MenuID.PlayerGUI; }

    // ==================== Configuration ====================
    [SerializeField] AudioClip gameTheme;
    [SerializeField] AudioClip pursuitTheme;

    // ====================== References =====================
    [field: SerializeField] public ResourceBar HPBar { get; private set; }
    [field: SerializeField] public ResourceBar TimeBar { get; private set; }
    [field: SerializeField] public TMP_Text RemainingObjectivesDisplay { get; private set; }

    // ===================== Custom Code =====================

    public override void OpenMenu() {
        base.LockCursor();
        base.OpenMenu();

        // Update UI elements
        HPBar.Refresh();
    }
    public override void CloseMenu() {
        base.UnlockCursor();
        base.CloseMenu();
    }

    public void UpdateBackgroundMusic(bool detected = false) { 
        AudioManager.PlayClip(detected ? pursuitTheme : gameTheme, false);
    }

    public void UpdateRemainingIbjectives(int remaining) {
        RemainingObjectivesDisplay.text = $"X {remaining}";
    }

}
