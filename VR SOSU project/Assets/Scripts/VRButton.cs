using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class VRButton : MonoBehaviour
{
    [Header("Button Settings")]
    // Time that the button is set inactive after release
    public float deadTime = 1.0f;
    // Bool used to lock down button during its set dead time
    private bool _deadTimeActive = false;

    [Header("Question Settings")]
    [SerializeField] private string question;
    [SerializeField] private string[] acceptableAnswers;
    public DictationScript dictationScript;

    [Header("Events")]
    // Public Unity Events we can use in the editor and tie other functions to.
    public UnityEvent onPressed, onReleased, onCorrectAnswer, onIncorrectAnswer;

    private IXRSelectInteractor currentInteractor = null;
    private XRBaseInteractable interactable;

    public void Awake()
    {
        // Renderer Button1 = GetComponent<Renderer>();
        // Get the XR interactable component
        interactable = GetComponent<XRBaseInteractable>();
        if (interactable == null)
        {
            // Add XR Simple Interactable if it doesn't exist
            interactable = gameObject.AddComponent<XRSimpleInteractable>();
        }

        // Subscribe to interaction events
        interactable.selectEntered.AddListener(OnSelectEntered);
        interactable.selectExited.AddListener(OnSelectExited);
    }

    private void OnDestroy()
    {
        // Unsubscribe from events when destroyed
        if (interactable != null)
        {
            interactable.selectEntered.RemoveListener(OnSelectEntered);
            interactable.selectExited.RemoveListener(OnSelectExited);
        }
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (!_deadTimeActive)
        {
            currentInteractor = args.interactorObject;
            onPressed?.Invoke();
            Debug.Log("Button pressed");

            if (dictationScript != null)
            {
                dictationScript.StartListening(question, acceptableAnswers,
                    () => onCorrectAnswer?.Invoke(),
                    () => onIncorrectAnswer?.Invoke());
            }
        }
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        if (!_deadTimeActive && currentInteractor == args.interactorObject)
        {
            currentInteractor = null;
            onReleased?.Invoke();
            Debug.Log("Button released");
            StartCoroutine(WaitForDeadTime());
        }
    }




    /*// Checks if the current collider entering is the Button and sets off OnPressed event.
    private void OnTriggerEnter(Collider other)
    {
        Renderer Button1 = GetComponent<Renderer> ();
        if (other.tag == "Button" && !_deadTimeActive)
        {
            Button1.material.color = Color.gray;
            onPressed?.Invoke();
            Debug.Log("I have been pressed");
            if (dictationScript != null)
            {
                dictationScript.StartListening(question, acceptableAnswers,
                    () => onCorrectAnswer?.Invoke(),
                    () => onIncorrectAnswer?.Invoke());
            }
        }
    }

    // Checks if the current collider exiting is the Button and sets off OnReleased event.
    // It will also call a Coroutine to make the button inactive for however long deadTime is set to.
    private void OnTriggerExit(Collider other)
    {
        Renderer Button1 = GetComponent<Renderer>();
        if (other.tag == "Button" && !_deadTimeActive)
        {
            Button1.material.color = Color.red;
            onReleased?.Invoke();
            Debug.Log("I have been released");
            StartCoroutine(WaitForDeadTime());
        }
    }*/

    // Locks button activity until deadTime has passed and reactivates button activity.
    private IEnumerator WaitForDeadTime()
    {
        _deadTimeActive = true;
        yield return new WaitForSeconds(deadTime);
        _deadTimeActive = false;
    }
}
