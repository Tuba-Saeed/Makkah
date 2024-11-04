using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButtonController : MonoBehaviour
{
    // Call this method to go back to the main menu or previous scene
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Mode"); // Replace with the name of your main menu scene
    }
}
