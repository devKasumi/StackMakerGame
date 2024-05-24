using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [SerializeField] private LayerMask brickLayer;
    [SerializeField] private Brick brick;
    [SerializeField] private Transform player;
    [SerializeField] private Transform wall;
    [SerializeField] private float moveSpeed;

    private List<Brick> bricks = new List<Brick>();

    private bool isRaycastHitBrick;

    private Rigidbody rb;

    private Vector2 startTouchPoint;
    //private Vector2 

    private Direction currentDirection = Direction.None;

    private int brickCount = 0;

    private int raycastHitCount = 0;

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
        //Debug.LogError("current direction:  " + currentDirection.ToString());

        //checkRaycast();

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
                //Debug.Log("vuot trai or phai !!!!!");
                if (currentMousePoint.x - startTouchPoint.x > 0)
                {
                    //Debug.LogError("vuot sang phai!!!!!!");
                    currentDirection = Direction.Right;
                }
                else
                {
                    //Debug.LogError("vuot sang trai !!!!!");
                    currentDirection = Direction.Left;
                }
            }
            else if (Mathf.Abs(distance.x) < Mathf.Abs(distance.y))
            {
                //Debug.Log("vuot len or xuong  !!!");
                if (currentMousePoint.y - startTouchPoint.y > 0)
                {
                    //Debug.LogError("vuot len tren !!!!");
                    currentDirection = Direction.Forward;
                }
                else
                {
                    //Debug.LogError("vuot xuong duoi !!!!!");
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
                //rb.velocity = new Vector3(0f, 0f, 1.5f);
                transform.position = Vector3.MoveTowards(transform.position, wall.position, moveSpeed*Time.deltaTime);
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
        //brickCount++;
        Debug.LogError("current brick count: " + brickCount);
        bricks.Add(Instantiate(brick, transform.position, transform.rotation));
    }

    public void RemoveBrick()
    {

    }

    public void ClearBrick()
    {

    }

    private void checkRaycast()
    {
        RaycastHit hit;
        //Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.1f, Color.red);
        if (Physics.Raycast(transform.position, Vector3.down, out hit,5f, brickLayer)) 
        {
            //Debug.Log("raycast hit xDDDD");
            //Debug.Log(hit.collider);
            raycastHitCount++;
            Debug.Log("raycastHit Count:   " + raycastHitCount);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            rb.velocity = Vector3.zero;
            currentDirection = Direction.None;
        }
        if (other.CompareTag("Brick"))
        {
            Vector3 newPos = player.position;
            newPos.y += 0.5f;
            player.position = newPos;
            //AddBrick();
            Transform t = other.transform;
            gameObject.tag = "Untagged";
            t.SetParent(this.transform);
            t.localPosition = new Vector3(0, brickCount * 0.3f, 0);
            //Destroy(other.gameObject);
            brickCount++;
        }
    }
}
