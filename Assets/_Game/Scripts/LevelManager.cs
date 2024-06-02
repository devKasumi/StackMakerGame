using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<Level> levelPrefabs;
    [SerializeField] private Player player;

    private Level currentLevel;
    private Vector3 playerPosition;
    private int currentLevelIndex;

    private static LevelManager instance;

    public static LevelManager GetInstance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType<LevelManager>();
            }

            return instance;
        }
    }

    private void Start()
    {
        OnInit();
    }

    public void OnInit()
    {
        Debug.Log("init level manager");
        playerPosition = Vector3.zero;
        currentLevelIndex = 0;
        InitLevel(currentLevelIndex);
    }

    public void OnDespawn()
    {
        OnInit();
    }

    public void InitLevel(int levelIndex)
    {
        currentLevel = Instantiate(levelPrefabs[levelIndex]);
        currentLevel.GetComponent<Transform>().position = playerPosition;
        currentLevel.gameObject.SetActive(true);
        player.AddBrige(currentLevel.GetBridge());
    }

    public void ReplayCurrentLevel()
    {
        LoadLevel();    
    }

    public void LoadNextLevel()
    {
        currentLevelIndex++;
        LoadLevel();
    }

    public void LoadLevel()
    {
        Destroy(currentLevel.gameObject);
        InitLevel(currentLevelIndex);
    }

    public void RestartLevel()
    {
        Destroy(currentLevel.gameObject);
        OnInit();
    }

    public int GetCurrentLevelIndex()
    {
        return currentLevelIndex;
    }
}
