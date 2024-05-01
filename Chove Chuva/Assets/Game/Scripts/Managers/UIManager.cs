using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] GameObject _gameplayPanel;
    [SerializeField] GameObject _gameOverPanel;
    [SerializeField] GameObject _victoryPanel;

    [Header("Gameplay Panel")]
    [SerializeField] Slider _healthSlider;
    [SerializeField] TextMeshProUGUI _enemiesCount;

    PlayerController _player;

    private void Awake()
    {
        Instance = this;

        _player = FindObjectOfType<PlayerController>();
    }

    private void Start()
    {
        UpdateGameplayPanel();
        
    }

    public void UpdateGameplayPanel()
    {
        _enemiesCount.text = "x" + GameManager.Instance.EnemyCount.ToString();
        _healthSlider.value = _player.CurrentHealth;
    }

    public void Victory()
    {
        _victoryPanel.SetActive(true);
        _gameplayPanel.SetActive(false);
    }

    public void GameOver()
    {
        _gameOverPanel.SetActive(true);
        _gameplayPanel.SetActive(false);
    }
}
