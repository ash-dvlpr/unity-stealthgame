using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider))]
public abstract class Collectible : MonoBehaviour {
    [SerializeField] string targetTag = "Player";


    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag(targetTag)) {
            Collect(other);
            Destroy(this.gameObject);
        }
    }

    protected abstract void Collect(Collider other);
}
