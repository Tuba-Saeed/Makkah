using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Import for TextMeshPro compatibility

public class QuizManager : MonoBehaviour
{
    public List<QuestionAndAnswers> QnA;
    public GameObject[] options; // Array of answer buttons
    public int currentQuestion;
    public Text QuestionTxt; // For standard Unity Text
    public TextMeshProUGUI QuestionTMP; // Optional: For TMP Question Text

    private Color correctColor = Color.green; // Color for correct answer
    private Color wrongColor = Color.red;     // Color for wrong answer
    private Color defaultColor;               // Default button color

    private void Start()
    {
        if (options.Length > 0)
        {
            defaultColor = options[0].GetComponent<Image>().color; // Get default button color
        }

        if (QnA.Count > 0)
        {
            generateQuestion();
        }
        else
        {
            Debug.LogError("QnA list is empty. Please add questions to the list.");
        }
    }

    public void correct()
    {
        QnA.RemoveAt(currentQuestion);

        if (QnA.Count > 0)
        {
            generateQuestion();
        }
        else
        {
            Debug.Log("All questions answered.");
            // Optionally show a completion panel or handle the end of the quiz
        }
    }

    void SetAnswers()
    {
        for (int i = 0; i < options.Length; i++)
        {
            options[i].GetComponent<AnswerScript>().isCorrect = false;

            var answerText = options[i].GetComponentInChildren<Text>();
            var answerTMPText = options[i].GetComponentInChildren<TextMeshProUGUI>();

            if (answerText == null && answerTMPText == null)
            {
                Debug.LogError("Text or TextMeshProUGUI component missing in option: " + options[i].name);
                continue;
            }

            if (answerText != null)
            {
                answerText.text = QnA[currentQuestion].Answers[i];
            }
            else if (answerTMPText != null)
            {
                answerTMPText.text = QnA[currentQuestion].Answers[i];
            }

            if (QnA[currentQuestion].CorrectAnswer == i + 1)
            {
                options[i].GetComponent<AnswerScript>().isCorrect = true;
            }

            // Reset button color to default
            options[i].GetComponent<Image>().color = defaultColor;
            options[i].GetComponent<Button>().interactable = true; // Enable button
        }
    }

    void generateQuestion()
    {
        currentQuestion = Random.Range(0, QnA.Count);

        if (QuestionTxt != null)
        {
            QuestionTxt.text = QnA[currentQuestion].Question;
        }
        else if (QuestionTMP != null)
        {
            QuestionTMP.text = QnA[currentQuestion].Question;
        }
        else
        {
            Debug.LogError("No Question Text or TMP component assigned.");
        }

        SetAnswers();
    }

    public void AnswerButtonPressed(GameObject selectedOption)
    {
        AnswerScript answerScript = selectedOption.GetComponent<AnswerScript>();

        if (answerScript.isCorrect)
        {
            selectedOption.GetComponent<Image>().color = correctColor; // Set green for correct
            Debug.Log("Correct Answer!");
            ShowCorrectAnswer(); // Highlight correct answer
            StartCoroutine(NextQuestionAfterDelay(2f)); // Proceed after delay
        }
        else
        {
            selectedOption.GetComponent<Image>().color = wrongColor; // Set red for wrong
            Debug.Log("Wrong Answer!");
            ShowCorrectAnswer(); // Highlight correct answer
            StartCoroutine(NextQuestionAfterDelay(2f)); // Proceed after delay
        }

        // Disable all buttons after an answer is selected
        foreach (GameObject option in options)
        {
            option.GetComponent<Button>().interactable = false;
        }
    }

    void ShowCorrectAnswer()
    {
        // Highlight the correct answer button in green
        for (int i = 0; i < options.Length; i++)
        {
            if (options[i].GetComponent<AnswerScript>().isCorrect)
            {
                options[i].GetComponent<Image>().color = correctColor;
            }
        }
    }

    IEnumerator NextQuestionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Enable buttons and reset colors
        foreach (GameObject option in options)
        {
            option.GetComponent<Button>().interactable = true;
            option.GetComponent<Image>().color = defaultColor;
        }

        if (QnA.Count > 0)
        {
            generateQuestion();
        }
        else
        {
            Debug.Log("Quiz Completed!");
        }
    }
}
