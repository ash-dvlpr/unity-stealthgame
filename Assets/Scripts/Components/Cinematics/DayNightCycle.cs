using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CinderUtils.Events;


public class DayNightCycle : MonoBehaviour {
    // ======================= Events ========================
    //EventBinding<DaylightCycleEvent> gameplayEvents = new();

    // ==================== Configuration ====================
    [field: SerializeField] public GameplayConfig Config { get; private set; }
    [SerializeField] float timeSpeed = 1f;
    [SerializeField] Light sunLight;
    [SerializeField] Light moonLight;

    // ====================== Variables ======================
    float time = 0f;

    // ===================== Unity Stuff =====================
    private void Awake() {
        sunLight.enabled = false;
        moonLight.enabled = false;
    }

    void Update() {
        time += Time.deltaTime * timeSpeed;

        // Reset time
        if (time > Config.SecondsPerDay) time = 0f;

        // Percentage from 0.0f to 1.0f 
        float percentace = time / Config.SecondsPerDay;
        float degrees = 360 * percentace;

        // Update rotation
        this.transform.localEulerAngles = new Vector3(degrees, -90f, 0f);

        // Update Day/Night
        if (percentace >= 0.5f) {
            sunLight.enabled = false;
            moonLight.enabled = true;
            // Send DayNightEvent
        } else {
            sunLight.enabled = true;
            moonLight.enabled = false;
            // Send DayNightEvent
        }
    }
}
