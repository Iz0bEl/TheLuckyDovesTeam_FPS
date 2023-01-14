using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public GameObject LoadScreen;
    public Image LoadBarFill;

    public void LoadtheScene(int sceneID)
    {
        StartCoroutine(LoadSceneAsync(sceneID));
    }

    IEnumerator LoadSceneAsync(int sceneID)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneID);

        LoadScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);

            LoadBarFill.fillAmount = progressValue;

            yield return null;
        }

        if (operation.isDone)
        {
            LoadScreen.SetActive(false);
        }
    }
}
