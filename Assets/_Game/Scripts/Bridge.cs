using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    [SerializeField] private Brick[] bricks;

    private List<Brick> currentBricksList = new List<Brick>();

    public void AddBrick(Brick brick)
    {
        currentBricksList.Add(brick);
    }

    public bool NotEnoughBrick()
    {
        return currentBricksList.Count < bricks.Length;
    }
}
