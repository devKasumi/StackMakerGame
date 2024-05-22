using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody rb;

    private Vector2 startTouchPoint;
    //private Vector2 

    private Direction currentDirection = Direction.None;

    public enum Direction
    {
        None,
        Forward,
        Backward,
        Left,
        Right,
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.LogError("current direction:  " + currentDirection.ToString());


        if (Input.GetMouseButtonDown(0))
        {
            //rb.velocity = new Vector3(1.5f, 1.5f, 0f);
            //Debug.Log(Input.mousePosition.ToString());
            startTouchPoint = Input.mousePosition;
            //Debug.Log("first touch point:  " + startTouchPoint.ToString());
        }
        if (Input.GetMouseButtonUp(0))
        {
            //rb.velocity = Vector3.zero;
            Vector2 currentMousePoint = Input.mousePosition;
            Vector2 distance = currentMousePoint - startTouchPoint;
            //Debug.LogError("distance with y axis:  " + Mathf.Abs(distance.y) + "   x:  " + Mathf.Abs(distance.x));
            //rb.velocity = new Vector3(0f, 0f, 1.5f);
            if (Mathf.Abs(distance.x) > Mathf.Abs(distance.y))
            {
                Debug.Log("vuot trai or phai !!!!!");
                if (currentMousePoint.x - startTouchPoint.x > 0)
                {
                    Debug.LogError("vuot sang phai!!!!!!");
                    currentDirection = Direction.Right;
                }
                else
                {
                    Debug.LogError("vuot sang trai !!!!!");
                    currentDirection = Direction.Left;
                }
            }
            else if (Mathf.Abs(distance.x) < Mathf.Abs(distance.y))
            {
                Debug.Log("vuot len or xuong  !!!");
                if (currentMousePoint.y - startTouchPoint.y > 0)
                {
                    Debug.LogError("vuot len tren !!!!");
                    currentDirection = Direction.Forward;
                }
                else
                {
                    Debug.LogError("vuot xuong duoi !!!!!");
                    currentDirection = Direction.Backward;
                }
            }
        }

        Control();
    }

    

    public void OnInit()
    {

    }

    public void Control()
    {
        switch (currentDirection)
        {
            case Direction.Forward:
                rb.velocity = new Vector3(0f, 0f, 1.5f);
                break;
            case Direction.Backward:
                rb.velocity = new Vector3(0f, 0f, -1.5f);
                break;
            case Direction.Right:
                rb.velocity = new Vector3(1.5f, 0f, 0f);
                break;
            case Direction.Left:
                rb.velocity = new Vector3(-1.5f, 0f, 0f);
                break;
            default:
                rb.velocity = Vector3.zero;
                break;
        }
    }

    public void AddBrick()
    {

    }

    public void RemoveBrick()
    {

    }

    public void ClearBrick()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            rb.velocity = Vector3.zero;
        }
    }
}
