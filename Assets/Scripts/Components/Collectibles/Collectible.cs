using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CinderUtils.Events;


[RequireComponent(typeof(Collider))]
public abstract class Collectible : EventRaiser {
    // ==================== Configuration ====================
    [Header("Collision")]
    [SerializeField] string targetTag = "Player";

    // ===================== Unity Stuff =====================
    void OnTriggerEnter(Collider other) {
        if (other.CompareTag(targetTag)) {
            Collect(other);
            Despawn();
        }
    }

    // ===================== Custom Code =====================
    protected abstract void Collect(Collider other);
    protected virtual void Despawn() { 
        Destroy(this.gameObject);
    }
}

// Resource Collectibles
public interface ICollectible<out R> where R : Resource {
    public Type ResourceType => typeof(R);
    public abstract float Amount { get; }
}

public abstract class Collectible<R> : Collectible, ICollectible<R> where R : Resource {
    // ====================== Variables ======================
    public Type ResourceType => typeof(R);
    public abstract float Amount { get; }

    //// ===================== Custom Code =====================
    protected override void Collect(Collider other) {
        EventBus.Raise<CollectibleEvent>(new() {
            collectible = this
        });
    }
}