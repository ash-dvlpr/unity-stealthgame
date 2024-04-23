using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CinderUtils.Events;


public class DayNightCycle : MonoBehaviour {
    const string STARS_ALPHA_SHADER_PROPERTY = "_StarTransparency";

    // ======================= Events ========================
    //EventBinding<DaylightCycleEvent> gameplayEvents = new();

    // ==================== Configuration ====================
    [field: SerializeField] public GameplayConfig Config { get; private set; }
    [SerializeField] float startingMinute = 1;
    [SerializeField] float timeSpeed = 1f;
    
    [SerializeField] Light sunLight;
    [SerializeField] Light moonLight;

    [SerializeField] Material cloudsMaterial;
    [SerializeField] Material starsMaterial;
    [SerializeField] AnimationCurve starAlphaCurve;

#if UNITY_EDITOR
    [SerializeField] bool _overrideTimer;
    [SerializeField, Range(0, 1)] float _timeOfDay;
#endif

    // ====================== Variables ======================
    float time = 0f;

    // ===================== Unity Stuff =====================
    private void Awake() {
        time = startingMinute * 60;
        sunLight.enabled = false;
        moonLight.enabled = false;
    }

    void Update() {
        time += Time.deltaTime * timeSpeed;

        // Reset time
        if (time > Config.SecondsPerDay) time = 0f;

        // Percentage from 0.0f to 1.0f 
        float normalizedTimeOfDay = time / Config.SecondsPerDay;

        // Runtime Testing Stuff
#if UNITY_EDITOR
        if (_overrideTimer) normalizedTimeOfDay = _timeOfDay;
        else _timeOfDay = normalizedTimeOfDay;
#endif

        float degrees = 360 * normalizedTimeOfDay;

        // Update rotation
        this.transform.localEulerAngles = new Vector3(degrees, -90f, 0f);

        // Update Day/Night
        if (normalizedTimeOfDay >= 0.5f) {
            sunLight.enabled = false;
            moonLight.enabled = true;
            // Send DayNightEvent
        }
        else {
            sunLight.enabled = true;
            moonLight.enabled = false;
            // Send DayNightEvent
        }

        //? Update shaders
        cloudsMaterial.mainTextureOffset = new Vector2(.2f * degrees, .1f * degrees);

        // Calculate stars transparency
        float starAlpha = starAlphaCurve.Evaluate(normalizedTimeOfDay);
        starsMaterial.SetFloat(STARS_ALPHA_SHADER_PROPERTY, starAlpha);
    }
}