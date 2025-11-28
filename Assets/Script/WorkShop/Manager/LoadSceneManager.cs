using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; 

public sealed class LoadSceneManager : MonoBehaviour
{
    private static LoadSceneManager _instance;
    public static LoadSceneManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("LoadSceneManager instance is null! Is it in the scene?");
            }
            return _instance;
        }
    }

    [Header("Loading Screen Reference")]
    public GameObject loadingScreenPanel; 

   
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);

            if (loadingScreenPanel != null)
            {
                loadingScreenPanel.SetActive(false);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ------------------- Core Functionality -------------------

    public void LoadNewScene(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        if (loadingScreenPanel != null)
        {
            loadingScreenPanel.SetActive(true);
        }

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            yield return null;
        }

        if (loadingScreenPanel != null)
        {
            loadingScreenPanel.SetActive(false);
        }

        Debug.Log($"Scene '{sceneName}' loaded and activated successfully.");
    }

    public void HideLoadingScreen()
    {
        if (loadingScreenPanel != null)
        {
            loadingScreenPanel.SetActive(false);
        }
    }

    // ------------------- Utility -------------------

    public string GetCurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }
}