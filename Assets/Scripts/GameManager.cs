using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public const string GAME_MANAGER_NAME = "GameManager";
    private const string GAME_SCENE = "Game";
    private const string MAIN_MENU_SCENE = "MainMenu";
    private const float SCENE_TRANSITION_TIME = 0.2f;
    private const string SELECTED_PLAYER_MODEL_TYPE = "SelectedPlayerModel";

    public static GameManager Instance;

    void Awake()
    {
        Instance = this;
    }

    public void LoadMainMenu()
    {
        StartCoroutine(SceneTransition(MAIN_MENU_SCENE));
        ResumeGame();
    }

    public void StartGame()
    {
        StartCoroutine(SceneTransition(GAME_SCENE));
    }

    public void RestartGame()
    {
        StartGame();
    }

    IEnumerator SceneTransition (string sceneName)
    {
        yield return new WaitForSecondsRealtime(SCENE_TRANSITION_TIME);
        SceneManager.LoadScene(sceneName);
    }

    public void ExitApplication()
    {
        StartCoroutine(ExitApplicationWithDelay());
    }

    IEnumerator ExitApplicationWithDelay()
    {
        yield return new WaitForSecondsRealtime(SCENE_TRANSITION_TIME);
        Application.Quit();
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public PlayerModelType GetSelectedPlayerModelType()
    {
        return (PlayerModelType) PlayerPrefs.GetInt(SELECTED_PLAYER_MODEL_TYPE);
    }

    public void SetSelectedPlayerModelType(PlayerModelType modelType)
    {
        PlayerPrefs.SetInt(SELECTED_PLAYER_MODEL_TYPE, (int) modelType);
    }
}
