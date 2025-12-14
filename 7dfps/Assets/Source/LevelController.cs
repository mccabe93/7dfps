using UnityEngine;

public class LevelController : MonoBehaviour
{
    public string ThisLevelName;
    public string NextLevelName;
    public GameObject RestartMenu;
    public GameObject LevelTransition;
    public GameObject VictoryScreen;

    public void ShowLevelTransition()
    {
        if (NextLevelName == "Victory")
        {
            VictoryScreen.SetActive(true);
            VictoryScreen.GetComponent<VictoryUI>().Initialize();
        }
        else
        {
            LevelTransition.SetActive(true);
            LevelTransition.GetComponent<LevelTransitionUI>().Initialize();
        }
    }

    public void HideLevelTransition()
    {
        LevelTransition.SetActive(false);
    }

    public void ShowRestartMenu()
    {
        HideLevelTransition();
        RestartMenu.SetActive(true);
    }

    public void HideRestartMenu()
    {
        RestartMenu.SetActive(false);
    }
}
