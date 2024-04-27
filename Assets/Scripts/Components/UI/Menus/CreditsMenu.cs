using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsMenu : AMenu {
    public override MenuID MenuKey => MenuID.Credits; 

    // ===================== Custom Code =====================

    // ===================== UI Actions ======================

    //? Normal actions
    public void OnClick_Back() {
        // Return to Main Menu
        MenuManager.OpenMenu(MenuID.Main);
    }
}
