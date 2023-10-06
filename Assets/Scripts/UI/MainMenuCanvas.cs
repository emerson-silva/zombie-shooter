using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuCanvas : MonoBehaviour
{
    public Text characterName;
    public GameObject exitApplicationButton;

    private void Start()
    {
        #if UNITY_STANDALONE || UNITY_EDITOR
            exitApplicationButton.SetActive(true);
        #endif
    }

    public void StartGame()
    {
        GameManager.Instance.StartGame();
    }

    public void ExitApplication()
    {
        GameManager.Instance.ExitApplication();
    }

    public void UpdateCharacterDisplayName(string displayName)
    {
        characterName.text = displayName;
    }
}
