using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishBox : MonoBehaviour
{
    //[SerializeField] private GameManager gameManager;
    //[SerializeField] private LevelManager levelManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("finish level!!!!!!!!");
            LevelManager.GetInstance.LoadLevel(0);
        }
    }
}
