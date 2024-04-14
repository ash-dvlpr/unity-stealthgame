using System.Threading;
using UnityEngine;

using CinderUtils.Attributes;


public abstract class EventRaiser : MonoBehaviour {
    static int nextId = 0;

    // ====================== Variables ======================
    [Header("EventRaiser data")]
    [Disabled][SerializeField] int id;
    public int ID { get => id; }

    // ===================== Unity Stuff =====================
    protected virtual void Awake() {
        // Set the id of the collectible. Using an atomic operation to avoid issues.
        id = Interlocked.Increment(ref nextId);    
    }
}
