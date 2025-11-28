using UnityEngine;
using TMPro;

public class QuestGiver : Identity, IInteractable
{
    [Header("Interact")]
    public bool canTalk = true;
    public bool isInteractable { get => canTalk; set => canTalk = value; }

    [Header("Quest Data")]
    public string questId = "KillQuest01";
    public string title = "Clear the Monsters";
    [TextArea] public string description = "Kill all monsters in the marked area.";
    public int requiredKills = 5;
    public int expReward = 100;
    public GameObject enemyPrefab;
    public QuestSpawnPoint[] spawnPoints;

    [Header("UI")]
    public TMP_Text interactionTextUI;   // ข้อความ "กด E เพื่อรับ Quest"
    public TMP_Text questAcceptTextUI;   // ข้อความ "Quest Accepted!"

    private bool isQuestStarted = false;

    private void Start()
    {
        if (interactionTextUI != null)
            interactionTextUI.gameObject.SetActive(false);

        if (questAcceptTextUI != null)
            questAcceptTextUI.gameObject.SetActive(false);
    }

    private void Update()
    {
        Player player = FindAnyObjectByType<Player>();
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.transform.position);

        if (!isQuestStarted && canTalk && dist <= 2f)
        {
            if (interactionTextUI != null)
                interactionTextUI.gameObject.SetActive(true);
        }
        else
        {
            if (interactionTextUI != null)
                interactionTextUI.gameObject.SetActive(false);
        }
    }

    public void Interact(Player player)
    {
        if (isQuestStarted) return;

        Debug.Log("Accept Quest");

        isQuestStarted = true;

        if (interactionTextUI != null)
            interactionTextUI.gameObject.SetActive(false);

        if (questAcceptTextUI != null)
        {
            questAcceptTextUI.text = "Quest Accepted!";
            questAcceptTextUI.gameObject.SetActive(true);
            Invoke(nameof(HideAcceptText), 3f);
        }

        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.BeginKillQuest(this, player);
        }
    }

    void HideAcceptText()
    {
        if (questAcceptTextUI != null)
            questAcceptTextUI.gameObject.SetActive(false);
    }
}
