using System;
using UnityEngine;
using TMPro;


public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    [Header("Quest UI")]
    public GameObject questPanel;
    public GameObject questUI;
    public TMP_Text questTitleText;
    public TMP_Text questDescriptionText;
    public TMP_Text questProgressText;

    private QuestGiver activeQuest;
    private int currentKillCount;
    private Player player;
    private PlayerExperience playerExperience;

    public void HideQuestUI()
    {
        if (questUI != null)
            questUI.SetActive(false);
    }


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void BeginKillQuest(QuestGiver giver, Player player)
    {
        if (activeQuest != null)
        {
            Debug.Log("Already have an active quest.");
            return;
        }

        Debug.Log("============== Quest Begin ===============");
        activeQuest = giver;
        currentKillCount = 0;
        this.player = player;
        playerExperience = player.GetComponent<PlayerExperience>();

        // เปิด UI เควส
        if (questPanel != null) questPanel.SetActive(true);
        UpdateQuestUI();

        // สั่งให้จุดเกิดของเควส spawn มอน
        foreach (var sp in giver.spawnPoints)
        {
            if (sp != null)
            {
                sp.ActivateSpawn(giver.enemyPrefab);
            }
        }
    }

    public void RegisterQuestEnemy(Enemy enemy)
    {
        if (enemy == null) return;

        // ฟัง event ทำลายจาก Character/Idestoryable
        enemy.OnDestory += OnQuestEnemyDestroyed;
    }

    private void OnQuestEnemyDestroyed(Idestoryable target)
    {
        if (activeQuest == null) return;

        currentKillCount++;
        UpdateQuestUI();

        if (currentKillCount >= activeQuest.requiredKills)
        {
            CompleteQuest();
        }
    }


    void CompleteQuest()
    {
        Debug.Log("Quest Complete!");

        if (playerExperience != null)
        {
            playerExperience.AddExp(activeQuest.expReward);
            // 👉 เพิ่มบรรทัดนี้
            HideQuestUI();

        }

        // ปิดสัญลักษณ์ spawn point
        foreach (var sp in activeQuest.spawnPoints)
        {
            if (sp != null)
            {
                sp.DeactivateSpawn();
            }
        }


        activeQuest = null;
        currentKillCount = 0;
    }

    void UpdateQuestUI()
    {
        if (activeQuest == null) return;

        if (questTitleText != null)
        {
            questTitleText.text = activeQuest.title;
        }

        if (questDescriptionText != null)
        {
            questDescriptionText.text = activeQuest.description;
        }

        if (questProgressText != null)
        {
            questProgressText.text = $"{currentKillCount}/{activeQuest.requiredKills}";
        }
    }
}
