using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    private IGameState gameState;

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

    //private void Start()
    //{
    //    OnInit();
    //}

    //public void OnInit()
    //{
    //    LevelManager.GetInstance.OnInit();
    //    UIManager.GetInstance.OnInit();
    //}

    //public void RestartLevel()
    //{

    //}

    //public void LoadNextLevel()
    //{

    //}
}
