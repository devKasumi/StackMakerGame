using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private Bridge bridge;

    public Bridge GetBridge()
    {
        return bridge;
    }
}
