using System;
using UnityEngine;

using CinderUtils.Events;


public abstract class CinematicEntity : MonoBehaviour {
    // ======================= Events ========================
    EventBinding<CinematicEvent> gameplayEvents = new();

    // ===================== Unity Stuff =====================
    protected virtual void Awake() {
        gameplayEvents.OnEvent += OnCinematicEvent;
    }

    void OnEnable() {
        EventBus.Register(gameplayEvents);
    }

    void OnDisable() {
        EventBus.Deregister(gameplayEvents);
    }

    // ===================== Custom Code =====================
    protected abstract void OnCinematicEvent(CinematicEvent e);
    
}
