using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CinderUtils.Events;


public class Coin : Collectible {

    // ===================== Unity Stuff =====================
    void OnEnable() {
        EventBus.Raise<GameplayEvent>(new() { id = ID, data = EventMetadata.SETUP });
    }

    // ===================== Custom Code =====================
    protected override void Collect(Collider other) {
        EventBus.Raise<GameplayEvent>(new() { id = ID, data = EventMetadata.OBJECTIVE });
    }
}
