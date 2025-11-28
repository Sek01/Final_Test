using System;
using UnityEngine;

public class PlayerExperience : MonoBehaviour
{
    [Header("Level Settings")]
    public int startLevel = 1;
    public int startExpToNextLevel = 100;
    public int expIncreasePerLevel = 50;
    public int skillPointsPerLevel = 3;

    [Header("Runtime Values (Read Only)")]
    [SerializeField] private int level;
    [SerializeField] private int currentExp;
    [SerializeField] private int expToNextLevel;
    [SerializeField] private int skillPoints;

    [Header("Audio")]
    public AudioClip levelUpSFX;

    public event Action<int> OnLevelUp;

    public int Level => level;
    public int CurrentExp => currentExp;
    public int ExpToNextLevel => expToNextLevel;
    public int SkillPoints => skillPoints;

    private void Awake()
    {
        level = startLevel;
        expToNextLevel = startExpToNextLevel;
        currentExp = 0;
        skillPoints = 0;

        UpdateUI();
    }

    public void AddExp(int amount)
    {
        if (amount <= 0) return;

        currentExp += amount;

        // เลเวลอัปได้หลายครั้งถ้า exp เกิน
        while (currentExp >= expToNextLevel)
        {
            currentExp -= expToNextLevel;
            LevelUp();
        }

        UpdateUI();
    }

    void LevelUp()
    {
        level++;
        skillPoints += skillPointsPerLevel;

        // เพิ่มเกณฑ์เลเวลถัดไป
        expToNextLevel += expIncreasePerLevel;

        // เล่นเสียงเลเวลอัป
        if (SoundManager.Instance != null)
        {
            if (levelUpSFX != null)
            {
                SoundManager.Instance.PlaySFX(levelUpSFX);
            }
            else
            {
                // ถ้าไม่ได้เซ็ต levelUpSFX ใช้เสียง default ไปก่อน
                SoundManager.Instance.PlaySFX(SoundManager.Instance.defaultButtonClick);
            }
        }

        OnLevelUp?.Invoke(level);
        UpdateUI();
    }

    public bool TrySpendSkillPoints(int amount)
    {
        if (amount <= 0) return true;
        if (skillPoints < amount) return false;

        skillPoints -= amount;
        UpdateUI();
        return true;
    }

    void UpdateUI()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.UpdateExpUI(level, currentExp, expToNextLevel, skillPoints);
        }
    }
}
