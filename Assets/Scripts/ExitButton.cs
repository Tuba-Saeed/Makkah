using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButton : MonoBehaviour
{
    public void OnExitButtonClick()
    {
        // Check GameData.CurrentMainLevel and load the corresponding scene
        if (GameData.CurrentMainLevel == "Hajj")
        {
            SceneManager.LoadScene("HajjScene");
        }
        else if (GameData.CurrentMainLevel == "Umrah")
        {
            SceneManager.LoadScene("UmrahScene");
        }
        else
        {
            // If CurrentMainLevel is not set, load a default scene (Home or Main Menu)
            SceneManager.LoadScene("HomeScene");
            Debug.LogWarning("CurrentMainLevel is not set correctly.");
        }
    }
}
