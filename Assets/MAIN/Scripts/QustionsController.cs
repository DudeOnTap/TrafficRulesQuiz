using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QustionsController : MonoBehaviour
{
    [SerializeField] private GameObject questionPanel;
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private Raycaster raycaster;

    [Header("Вопросы")]
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private Image[] answersBTN;
    [SerializeField] private AnswerButton[] buttons;
    [SerializeField] private TextMeshProUGUI answers0Text;
    [SerializeField] private TextMeshProUGUI answers1Text;
    [SerializeField] private TextMeshProUGUI answers2Text;
    [SerializeField] private TextMeshProUGUI answers3Text;
    [SerializeField] private TextMeshProUGUI questionCount;
    [SerializeField] private Color defaultColor;

    [Header("Звуки")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip rightSound;
    [SerializeField] private AudioClip wrongSound;
    [SerializeField] private AudioClip startQuizSound;

    [Header("Результат")]
    [SerializeField] private TextMeshProUGUI resultText;

    public Question[] questions;

    private int currentQuestion = 0;
    private int maxQuestion;
    private int rightAnswers = 0;
    private int attempts;

    private bool isFirst = true;

    public static QustionsController Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }


    private void Start()
    {
        maxQuestion = questions.Length;

        if (isFirst)
            StartCoroutine(FirstQuizStart());
        else
            ShowQuestion();
    }

    private IEnumerator FirstQuizStart()
    {
        isFirst = false;

        yield return new WaitForSeconds(3);

        audioSource.PlayOneShot(startQuizSound);

        yield return new WaitForSeconds(4);

        for (int i = 0; i < 4; i++)
        {
            buttons[i].active = true;
        }

        ShowQuestion();
    }

    public void GetAnswer(int btnId)
    {
        StopAllCoroutines();

        if (btnId == questions[currentQuestion].rightAnswerId)
        {
            rightAnswers++;

            audioSource.PlayOneShot(rightSound);

            StartCoroutine(ShowRightAnswer(false, btnId));
        }
        else
        {
            audioSource.PlayOneShot(wrongSound);

            StartCoroutine(ShowRightAnswer(true, btnId));
        }
        
    }

    private IEnumerator ShowRightAnswer(bool needShow, int currentAnswer)
    {
        int rightAnsw = questions[currentQuestion].rightAnswerId;

        if (needShow)
        {
            for (int i = 0; i < 4; i++)
            {
                buttons[i].active = false;

                if (currentAnswer == i)
                    answersBTN[i].color = Color.red;
            }
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                buttons[i].active = false;

                if (rightAnsw == i)
                    answersBTN[i].color = Color.green;
            }
        }

        yield return new WaitForSeconds(3f);

        for (int i = 0; i < 4; i++)
        {
            answersBTN[i].color = defaultColor;

            buttons[i].active = true;
        }

        if (needShow)
        {
            ShowQuestion();
        }
        else
        {
            if (currentQuestion < maxQuestion - 1)
            {
                currentQuestion++;

                ShowQuestion();
            }
            else
                ShowResults();
        }
    }

    private IEnumerator QuestionTimer()
    {
        yield return new WaitForSeconds(30f);

        GetAnswer(-1);
    }

    private void ShowQuestion()
    {
        attempts++;

        questionCount.text = (currentQuestion + 1) + " из " + maxQuestion;

        questionText.text = questions[currentQuestion].question;

        answers0Text.text = questions[currentQuestion].answer0;
        answers1Text.text = questions[currentQuestion].answer1;
        answers2Text.text = questions[currentQuestion].answer2;
        answers3Text.text = questions[currentQuestion].answer3;

        audioSource.PlayOneShot(questions[currentQuestion].questionSound);

        StartCoroutine(QuestionTimer());
    }

    private void ShowResults()
    {
        questionPanel.SetActive(false);
        resultPanel.SetActive(true);

        resultText.text = "Ваш результат:\nВы ответили на " + rightAnswers + " вопросов за " + attempts + " попыток";
    }

    public void ResetTest()
    {
        currentQuestion = 0;
        rightAnswers = 0;

        StartCoroutine(LoadQuestions());
    }

    private IEnumerator LoadQuestions()
    {
        resultPanel.SetActive(false);

        yield return new WaitForSeconds(0.3f);

        ShowQuestion();

        questionPanel.SetActive(true);
    }

}

[Serializable]
public class Question
{
    public string question;
    public string answer0;
    public string answer1;
    public string answer2;
    public string answer3;
    public int rightAnswerId;
    public AudioClip questionSound;
}
