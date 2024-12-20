using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void SwitchtoGameScene()
    {
        SceneManager.LoadScene("GameplayScene");
    }
}
