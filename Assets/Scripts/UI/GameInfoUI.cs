using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameInfoUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] TextMeshProUGUI waveNumber;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI waveTimer;
    [SerializeField] TextMeshProUGUI winOrLoseText;
    [SerializeField] Slider gameSpeedSlider;
    [SerializeField] GameObject gameOverPanel;

    private void Awake()
    {
        EventBus<HealthUpdateEvent>.Subscribe(OnHealthChange);
        EventBus<MoneyChangeEvent>.Subscribe(OnMoneyChanged);
        EventBus<WavePauseUpdate>.Subscribe(OnWaveStatusUpdate);
        EventBus<GameOverEvent>.Subscribe(OnGameOver);

        livesText.text = "Health: " + GameManager.Instance.Health.ToString();
        moneyText.text = "Money: " + GameManager.Instance.Money.ToString();
        waveNumber.text = "";
        levelText.text = "";
        waveTimer.text = "";
        winOrLoseText.text = "";
        gameOverPanel.SetActive(false);
    }

    private void OnDestroy()
    {
        EventBus<HealthUpdateEvent>.Unsubscribe(OnHealthChange);
        EventBus<MoneyChangeEvent>.Unsubscribe(OnMoneyChanged);
        EventBus<WavePauseUpdate>.Unsubscribe(OnWaveStatusUpdate);
        EventBus<GameOverEvent>.Unsubscribe(OnGameOver);
    }

    void OnDisable()
    {
        EventBus<HealthUpdateEvent>.Unsubscribe(OnHealthChange);
        EventBus<MoneyChangeEvent>.Unsubscribe(OnMoneyChanged);
        EventBus<WavePauseUpdate>.Unsubscribe(OnWaveStatusUpdate);
        EventBus<GameOverEvent>.Unsubscribe(OnGameOver);
    }

    void OnHealthChange(Event e)
    {
        livesText.text = "Health: " + (e as HealthUpdateEvent).health.ToString();
    }

    void OnMoneyChanged(Event e)
    {
        moneyText.text = "Money: " + (e as MoneyChangeEvent).money.ToString();
    }

    void OnWaveStatusUpdate(Event e)
    {
        WavePauseUpdate waveStatus = e as WavePauseUpdate;
        waveTimer.text = waveStatus.isPaused ? "Build Phase: " + waveStatus.pauseTime.ToString() : "";
        waveNumber.text = "Wave: " + WaveManager.Instance.currentWave.ToString();
        levelText.text = "Level: " + WaveManager.Instance.currentLevel.ToString();
    }

    void OnGameOver(Event e)
    {
        GameOverEvent gameOver = e as GameOverEvent;
        winOrLoseText.text = gameOver.playerWon ? "You Win!" : "You lose!";
        gameOverPanel.SetActive(true);

    }

    public void UpdateGameSpeed()
    {
        Time.timeScale = gameSpeedSlider.value;
    }
}
