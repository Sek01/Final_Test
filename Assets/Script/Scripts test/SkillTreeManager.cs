using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum SkillBranch
{
    Health,
    Damage,
    Mage
}

[System.Serializable]
public class SkillNode
{
    public string id;
    public SkillBranch branch;

    [Tooltip("Node ที่ต้องอัปก่อน (ปล่อยว่างได้)")]
    public int prerequisiteIndex = -1;

    [Tooltip("ต้องใช้ Skill Points เท่าไหร่")]
    public int cost = 1;

    [Header("UI")]
    public Button button;
    public TMP_Text label;

    [Header("ผลลัพธ์สายเลือด")]
    public int healthBonus;
    public int maxHealthBonus;

    //[Header("ผลลัพธ์สายดาเมจ")]
    public int damageBonus;

    [Header("ผลลัพธ์สายเมจ")]
    [Tooltip("Index ของสกิลใน SkillBook (0 = Fireball, 1 = Heal, 2 = Speed Buff)")]
    public int mageSkillIndex = -1;

    [HideInInspector] public bool isUnlocked;
}

public class SkillTreeManager : MonoBehaviour
{

    [Header("SkillTree UI")]
    public TMP_Text skillTreeLevelText;
    public TMP_Text skillTreeSPText;

    Color GetBranchColor(SkillBranch branch)
    {
        switch (branch)
        {
            case SkillBranch.Health: return Color.green;
            case SkillBranch.Damage: return Color.red;
            case SkillBranch.Mage: return Color.cyan;
        }
        return Color.white;
    }

    [Header("General")]
    public GameObject skillTreePanel;

    public Player player;
    public PlayerExperience playerExperience;
    public SkillBook skillBook;

    [Header("Nodes")]
    public List<SkillNode> nodes = new List<SkillNode>();


    private void Start()
    {
        if (skillTreePanel != null)
        {
            skillTreePanel.SetActive(false);
        }

        // ผูกปุ่มกับ node แต่ละตัว
        foreach (var node in nodes)
        {
            if (node.button != null)
            {
                SkillNode captured = node;
                node.button.onClick.AddListener(() => OnClickNode(captured));
            }
            UpdateNodeVisual(node);
        }

        RefreshExpUI();
    }


    private void Update()
    {
        // กด B เพื่อเปิด/ปิด Skill Tree
        if (Input.GetKeyDown(KeyCode.B))
        {
            bool newState = !skillTreePanel.activeSelf;
            skillTreePanel.SetActive(newState);

            // ⭐ อัปเดต LV / SP ทุกครั้งที่เปิด SkillTree
            if (newState)
            {
                RefreshExpUI();
            }
        }

    }

    void OnClickNode(SkillNode node)
    {
        // ถ้าปลดแล้ว ไม่ต้องทำอะไร
        if (node.isUnlocked)
            return;

        // ================================
        // 1) เช็ค prerequisite
        // ================================
        if (node.prerequisiteIndex >= 0)
        {
            SkillNode pre = nodes[node.prerequisiteIndex];

            if (!pre.isUnlocked)
            {
                Debug.Log($"ต้องอัป {pre.id} ก่อน");
                return;
            }
        }

        // ไม่มี playerExperience = error ปิดการอัป
        if (playerExperience == null)
            return;

        // ================================
        // 2) เช็ค Skill Points พอไหม
        // ================================
        if (!playerExperience.TrySpendSkillPoints(node.cost))
        {
            Debug.Log("Not enough Skill Points.");
            return;
        }

        // ================================
        // 3) ปลดสกิล
        // ================================
        node.isUnlocked = true;

        // 4) ให้ผลสกิล
        ApplyNodeEffect(node);

        // ================================
        // 5) อัปเดต UI ของทุกปุ่ม (สำคัญมาก)
        // ================================
        foreach (var n in nodes)
        {
            UpdateNodeVisual(n);
        }

        // ================================
        // 6) อัปเดต UI EXP / Skill Point
        // ================================
        RefreshExpUI();
    }


    void ApplyNodeEffect(SkillNode node)
    {
        if (player != null)
        {
            // สายเพิ่มเลือด
            if (node.maxHealthBonus != 0)
            {
                player.maxHealth += node.maxHealthBonus;
            }

            if (node.healthBonus != 0)
            {
                player.health += node.healthBonus;
            }

            // สายเพิ่มดาเมจ
            if (node.damageBonus != 0)
            {
                player.Damage += node.damageBonus;
            }

            // อัปเดต HPBar ใน GameManager
            if (GameManager.Instance != null)
            {
                GameManager.Instance.UpdateHealthBar(player.health, player.maxHealth);
            }
        }

        // สายเมจ → ปลดล็อกสกิล
        if (node.branch == SkillBranch.Mage && skillBook != null && node.mageSkillIndex >= 0)
        {
            skillBook.UnlockSkill(node.mageSkillIndex);
        }
    }

    void UpdateNodeVisual(SkillNode node)
    {
        if (node.label != null)
        {
            node.label.text = $"{node.id}\nCost: {node.cost}";
        }

        if (node.button != null)
        {
            Image img = node.button.GetComponent<Image>();

            // 1) กรณียังไม่ปลดสกิล
            if (!node.isUnlocked)
            {
                // กดได้ก่อน (เพื่อให้ไปเช็ค prerequisite ใน OnClickNode)
                node.button.interactable = true;
                img.color = Color.gray;

                // 2) เช็ค prerequisite ว่าปลดก่อนหน้าหรือยัง
                if (node.prerequisiteIndex >= 0)
                {
                    SkillNode pre = nodes[node.prerequisiteIndex];

                    if (!pre.isUnlocked)
                    {
                        // ถ้ายังไม่ปลดสกิลก่อนหน้า → ปิดปุ่ม
                        node.button.interactable = false;
                        img.color = Color.gray;
                    }
                }

                return;
            }

            // 3) ถ้าปลดแล้ว = สีตามสาย
            node.button.interactable = true;

            switch (node.branch)
            {
                case SkillBranch.Health:
                    img.color = Color.green;
                    break;

                case SkillBranch.Damage:
                    img.color = Color.red;
                    break;

                case SkillBranch.Mage:
                    img.color = Color.cyan;
                    break;
            }
        }
    }


    void RefreshExpUI()
    {
        if (playerExperience != null && GameManager.Instance != null)
        {
            GameManager.Instance.UpdateExpUI(
                playerExperience.Level,
                playerExperience.CurrentExp,
                playerExperience.ExpToNextLevel,
                playerExperience.SkillPoints
            );
        }

        // ⭐ อัปเดตค่าใน SkillTreePanel ด้วย
        if (skillTreeLevelText != null)
            skillTreeLevelText.text = $"Lv. {playerExperience.Level}";

        if (skillTreeSPText != null)
            skillTreeSPText.text = $"SP: {playerExperience.SkillPoints}";
    }  

}   


