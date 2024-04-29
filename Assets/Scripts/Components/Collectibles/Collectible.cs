using System;
using UnityEngine;

using CinderUtils.Events;


[RequireComponent(typeof(Collider))]
public abstract class Collectible : EventRaiser {
    // ==================== Configuration ====================
    [Header("Collision")]
    [SerializeField] string targetTag = "Player";

    [Header("Sounds")]
    [SerializeField] AudioClip collectionSound;

    // ===================== Unity Stuff =====================
    void OnTriggerEnter(Collider other) {
        if (other.CompareTag(targetTag)) {
            Collect(other);
            Despawn();
        }
    }

    // ===================== Custom Code =====================
    protected virtual void Collect(Collider other) {
        if (collectionSound) {
            AudioManager.PlayClipAt(collectionSound, transform.position);
        }
    }
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
        base.Collect(other);
    }
}