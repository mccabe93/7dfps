using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransitionUI : MonoBehaviour
{
    public TextMeshProUGUI LevelName;
    public TextMeshProUGUI Countdown;
    public int CountdownStart = 5;

    private LevelController _levelController;

    public void Initialize()
    {
        _levelController = GameObject
            .FindGameObjectWithTag("LevelController")
            .GetComponent<LevelController>();
        LevelName.text = _levelController.NextLevelName;
        StartCoroutine(StartCountdown());
    }

    private System.Collections.IEnumerator StartCountdown()
    {
        int countdownValue = CountdownStart;
        while (countdownValue > 0)
        {
            Countdown.text = countdownValue.ToString();
            yield return new WaitForSeconds(1f);
            countdownValue--;
        }
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(_levelController.NextLevelName);
    }
}
