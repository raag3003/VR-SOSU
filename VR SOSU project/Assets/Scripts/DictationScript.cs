using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class DictationScript : MonoBehaviour
{
    public Player player;
    private Text m_Recognitions;

    // Liste af strings som gemmer outputtet af dikationen
    public System.Action<string> OnTextRecognized;

    private DictationRecognizer m_DictationRecognizer;

    void Start()
    {
        m_DictationRecognizer = new DictationRecognizer();

        // Når et ord bliver korrekt genkendt, tilføjes det til m_Recognitions og logges.
        m_DictationRecognizer.DictationResult += (text, confidence) =>
        {
            Debug.LogFormat("Dictation result: {0}", text);
            /*if (m_Recognitions != null)
                m_Recognitions.text += text + "\n";*/

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

        // Start()-metoden og det at den ligger inde i unity's Start() metode gør, at den begynder at lytte fra første frame af spillet.
        m_DictationRecognizer.Start();
    }

    private void Update()
    {
        m_DictationRecognizer.Start();
    }

}
