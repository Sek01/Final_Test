using UnityEngine;

public class SceneMusicChanger : MonoBehaviour
{
    public AudioClip sceneMusic;  // เพลงของซีนนี้

    void Start()
    {
        if (sceneMusic != null)
        {
            SoundManager.Instance.PlayMusic(sceneMusic);
        }
    }
}
