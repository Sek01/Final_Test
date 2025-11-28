using System.Collections.Generic;
using UnityEngine;

public class SkillBook : MonoBehaviour
{
    public List<Skill> skillSet = new List<Skill>();
    public GameObject[] skillEffects;

    // สกิลที่ทำงานต่อเนื่อง
    private List<Skill> DulationSkills = new List<Skill>();

    private Player player;

    // สถานะสกิลเมจว่า unlock หรือยัง
    [Header("Skill Unlocks")]
    [SerializeField] private bool[] unlockedSkills = new bool[3];

    private void Start()
    {
        player = GetComponent<Player>();

        // เพิ่มสกิลพื้นฐาน
        skillSet.Add(new FireballSkill());       // index 0
        skillSet.Add(new HealSkill());           // index 1
        skillSet.Add(new BuffSkillMoveSpeed());  // index 2

        // เริ่มต้นล็อกทั้งหมดไว้ก่อน
        for (int i = 0; i < unlockedSkills.Length; i++)
        {
            unlockedSkills[i] = false;
        }
    }

    private void Update()
    {
        // กดเลข 1
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TryUseSkill(0);
        }
        // กดเลข 2
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            TryUseSkill(1);
        }
        // กดเลข 3
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            TryUseSkill(2);
        }

        // อัปเดตสกิลระยะเวลา (Duration)
        for (int i = DulationSkills.Count - 1; i >= 0; i--)
        {
            DulationSkills[i].UpdateSkill(player);

            if (DulationSkills[i].timer <= 0)
            {
                DulationSkills.RemoveAt(i);
            }
        }
    }

    void TryUseSkill(int index)
    {
        if (index < 0 || index >= skillSet.Count) return;

        // ไม่ได้ปลดล็อกสกิลนี้ → กดไม่ได้
        if (index < unlockedSkills.Length && !unlockedSkills[index])
        {
            Debug.Log($"Skill {index} is locked. Unlock it in the skill tree.");
            return;
        }

        UseSkill(index);
    }

    public void UseSkill(int index)
    {
        if (index < 0 || index >= skillSet.Count) return;

        Skill skill = skillSet[index];

        if (!skill.IsReady(Time.time))
        {
            Debug.Log($"Skill '{skill.skillName}' is on cooldown. Time remaining: {skill.lastUsedTime + skill.cooldownTime - Time.time:F2}s");
            return;
        }

        if (skillEffects != null && index < skillEffects.Length && skillEffects[index] != null)
        {
            GameObject g = Instantiate(skillEffects[index], transform.position, Quaternion.identity, transform);
            Destroy(g, 1f);
        }

        skill.Activate(player);
        skill.TimeStampSkill(Time.time);

        if (skill.timer > 0)
        {
            DulationSkills.Add(skill);
        }
    }

    // เรียกจาก SkillTreeManager ตอนอัปสกิลสายเมจ
    public void UnlockSkill(int index)
    {
        if (index < 0 || index >= unlockedSkills.Length) return;

        unlockedSkills[index] = true;
        Debug.Log($"Skill {index} unlocked.");
    }
}
