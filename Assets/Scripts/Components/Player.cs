using System;
using System.Collections.Generic;
using UnityEngine;

using CinderUtils.Events;
using CinderUtils.Extensions;


public class Player : MonoBehaviour {
    // ======================= Events ========================
    readonly Dictionary<Type, Resource> resources = new();
    EventBinding<CollectibleEvent> collectibleEvents = new();

    // ====================== Variables ======================

    // ===================== Unity Stuff =====================
    void Awake() {
        GetComponents<Resource>().ForEach(r => resources.Add(r.GetType(), r));

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
        // Check if we have the resource needed to handle that event.
        if (resources.TryGetValue(e.ResourceType, out var _resource)) {
            _resource.Add(e.collectible.Amount);
        }
    }
}