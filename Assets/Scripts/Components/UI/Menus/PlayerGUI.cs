using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class PlayerGUI : AMenu {
    public override MenuID MenuKey { get => MenuID.PlayerGUI; }

    // ==================== Configuration ====================
    //[SerializeField] AudioClip gameTheme;

    // ====================== References =====================
    [field: SerializeField] public ResourceBar HPBar { get; private set; }

    //[field: SerializeField] public TMP_Text RoundDisplay { get; private set; }
    //[field: SerializeField] public TMP_Text RemainingEnemyCountDisplay { get; private set; }

    // ===================== Custom Code =====================

    public override void OpenMenu() {
        base.LockCursor();
        base.OpenMenu();

        //AudioManager.ResumeAudio();
        //AudioManager.PlayClip(gameTheme, false);

        // Update UI elements
        HPBar.Refresh();
    }
    public override void CloseMenu() {
        base.UnlockCursor();
        base.CloseMenu();
    }

    // ===================== UI Actions ======================

}
