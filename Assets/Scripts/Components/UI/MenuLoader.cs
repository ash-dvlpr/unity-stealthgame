using System;
using UnityEngine;


public class MenuLoader : MonoBehaviour {
    private void Start() {
        MenuManager.OpenMenu(MenuID.Main);
    }
}