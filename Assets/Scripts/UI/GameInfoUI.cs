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

    private void Awake()
    {
        EventBus<HealthUpdateEvent>.Subscribe(OnHealthChange);
        EventBus<MoneyChangeEvent>.Subscribe(OnMoneyChanged);
        EventBus<WavePauseUpdate>.Subscribe(OnWaveStatusUpdate);
        EventBus<GameOverEvent>.Subscribe(OnGameOver);

        livesText.text = "";
        moneyText.text = "";
        waveNumber.text = "";
        levelText.text = "";
        waveTimer.text = "";
        winOrLoseText.text = "";
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
    }

    void OnGameOver(Event e)
    {
        GameOverEvent gameOver = e as GameOverEvent;
        winOrLoseText.text = (gameOver.playerWon ? "You Win!" : "You lose :(");
    }

    private void Update()
    {
        levelText.text = "Level: " + EnemyManager.Instance.currentLevel.ToString();
        waveNumber.text = "Wave: " + EnemyManager.Instance.currentWave.ToString();
    }
}
