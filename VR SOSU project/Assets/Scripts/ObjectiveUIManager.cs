using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Core.Gameplay.Objectives;
using System.Collections;
using static Unity.Burst.Intrinsics.X86.Avx;

public class ObjectiveUIManager : MonoBehaviour
{
    private ObjectiveManager objectiveManager;
    public Transform objectiveListParent; // The Vertical Layout group
    public GameObject objectiveTextPrefab; // A TMP text prefab
    public Canvas objectiveCanva; // The world space canva used for holding the objective text and panel

    private Dictionary<Objective, TMP_Text> uiMap = new();

    private Objective findHans;
    void Start()
    {
        objectiveManager = new ObjectiveManager();
        objectiveManager.OnObjectiveAdded += CreateObjectiveUI;



        findHans = new Objective("FindHans", "Hans Found: {0}/{1}", 5);
        objectiveManager.AddObjective(findHans);
    }
    void Update()
    {
        if ((OVRInput.GetDown(OVRInput.Button.Four, OVRInput.Controller.LTouch) || Input.GetKeyDown(KeyCode.O)) && objectiveCanva.gameObject.active == false)
        {
            objectiveCanva.gameObject.SetActive(true);
        } else if ((OVRInput.GetDown(OVRInput.Button.Four, OVRInput.Controller.LTouch) || Input.GetKeyDown(KeyCode.O)) && objectiveCanva.gameObject.active == true)
        {
            objectiveCanva.gameObject.SetActive(false);
        }

        // Simulate progress
        StartCoroutine(DelayedProgress(findHans));
    }
    IEnumerator DelayedProgress(Objective obj)
    {
        yield return new WaitForSecondsRealtime(4);
        obj.AddProgress(1);
    }

    void CreateObjectiveUI(Objective objective)
    {
        GameObject newTextObj = Instantiate(objectiveTextPrefab, objectiveListParent);
        TMP_Text textComponent = newTextObj.GetComponent<TMP_Text>();
        textComponent.text = objective.GetStatusText();

        // Update UI when value changes
        objective.OnValueChange += () =>
        {
            if (objective.IsComplete)
            {
                textComponent.text = $"<s>{objective.GetStatusText()}</s>";
            } else
            {
                textComponent.text = objective.GetStatusText();
            }

            // Cross out the completed objectives
            objective.OnComplete += () =>
            {
                textComponent.text = $"<s>{objective.GetStatusText()}</s>";
            };
        };

        uiMap.Add(objective, textComponent);
    }
}