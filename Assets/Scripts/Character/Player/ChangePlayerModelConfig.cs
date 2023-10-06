using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlayerModelConfig : MonoBehaviour
{
    public MainMenuCanvas canvas;
    private PlayerModelConfig currentActivePlayerModel;
    public PlayerModelConfig[] playerModels;

    private void Start()
    {
        UpdatePlayerModel();
    }

    public void UpdatePlayerModel()
    {
        if(currentActivePlayerModel==null&&playerModels!=null&&playerModels.Length>0)
        {
            currentActivePlayerModel = playerModels[0];
        }

        foreach (PlayerModelConfig modelConfig in playerModels)
        {
            if (GameManager.Instance.GetSelectedPlayerModelType() == modelConfig.playerModel)
            {
                currentActivePlayerModel.gameObject.SetActive(false);
                modelConfig.gameObject.SetActive(true);
                currentActivePlayerModel = modelConfig;
            }
        }
        if (canvas!=null)
        {
            canvas.UpdateCharacterDisplayName(currentActivePlayerModel.ModelDisplayName);
        }
    }

    public void NextPlayerModel()
    {
        PlayerModelType currentPlayerModel = GameManager.Instance.GetSelectedPlayerModelType();
        if (PlayerModelType.SURVIVOR_04 == currentPlayerModel)
        {
            currentPlayerModel = PlayerModelType.BIOLOGIC_RISC;
        }
        else
        {
            currentPlayerModel++;
        }
        GameManager.Instance.SetSelectedPlayerModelType(currentPlayerModel);
        UpdatePlayerModel();
    }

    public void PreviousPlayerModel()
    {
        PlayerModelType currentPlayerModel = GameManager.Instance.GetSelectedPlayerModelType();
        if (PlayerModelType.BIOLOGIC_RISC == currentPlayerModel)
        {
            currentPlayerModel = PlayerModelType.SURVIVOR_04;
        }
        else
        {
            currentPlayerModel--;
        }
        GameManager.Instance.SetSelectedPlayerModelType(currentPlayerModel);
        UpdatePlayerModel();
    }
}
