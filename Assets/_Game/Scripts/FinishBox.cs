using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("finish level!!!!!!!!");
            other.GetComponent<Player>().FinishLevel();
            UIManager.GetInstance.ShowVictoryUI();
        }
    }
}
