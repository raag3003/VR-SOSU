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
        }



    }

    public void VoiceActivate()
    {
        voiceExperience.Activate();
    }



    /*public void OnSelectEntered(SelectEnterEventArgs args)
    {
        isPressed = true;

    }

    public void OnSelectExited(SelectExitEventArgs args)
    {
        isPressed = false;

    }*/
}
