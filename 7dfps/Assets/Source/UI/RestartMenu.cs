using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartMenu : MonoBehaviour
{
    public void OnRestartClicked()
    {
        LevelController levelController = GameObject
            .FindWithTag("LevelController")
            .GetComponent<LevelController>();
        SceneManager.LoadScene(levelController.ThisLevelName);
    }

    public void OnGoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
