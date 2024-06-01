using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Canvas victoryCanvas;

    private static UIManager instance;

    public static UIManager GetInstance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType<UIManager>();
            }

            return instance;
        }
    }

    public void ShowVictoryUI()
    {
        victoryCanvas.gameObject.SetActive(true);
    }
}
