using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Voice;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
public class VoiceScript : MonoBehaviour
{
    public AppVoiceExperience voiceExperience;
    public UnityEvent onPressed, onReleased, onCorrectAnswer, onIncorrectAnswer;
    private bool isPressed;
    public string Korrekt = "Så fik du vasket ansigtet";
    public string Forkert = "Det vat godt gået";

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetUp(OVRInput.Button.One) || Input.GetKey(KeyCode.K))
        {
            voiceExperience.Activate();
            Debug.Log("Listening from input");
        }



    }

    /*public void VoiceActivate()
    {
        voiceExperience.Activate();
        Debug.Log("Listening from button");
    }*/



    public void OnSelectEntered(SelectEnterEventArgs args)
    {
        isPressed = true;
        onPressed?.Invoke();
        Debug.Log("Button pressed");
    }

    public void OnSelectExited(SelectExitEventArgs args)
    {
        isPressed = false;
        onReleased?.Invoke();
        voiceExperience.Activate();
        Debug.Log("Button released");
    }
}
