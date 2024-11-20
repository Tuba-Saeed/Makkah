using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
  

    // Load Hajj level and update the GameData
    public void LoadHajjLevel()
    {
        GameData.CurrentMainLevel = "Hajj";
        SceneManager.LoadScene("HajjScene");
    }

    // Load Umrah level and update the GameData
    public void LoadUmrahLevel()
    {
        GameData.CurrentMainLevel = "Umrah";
        SceneManager.LoadScene("UmrahScene");
    }
}

