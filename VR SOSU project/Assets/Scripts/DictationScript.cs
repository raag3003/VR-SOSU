using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;
using UnityEngine.Networking;

public class DictationScript : MonoBehaviour
{
    public Player player;
    private Text m_Recognitions;

    // Liste af strings som gemmer outputtet af dikationen
    public System.Action<string> OnTextRecognized;
    public DictationRecognizer m_DictationRecognizer;

    private string[] currentAcceptableAnswers;
    private System.Action onCorrectAnswer;
    private System.Action onIncorrectAnswer;

    [Header("UI")]
    public TMPro.TextMeshProUGUI Correct;
    public TMPro.TextMeshProUGUI Incorrect;

    void Start()
    {
        m_DictationRecognizer = new DictationRecognizer();

        // Når et ord bliver korrekt genkendt, tilføjes det til m_Recognitions og logges.
        m_DictationRecognizer.DictationResult += (text, confidence) =>
        {
            Debug.LogFormat("Dictation result: {0}", text);
            CheckAnswer(text);

            // Sender diktationen til LLM'en
            OnTextRecognized?.Invoke(text);
        };
        // Når genkendelsen stopper, bliver der tjekket om det skete som det skulle, eller om der opstod en fejl.
        m_DictationRecognizer.DictationComplete += (completionCause) =>
        {
            if (completionCause != DictationCompletionCause.Complete)
                Debug.LogErrorFormat("Dictation completed unsuccessfully: {0}.", completionCause);
        };
        // Hvis der skulle opstå en fejl i DictationComplete, vil den så blive  vist i consolen.
        m_DictationRecognizer.DictationError += (error, hresult) =>
        {
            Debug.LogErrorFormat("Dictation error: {0}; HResult = {1}.", error, hresult);
        };

        if (player.chatIsActive)
            // Start()-metoden og det at den ligger inde i unity's Start() metode gør, at den begynder at lytte fra første frame af spillet.
            m_DictationRecognizer.Start();
    }

    private void Update()
    {
        StartCoroutine(CheckFeedbackIsActive());

        if (player.chatIsActive && m_DictationRecognizer.Status != SpeechSystemStatus.Running)
            m_DictationRecognizer.Start();
    }


    public void StartListening(string question, string[] acceptableAnswers,
        System.Action correctCallback, System.Action incorrectCallback)
    {
        currentAcceptableAnswers = acceptableAnswers;
        onCorrectAnswer = correctCallback;
        onIncorrectAnswer = incorrectCallback;

        TextToSpeech.Speak(question);
        m_DictationRecognizer.Start();
    }

    private void CheckAnswer(string spokenText)
    {
        if (currentAcceptableAnswers == null) return;

        foreach (string answer in currentAcceptableAnswers)
        {
            if (spokenText.ToLower().Contains(answer.ToLower()))
            {
                Correct.gameObject.SetActive(true);
                onCorrectAnswer?.Invoke();
                m_DictationRecognizer.Stop();
                return;
            } else
            {
                Incorrect.gameObject.SetActive(true);
            }
        }

        onIncorrectAnswer?.Invoke();
    }

    private IEnumerator CheckFeedbackIsActive()
    {
        if (Correct.gameObject.active == true)
        {
            yield return new WaitForSecondsRealtime(5);
            Correct.gameObject.SetActive(false);
        } else if (Incorrect.gameObject.active == true)
        {
            yield return new WaitForSecondsRealtime(5);
            Incorrect.gameObject.SetActive(false);
        }

    }
}
