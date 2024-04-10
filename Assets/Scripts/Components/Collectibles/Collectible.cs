using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

using CinderUtils.Events;
using CinderUtils.Attributes;

[RequireComponent(typeof(Collider))]
public abstract class Collectible : MonoBehaviour {
    static int nextId = 0;

    // ==================== Configuration ====================
    [Header("Collision")]
    [SerializeField] string targetTag = "Player";

    // ====================== Variables ======================
    [Disabled][SerializeField] int id;
    public int ID { get => id; }

    // ===================== Unity Stuff =====================
    void Awake() {
        // Set the id of the collectible. Using an atomic operation to avoid issues.
        id = Interlocked.Increment(ref nextId);    
    }
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
