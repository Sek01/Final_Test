using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public sealed class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    // ================================
    //   Game State
    // ================================
    public int currentScore = 0;
    public bool isGamePaused = false;

    // ================================
    //   UI References (Auto Find)
    // ================================
    public GameObject pauseMenuUI;
    public Slider HPBar;

    public TMP_Text levelText;
    public Slider expBar;
    public TMP_Text skillPointsText;

    // ================================
    //   Awake
    // ================================
    private void Awake()
    {
        // ทำ Singleton
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);

            // Subscribe event เมื่อ Scene โหลดเสร็จ
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // ================================
    //   Auto bind UI when scene changes
    // ================================
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AutoFindUI();
    }

    private void AutoFindUI()
    {
        // PauseMenu
        pauseMenuUI = GameObject.Find("Panel Paused");

        // HP Bar
        var hpObj = GameObject.Find("HPBar");
        if (hpObj != null) HPBar = hpObj.GetComponent<Slider>();

        // Level Text
        var lt = GameObject.Find("LevelText");
        if (lt != null) levelText = lt.GetComponent<TMP_Text>();

        // EXP Bar
        var xpObj = GameObject.Find("ExpBar");
        if (xpObj != null) expBar = xpObj.GetComponent<Slider>();

        // Skill Points Text
        var spObj = GameObject.Find("SkillPointsText");
        if (spObj != null) skillPointsText = spObj.GetComponent<TMP_Text>();

        Debug.Log("GameManager: UI elements auto-bound for scene " + SceneManager.GetActiveScene().name);
    }

    // ================================
    //   Health UI handling
    // ================================
    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        if (HPBar == null) return;

        HPBar.maxValue = maxHealth;
        HPBar.value = currentHealth;
    }

    // ================================
    //   EXP UI handling
    // ================================
    public void UpdateExpUI(int level, int currentExp, int expToNext, int skillPoints)
    {
        if (levelText != null)
            levelText.text = $"Lv. {level}";

        if (expBar != null)
        {
            expBar.maxValue = expToNext;
            expBar.value = currentExp;
        }

        if (skillPointsText != null)
            skillPointsText.text = $"SP: {skillPoints}";
    }

    // ================================
    //   Score
    // ================================
    public void AddScore(int amount)
    {
        currentScore += amount;
    }

    // ================================
    //   Pause System
    // ================================
    public void TogglePause()
    {
        isGamePaused = !isGamePaused;
        Time.timeScale = isGamePaused ? 0f : 1f;

        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(isGamePaused);
    }

    // ================================
    //   Input
    // ================================
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }
}
