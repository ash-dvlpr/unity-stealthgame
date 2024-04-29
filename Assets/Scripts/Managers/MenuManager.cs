using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

using CinderUtils.Events;


public enum MenuID : int {
    None       = 0,
    Main       = 1,
    //Settings   = 2,
    //Credits    = 3,
    //Pause      = 4,
    PlayerGUI  = 5,
    GameOverUI = 7,
    //Crossfade  = 8,
}


[RequireComponent(typeof(Canvas))]
public class MenuManager : MonoBehaviourService<MenuManager> {

    // ====================== Variables ======================
    public static MenuID CurrentMenu { get; private set; } = MenuID.None;

    private Dictionary<MenuID, AMenu> menuChache;

    // ===================== Custom Code =====================
    public static void Init() {
        if (!Initialized) {
            Instance.menuChache = new Dictionary<MenuID, AMenu>();
            Instance.gameObject.SetActive(true);

            // Cache all menu objects
            foreach (Transform child in Instance.gameObject.transform) {
                var menu = child.GetComponent<AMenu>();
                if (!menu || MenuID.None == menu.MenuKey) continue;

                Instance.menuChache[menu.MenuKey] = menu;
            }

            Initialized = true;
        }
    }

    public static void Cleanup() {
        Initialized = false;
    }

    // ================== Outside Facing API ==================
    public static AMenu Get(MenuID id) {
        Instance.menuChache.TryGetValue(id, out var menu);
        return menu;
    }

    public static void ResetSelectedUIObject(GameObject newSelected = null) {
        EventSystem.current?.SetSelectedGameObject(newSelected);
    }

    /// <summary>
    /// Opens up a menu based on it's identifier and closes the previouslly open menu.
    /// After opening a menu it caches it.
    /// </summary>
    /// <param name="menu"><see cref="MenuID">MenuID</see> of the menu that you want to open.</param>
    /// <param name="ignoreOldMenu">If set to true, will just show the menu without closing other menus or setting it as the last open menu.</param>
    public static void OpenMenu(MenuID menu, bool ignoreOldMenu = false) {
        if (!Initialized) Init();
        var previous = CurrentMenu;
        if (!ignoreOldMenu) {
            CurrentMenu = menu;
        }

        try {
            // Hide old Menu
            if (!ignoreOldMenu && Instance.menuChache.TryGetValue(previous, out var currentMenu)) {
                currentMenu.CloseMenu();
            }

            // TODO: Menu transitions

            // Show new Menu
            if (Instance.menuChache.TryGetValue(menu, out var newMenu)) {
                newMenu.OpenMenu();
            }
        }
        catch (KeyNotFoundException ke) {
            Debug.LogError($"MenuManger.OpenMenu({menu}, {previous})");
            Debug.LogException(ke, Instance.gameObject);
        }
    }

    /// <summary>
    /// Closes up the currently <see cref="CurrentMenu">opened menu</see>.
    /// Will only alter the <see cref="CurrentMenu">cached menu</see> only if you don't specify a menuID.
    /// </summary>
    /// <param name="menuID"><see cref="MenuID">MenuID</see> of the menu that you want to close.</param>
    public static void CloseMenu(MenuID menuID = MenuID.None) {
        if (!Initialized) Init();
        var targetMenu = MenuID.None == menuID ? CurrentMenu : menuID;


        try {
            if (Instance.menuChache.TryGetValue(targetMenu, out var menu)) {
                if (MenuID.None == menuID) {
                    CurrentMenu = MenuID.None;
                }
                menu.CloseMenu();
            }
        }
        catch (KeyNotFoundException ke) {
            Debug.LogError($"MenuManger.CloseMenu(): {CurrentMenu} not found.");
            Debug.LogException(ke, Instance.gameObject);
        }
    }
}
