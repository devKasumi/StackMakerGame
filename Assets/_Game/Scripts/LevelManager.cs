using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private List<Level> levelPrefabs;

    private Level currentLevel;
    private int currentLevelIndex;
    private Vector3 playerPosition;

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
        playerPosition = player.transform.position;
        currentLevelIndex = 0;
        LoadLevel(currentLevelIndex);
    }

    public void LoadLevel(int levelIndex)
    {
        currentLevel = Instantiate(levelPrefabs[levelIndex]);
        currentLevel.GetComponent<Transform>().position = playerPosition;
        currentLevel.gameObject.SetActive(true);
    }

    public void OnDespawn()
    {
        OnInit();
    }

    public void Victory()
    {
        Debug.Log("finish levelllasdlasd");
        Destroy(currentLevel.gameObject);
        currentLevelIndex++; 
        LoadLevel(currentLevelIndex);
    }

    public void Lose()
    {

    }
}
