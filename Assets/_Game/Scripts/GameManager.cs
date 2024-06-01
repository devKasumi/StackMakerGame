using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    private IGameState gameState;

    [SerializeField] private Player player;

    public static GameManager GetInstance
    {
        get
        {
            if (!instance) 
            {
                instance = FindObjectOfType<GameManager>();
            }

            return instance;
        }        
    }


    public void ReplayLevel()
    {
        LevelManager.GetInstance.ReplayCurrentLevel();
        UIManager.GetInstance.DisableUI();
        player.OnDespawn();
    }

    public void LoadNextLevel()
    {
        LevelManager.GetInstance.LoadNextLevel();
        UIManager.GetInstance.DisableUI();
        player.OnDespawn();
    }

    public void RestartGame()
    {
        LevelManager.GetInstance.OnDespawn();
        UIManager.GetInstance.DisableUI();
        player.OnDespawn();
    }
}
