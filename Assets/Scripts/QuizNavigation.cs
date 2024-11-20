using UnityEngine;
using UnityEngine.SceneManagement;

public class QuizNavigation : MonoBehaviour
{
    public void OpenQuizFromHajj()
    {
        GameData.CurrentMainLevel = "Hajj"; // Set current main level
        SceneManager.LoadScene("QuizScene"); // Load quiz scene
    }

    public void OpenQuizFromUmrah()
    {
        GameData.CurrentMainLevel = "Umrah"; // Set current main level
        SceneManager.LoadScene("QuizScene"); // Load quiz scene
    }
}
