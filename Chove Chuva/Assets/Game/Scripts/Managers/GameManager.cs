using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    Enemy[] _enemies;

    public int EnemyCount { get; private set; }

    PlayerController _player;

    private void Awake()
    {
        Instance = this;

        _player = FindObjectOfType<PlayerController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        StartCoroutine(CheckForEnemies());
    }

    IEnumerator CheckForEnemies()
    {
        _enemies = FindObjectsOfType<Enemy>();
        EnemyCount = _enemies.Length;

        yield return new WaitUntil(() => _enemies.All(t => !t.gameObject.activeSelf));

        Victory();
    }

    public void DeadEnemy()
    {
        EnemyCount--;
    }

    public void Victory()
    {
        _player.enabled = false;
        UIManager.Instance.Victory();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void GameOver()
    {
        StopAllCoroutines();
        UIManager.Instance.GameOver();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void TryAgain() => SceneManager.LoadScene(0);

    public void Quit() => Application.Quit();
}
