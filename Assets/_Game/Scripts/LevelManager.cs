using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject[] levelPrefabs;

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

    public void OnInit()
    {
        currentLevelIndex = 0;
        LoadLevel(currentLevelIndex);
    }

    public void LoadLevel(int levelIndex)
    {
        //if (levelIndex > 0)
        //{
        //    Destroy(levelPrefabs[levelIndex - 1]);
        //}
        //DestroyImmediate(levelPrefabs[levelIndex], true);
        //Instantiate(levelPrefabs[levelIndex], Vector3.zero, Quaternion.identity);
    }

    public void OnDespawn()
    {
        OnInit();
    }
}
