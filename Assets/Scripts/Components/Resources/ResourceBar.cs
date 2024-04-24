using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Slider))]
public class ResourceBar : MonoBehaviour {

    // ====================== References =====================
    [Header("Customization")]
    [SerializeField] Color borderColor;
    [SerializeField] Color backgroundColor;
    [SerializeField] Color barColor;

    [Header("Configuration")]
    [SerializeField] Resource trackedResource;
    [SerializeField] bool showValues;

    [Header("References")]
    [SerializeField] Image border;
    [SerializeField] Image background;
    [SerializeField] Image bar;
    [SerializeField] TMP_Text displayText;
    Slider barSlider;

    // ====================== Unity Code ======================
#if UNITY_EDITOR
    void OnValidate() {
        ReloadBar();
    }
#endif

    void Awake() {
        barSlider = GetComponent<Slider>();
        ReloadBar();
    }

    void OnEnable() {
        if (trackedResource) trackedResource.OnChange += OnResourceChanged;
    }

    void OnDisable() {
        if (trackedResource) trackedResource.OnChange -= OnResourceChanged;
    }

    // ===================== Custom Code =====================
    void OnResourceChanged(Resource resource) => UpdateGUI();
    void ReloadBar() {
        if (displayText) displayText.enabled = showValues;

        border.color = borderColor;
        background.color = backgroundColor;
        bar.color = barColor;

        UpdateGUI();
    }

    void UpdateGUI() {
        if (barSlider && trackedResource) { 
            var percent = trackedResource.Amount / (float) trackedResource.Max;
            barSlider.value = percent;
            if (showValues && displayText.enabled) {
                displayText.text = trackedResource.ValuesString;
            }
        }
    }

    // ================== Outside Facing API ==================
    public void Refresh() => UpdateGUI();

    public void SwapTrackedResource(Resource newResource) {
        if (trackedResource) trackedResource.OnChange -= OnResourceChanged;
        trackedResource = newResource;
        trackedResource.OnChange += OnResourceChanged;
        ReloadBar();
    }
}
