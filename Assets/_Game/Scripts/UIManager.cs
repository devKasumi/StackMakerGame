using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
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

    public void OnInit()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
