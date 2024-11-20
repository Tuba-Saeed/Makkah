using UnityEngine;

public class AnswerScript : MonoBehaviour
{
    public bool isCorrect = false; // Set by QuizManager for each button

    public void Answer()
    {
        FindObjectOfType<QuizManager>().AnswerButtonPressed(gameObject);
    }
}
