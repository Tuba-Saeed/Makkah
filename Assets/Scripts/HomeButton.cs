using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class HomeButton : MonoBehaviour
{
    public void OnHomeButtonClick()
    {
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
            Debug.LogWarning("CurrentMainLevel is not set to a valid level.");
        }
    }
}
