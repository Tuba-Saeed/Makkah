using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class PanelController : MonoBehaviour
{
    public GameObject panel;
    public Text infoText;
    private Coroutine autoCloseCoroutine;

    public void ShowPanel(string message, Action onCloseCallback, float delay = 3f)
    {
        infoText.text = message;
        panel.SetActive(true);

        // Stop any existing auto-close coroutine to avoid conflicts
        if (autoCloseCoroutine != null)
            StopCoroutine(autoCloseCoroutine);

        // Start a new auto-close coroutine with a callback
        autoCloseCoroutine = StartCoroutine(AutoClosePanel(onCloseCallback, delay));
    }

    private IEnumerator AutoClosePanel(Action onCloseCallback, float delay)
    {
        yield return new WaitForSeconds(delay);
        onCloseCallback?.Invoke();
        ClosePanel();
    }

    public void ClosePanel()
    {
        panel.SetActive(false);

        // Stop auto-close coroutine if it's running
        if (autoCloseCoroutine != null)
        {
            StopCoroutine(autoCloseCoroutine);
            autoCloseCoroutine = null;
        }
    }
}
