using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;


public class LocaleChanger : MonoBehaviour {
    // ==================== Configuration ====================
    [Header("Target Language")]
    [SerializeField] string _langCode = "en";

    // ====================== Variables ======================
    bool _disabled = false;

    // ===================== Custom Code =====================
    public void ChangeLocale() {
        if (_disabled) return;
        StartCoroutine(SetLocale(_langCode));
    }

    private IEnumerator SetLocale(string _langCode) {
        _disabled = true;

        // Wait for the localization service to initialize
        yield return LocalizationSettings.InitializationOperation;

        // Try to get the new locale
        var langCode = new LocaleIdentifier(_langCode);
        var newLocale = LocalizationSettings.AvailableLocales.GetLocale(langCode);

        // Check and change the locale if it exists
        if (newLocale) LocalizationSettings.SelectedLocale = newLocale;

        //TODO: Generate language selection menu using the locales
        //var locales = LocalizationSettings.AvailableLocales.Locales;

        _disabled = false;
    }

}
