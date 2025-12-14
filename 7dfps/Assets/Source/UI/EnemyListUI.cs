using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyListUI : MonoBehaviour
{
    public TextMeshProUGUI KillThemAllText;
    public Image EnemyUIToken;

    private Dictionary<GameObject, Image> _enemyIconsTables = new Dictionary<GameObject, Image>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy_ClownCar"));

        foreach (var enemy in enemies) { }
    }

    public void AddEnemy(GameObject enemy)
    {
        var uiToken = GameObject.Instantiate(EnemyUIToken, this.transform);
        uiToken.transform.SetParent(this.transform);
        uiToken.rectTransform.localPosition = new Vector3(
            8 + 16 * _enemyIconsTables.Count % 800,
            -8 - 16 * _enemyIconsTables.Count / 800
        );
        _enemyIconsTables[enemy] = uiToken;
        enemy.GetComponent<EnemyActor>().OnDeath += () => RemoveEnemy(enemy);
    }

    public void RemoveEnemy(GameObject enemy)
    {
        if (_enemyIconsTables.TryGetValue(enemy, out var image) && image != null)
        {
            GameObject.Destroy(image.gameObject);
        }
        _enemyIconsTables.Remove(enemy);
        if (_enemyIconsTables.Count == 0)
        {
            KillThemAllText.enabled = false;
            var levelController = GameObject
                .FindGameObjectWithTag("LevelController")
                .GetComponent<LevelController>();
            levelController.ShowLevelTransition();
        }
    }
}
