using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainLoaderScript : MonoBehaviour
{
    [SerializeField] private Slider progressBar;
    [SerializeField] private TMP_Text progressText;

    void Start()
    {
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        string sceneName = SceneLoaderData.SceneToLoad;

        if (string.IsNullOrEmpty(sceneName))
        {
            sceneName = "MenuScene";
            Debug.LogWarning("Scene to load was not specified. Loading MainMenu.");
        }
        AsyncOperation operation;
        try
        {
            operation = SceneManager.LoadSceneAsync(sceneName);
            operation.allowSceneActivation = false;
        }
        catch
        {
            operation = SceneManager.LoadSceneAsync("MenuScene");
            operation.allowSceneActivation = false;
        }

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            if (progressBar != null)
                progressBar.value = progress;

            if (progressText != null)
                progressText.text = Mathf.Round(progress * 100) + "%";

            if (operation.progress >= 0.9f)
            {
                yield return new WaitForSeconds(0.5f);
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}


public static class SceneLoaderData
{
    public static string SceneToLoad { get; set; }
}