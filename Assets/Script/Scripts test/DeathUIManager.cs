using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathUIManager : MonoBehaviour
{
    public static DeathUIManager Instance;

    public GameObject deathScreen;

    private void Awake()
    {
        Instance = this;
        if (deathScreen != null)
            deathScreen.SetActive(false);
    }

    public void ShowDeathScreen()
    {
        if (deathScreen != null)
            deathScreen.SetActive(true);

        Time.timeScale = 0f; // หยุดเกม
    }

    public void Retry()
    {
        Time.timeScale = 1f;  // คืนเวลา
        string scene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(scene);
    }
}

