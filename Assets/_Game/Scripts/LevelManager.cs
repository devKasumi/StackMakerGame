using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private List<Level> levelPrefabs;

    //private Transform originPosition = new Vector3(-11.2241859f, 3.28916001f, -21.0589981f);

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
        //currentLevel = Instantiate(levelPrefabs[currentLevelIndex]);
        //currentLevel.GetComponent<Transform>().position = playerPosition;
        //currentLevel.gameObject.SetActive(true);
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
        //Destroy(levelPrefabs[currentLevelIndex].gameObject);
        //Destroy(currentLevel);
        Debug.Log("finish levelllasdlasd");
        //currentLevel.gameObject.SetActive(false);
        Destroy(currentLevel.gameObject);
        currentLevelIndex++; 
        //currentLevel = levelPrefabs[currentLevelIndex];
        LoadLevel(currentLevelIndex);
    }

    public void Lose()
    {

    }
}
