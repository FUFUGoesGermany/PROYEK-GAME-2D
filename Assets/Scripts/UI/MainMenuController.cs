using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private string gameplaySceneName = "Gameplay";
    
    public void PlayGame()
    {
        SceneManager.LoadScene(gameplaySceneName);
    }
}