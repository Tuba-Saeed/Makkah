using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    // Called when Hajj button is pressed
    public void OnHajjButtonPressed()
    {
        // Load the Hajj scene
        SceneManager.LoadScene("HajjScene");
    }

    // Called when Umrah button is pressed
    public void OnUmrahButtonPressed()
    {
        // Load the Umrah scene
        SceneManager.LoadScene("UmrahScene");
    }
}
