using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings ;
  public class LocaleSelector : MonoBehaviour
{
    private bool active = false;

    public void ChangeLocale(int localeID)
    {
        if (active == true)
            return;

        StartCoroutine(SetLocale(localeID));
    }

    IEnumerator SetLocale(int _localeID)
    {
        active = true;
        yield return LocalizationSettings.InitializationOperation;

        // Make sure the localeID is within the range of available locales
        if (_localeID >= 0 && _localeID < LocalizationSettings.AvailableLocales.Locales.Count)
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_localeID];
        }
        else
        {
            Debug.LogError("Invalid locale ID: " + _localeID);
        }

        active = false;
    }
}
