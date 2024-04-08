using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Collectible {
    protected override void Collect(Collider other) {
        Debug.Log("Coin Collected");
        /** TODO: Send a GameEvent 
         * Handle event on the GameManager to decrease the coin count.
         */
    }

    void OnEnable() {
        // TODO: Increase global coint count.
    }
}
