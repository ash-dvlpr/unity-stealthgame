using System;
using UnityEngine;

using CinderUtils.Events;


public class DayNightCycle : MonoBehaviour {
    const string EXTERNAL_TIME_SHADER_PROPERTY = "_ExternalTime";
    const string STARS_ALPHA_SHADER_PROPERTY = "_StarTransparency";

    // ======================= Events ========================
    //EventBinding<DaylightCycleEvent> gameplayEvents = new();

    // ==================== Configuration ====================
    [field: SerializeField] public GameplayConfig Config { get; private set; }

    [Header("Time Settings")]
    [SerializeField] float startingMinute = 1;
    [SerializeField] float timeSpeed = 1f;

    [Header("Lights")]
    [SerializeField] Light sunLight;
    [SerializeField] Light moonLight;

    [Header("Enviroment")]
    [SerializeField] Renderer _clouds;
    [SerializeField] Renderer _stars;
    [SerializeField] AnimationCurve starAlphaCurve;

#if UNITY_EDITOR
    [Header("Debug")]
    [SerializeField] bool _overrideTimer;
    [SerializeField, Range(0, 1)] float _timeOfDay;
#endif

    // ====================== Variables ======================
    float time = 0f;
    Material cloudsMaterial;
    Material starsMaterial;


    // ===================== Unity Stuff =====================
    private void Awake() {
        time = startingMinute * 60;
        sunLight.enabled = false;
        moonLight.enabled = false;

        cloudsMaterial = _clouds.material;
        starsMaterial = _stars.material;
    }

    void OnDestroy() {
        // Do some cleaunp
        Destroy(cloudsMaterial);
        Destroy(starsMaterial);
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
        var shaderTime = normalizedTimeOfDay * Config.SecondsPerDay;
        cloudsMaterial.SetFloat(EXTERNAL_TIME_SHADER_PROPERTY, shaderTime);
        starsMaterial.SetFloat(EXTERNAL_TIME_SHADER_PROPERTY, shaderTime);

        // Calculate stars transparency
        float starAlpha = starAlphaCurve.Evaluate(normalizedTimeOfDay);
        starsMaterial.SetFloat(STARS_ALPHA_SHADER_PROPERTY, starAlpha);
    }
}
