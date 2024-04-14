using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
