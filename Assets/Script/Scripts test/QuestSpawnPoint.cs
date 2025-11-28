using UnityEngine;

public class QuestSpawnPoint : MonoBehaviour
{
    [Header("Indicator")]
    public GameObject indicator;     // ไอคอน/สัญลักษณ์ตำแหน่งมอน (เช่น ลูกศร, Marker)

    [Header("Spawn Settings")]
    public int enemiesToSpawn = 3;
    public float spawnRadius = 2f;

    public void ActivateSpawn(GameObject enemyPrefab)
    {
        Debug.Log("Active Spawn");
        Debug.Log(indicator);

        if (indicator != null)
        {
            indicator.SetActive(true);
        }

        if (enemyPrefab == null) return;

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Debug.Log("in");
            Vector3 randomPos = transform.position + Random.insideUnitSphere * spawnRadius;
            randomPos.y = transform.position.y;

            GameObject enemyGO = Instantiate(enemyPrefab, randomPos, Quaternion.identity);
            Enemy enemy = enemyGO.GetComponent<Enemy>();

            if (enemy != null && QuestManager.Instance != null)
            {
                QuestManager.Instance.RegisterQuestEnemy(enemy);
            }
        }
    }

    public void DeactivateSpawn()
    {
        if (indicator != null)
        {
            indicator.SetActive(false);
        }
    }
}
