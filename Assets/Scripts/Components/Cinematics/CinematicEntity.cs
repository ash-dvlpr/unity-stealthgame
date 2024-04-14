using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CinderUtils.Events;


public abstract class CinematicEntity : EventRaiser {
    // ======================= Events ========================
    EventBinding<CinematicEvent> gameplayEvents = new();

    // ===================== Unity Stuff =====================
    protected override void Awake() {
        base.Awake();

        Debug.Log("CinematicEntity.Awake()");
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
