using System;
using UnityEngine;

using CinderUtils.Events;


public class Objective : Collectible {

    // ===================== Unity Stuff =====================
    void OnEnable() {
        EventBus.Raise<GameplayEvent>(new() { id = ID, data = EventMetadata.OBJECTIVE_SETUP });
    }

    // ===================== Custom Code =====================
    protected override void Collect(Collider other) {
        EventBus.Raise<GameplayEvent>(new() { id = ID, data = EventMetadata.OBJECTIVE_COMPLETED });
    }
}
