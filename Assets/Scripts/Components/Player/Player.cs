using System;
using System.Collections.Generic;
using UnityEngine;

using CinderUtils.Events;
using CinderUtils.Extensions;


[RequireComponent(typeof(PlayerController), typeof(Health))]
public class Player : MonoBehaviour {
    // ======================= Events ========================
    readonly Dictionary<Type, Resource> resources = new();
    EventBinding<CollectibleEvent> collectibleEvents = new();

    // ====================== Variables ======================
    public PlayerController PlayerController { get; private set; }
    public Animator Animator => PlayerController.Animator;
    public Health HP => (Health) resources[typeof(Health)];

    // ===================== Unity Stuff =====================
    void Awake() {
        GetComponents<Resource>().ForEach(r => resources.Add(r.GetType(), r));
        PlayerController = GetComponent<PlayerController>();

        collectibleEvents.OnEvent += OnPickup;
    }

    void OnEnable() {
        HP.OnChange += OnHit;
        EventBus.Register(collectibleEvents);
    }

    void OnDisable() {
        HP.OnChange -= OnHit;
        EventBus.Deregister(collectibleEvents);
    }

    // ===================== Custom Code =====================
    void OnPickup(CollectibleEvent e) {
        // Check if we have the resource needed to handle that event.
        if (resources.TryGetValue(e.ResourceType, out var _resource)) {
            _resource.Add(e.collectible.Amount);
        }
    }

    void OnHit(Resource health) {
        Animator.SetTrigger(AnimatorID.Hit);
    }
}