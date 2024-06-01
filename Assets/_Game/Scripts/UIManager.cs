using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Canvas victoryCanvas;
    [SerializeField] private Canvas loseCanvas;

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

    public void ShowLoseUI()
    {
        loseCanvas.gameObject.SetActive(true);
    }

    public void DisableUI()
    {
        victoryCanvas.gameObject.SetActive(false);
        loseCanvas.gameObject.SetActive(false);
    }
}
