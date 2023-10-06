using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlUI : MonoBehaviour
{
    private Player player;
    public Slider LifeSlider;
    public GameObject PauseBackground;
    public GameObject DeathPanel;
    public Text TimeAlive;
    public Text BestTime;
    public Text ActualScore;
    public Text BestScore;

    private int zombiesKilled;

    private const string STR_FMT_TIME_ALIVE = "{0}m{1}s";
    private const string STR_FMT_ACTUAL_SCORE = "x{0}";
    private const string STR_FMT_BEST_SCORE = "Recorde x{0}";
    private const string PLAYER_PREFS_BEST_SCORE = "bestScore";
    private const string PLAYER_PREFS_BEST_TIME = "bestTime";

    void Start()
    {
        player = GameObject.FindGameObjectWithTag(Tags.PLAYER).GetComponent<Player>();
        LifeSlider.maxValue = player.GetLife();
        UpdateLifeDisplay();
        UpadteScoreDisplay();
        UpdateBestScoreDisplay();
    }

    public void UpdateLifeDisplay()
    {
        LifeSlider.value = player.GetLife();
    }

    public void UpadteScoreDisplay()
    {
        ActualScore.text = string.Format(STR_FMT_ACTUAL_SCORE, zombiesKilled);
    }

    public void UpdateBestScoreDisplay()
    {
        BestScore.text = string.Format(STR_FMT_BEST_SCORE, PlayerPrefs.GetInt(PLAYER_PREFS_BEST_SCORE));
    }

    public void ShowDeathPanel()
    {
        UpdateBestTime(Time.timeSinceLevelLoad);
        UpdateBestScore();
        DeathPanel.SetActive(true);
        ResetZombiesKilled();
    }

    private void UpdateBestTime(float lastTime)
    {
        float bestTime = PlayerPrefs.GetFloat(PLAYER_PREFS_BEST_TIME);
        if (lastTime > bestTime)
        {
            bestTime = lastTime;
            PlayerPrefs.SetFloat(PLAYER_PREFS_BEST_TIME, bestTime);
        }
        TimeAlive.text = string.Format(STR_FMT_TIME_ALIVE, (int) lastTime / 60, (int) lastTime % 60);
        BestTime.text = string.Format(STR_FMT_TIME_ALIVE, (int) bestTime / 60, (int) bestTime % 60);
    }

    private void UpdateBestScore()
    {
        int bestScore = PlayerPrefs.GetInt(PLAYER_PREFS_BEST_SCORE);
        if (zombiesKilled > bestScore)
        {
            bestScore = zombiesKilled;
            PlayerPrefs.SetInt(PLAYER_PREFS_BEST_SCORE, bestScore);
        }
        BestScore.text = string.Format(STR_FMT_BEST_SCORE, bestScore);
        UpdateBestScoreDisplay();
    }

    public void IncrementZombiesKilled()
    {
        zombiesKilled ++;
        UpadteScoreDisplay();
    }

    private void ResetZombiesKilled()
    {
        zombiesKilled = 0;
        UpadteScoreDisplay();
    }

    public void ShowPauseBackground()
    {
        PauseBackground.SetActive(true);
    }

    public void Restart()
    {
        GameManager.Instance.RestartGame();
    }

    public void ExitGame ()
    {
        GameManager.Instance.LoadMainMenu();
    }
}
