using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public void OnPlayButtonClicked()
    {
        // ใส่ชื่อซีนจริงของคุณ เช่น "GameScene"
        LoadSceneManager.Instance.LoadNewScene("MainGame");
    }
}
