using System.Runtime.InteropServices;
using UnityEngine;

public class TextToSpeech : MonoBehaviour
{
    [DllImport("ttsrust")]
    private static extern void ttsrust_say(string text);

    public static void Speak(string textToSpeak) => ttsrust_say(textToSpeak);
}