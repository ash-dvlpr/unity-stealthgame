using System;
using UnityEngine;

using CinderUtils.Events;
using CinderUtils.Extensions;


[RequireComponent(typeof(Health))]
public class Player : MonoBehaviour {
    // ======================= Events ========================
    EventBinding<CollectibleEvent> collectibleEvents = new();

    // ====================== Variables ======================

    // ===================== Unity Stuff =====================
    void Awake() {
        collectibleEvents.OnEvent += OnPickup;
    }

    void OnEnable() {
        EventBus.Register(collectibleEvents);
    }

    void OnDisable() {
        EventBus.Deregister(collectibleEvents);
    }

    // ===================== Custom Code =====================
    void OnPickup(CollectibleEvent e) {
        // If the collectible is for 
        if (e.resourceType.Is<Resource>()) {
            if (TryGetComponent(e.resourceType, out var _resource)) { 
                var resource = _resource as Resource;
                resource.Add(e.amount);
            }
        }
    }
}