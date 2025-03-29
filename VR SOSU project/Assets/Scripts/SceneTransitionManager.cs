using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public FadeScreen fadeScreen;
    public static SceneTransitionManager singleton;

    private void Awake()
    {
        if (singleton && singleton != this)
            Destroy(singleton);

        singleton = this;
    }

    public void GoToScene(int PrototypeRoom)
    {
        StartCoroutine(GoToSceneRoutine(PrototypeRoom));
    }

    IEnumerator GoToSceneRoutine(int PrototypeRoom)
    {
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(fadeScreen.fadeDuration);

        //Launch the new scene
        SceneManager.LoadScene(PrototypeRoom);
    }

    public void GoToSceneAsync(int PrototypeRoom)
    {
        StartCoroutine(GoToSceneAsyncRoutine(PrototypeRoom));
    }

    IEnumerator GoToSceneAsyncRoutine(int PrototypeRoom)
    {
        fadeScreen.FadeOut();
        //Launch the new scene
        AsyncOperation operation = SceneManager.LoadSceneAsync(PrototypeRoom);
        operation.allowSceneActivation = false;

        float timer = 0;
        while(timer <= fadeScreen.fadeDuration && !operation.isDone)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        operation.allowSceneActivation = true;
    }
}
