using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Core.Gameplay.Objectives;
using System.Collections;

public class ObjectiveUIManager : MonoBehaviour
{
    private ObjectiveManager objectiveManager;
    public Transform objectiveListParent; // The Vertical Layout group
    public GameObject objectiveTextPrefab; // A TMP text prefab

    private Dictionary<Objective, TMP_Text> uiMap = new();

    void Start()
    {
        objectiveManager = new ObjectiveManager();

        objectiveManager.OnObjectiveAdded += CreateObjectiveUI;



        var findHans = new Objective("FindHans", "Hans Found: {0}/{1}", 1);
        objectiveManager.AddObjective(findHans);

        // Simulate progress
        StartCoroutine(DelayedProgress(findHans));
    }
    IEnumerator DelayedProgress(Objective obj)
    {
        yield return new WaitForSecondsRealtime(10);
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
            textComponent.text = objective.GetStatusText();
        };

        uiMap.Add(objective, textComponent);
    }
}
