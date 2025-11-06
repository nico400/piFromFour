using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    [SerializeField] private string sceneName;
    public void StartTheGame()
    {
        SceneManager.LoadScene(sceneName);
    }
}
