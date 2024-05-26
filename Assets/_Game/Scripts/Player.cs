using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [SerializeField] private LayerMask brickLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask pivotLayer;
    [SerializeField] private Brick brick;
    [SerializeField] private Transform player;
    //[SerializeField] private Transform frontWall;
    //[SerializeField] private Transform backWall;
    //[SerializeField] private Transform leftWall;
    //[SerializeField] private Transform rightWall;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxDistance = 0f;

    private List<Brick> bricks = new List<Brick>();

    private bool isRaycastHitBrick;

    private Rigidbody rb;

    private Vector2 startTouchPoint;
    //private Vector2 

    private Direction currentDirection = Direction.None;

    private int brickCount = 0;

    private int raycastHitCount = 0;

    private bool isStopping = false;

    private Transform currentPivotPostion;

    private List<Transform> pivots = new List<Transform>();

    private bool isMoving = false;

    //private float storeMaxDistance = 0f;

    private Vector3 raycastPosition;

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
        //raycastPosition = new Vector3(transform)
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.LogError("current direction:  " + currentDirection.ToString());

        //if (isStopping) return;

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
                    DirectionUsingRaycast(currentDirection);
                    if (DetectWall(currentDirection)) return;

                }
                else
                {
                    //Debug.LogError("vuot sang trai !!!!!");
                    currentDirection = Direction.Left;
                    DirectionUsingRaycast(currentDirection);
                    if (DetectWall(currentDirection)) return;

                }
            }
            else if (Mathf.Abs(distance.x) < Mathf.Abs(distance.y))
            {
                //Debug.Log("vuot len or xuong  !!!");
                if (currentMousePoint.y - startTouchPoint.y > 0)
                {
                    //Debug.LogError("vuot len tren !!!!");
                    currentDirection = Direction.Forward;
                    DirectionUsingRaycast(currentDirection);
                    if (DetectWall(currentDirection)) return;
                }
                else
                {
                    //Debug.LogError("vuot xuong duoi !!!!!");
                    currentDirection = Direction.Backward;
                    DirectionUsingRaycast(currentDirection);
                    if (DetectWall(currentDirection)) return;
                }
            }
        }

        //Control();
        if (currentPivotPostion)
        {
            Move(currentPivotPostion.position);
        }
        //if (pivots.Count > 0)
        //{
        //    Move(pivots[0].position);
        //    pivots.Clear();
        //}
    }



    public void OnInit()
    {

    }

    public void Move(Vector3 pivotPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, pivotPosition, moveSpeed * Time.deltaTime);
        //isMoving = true;    
    }

    public bool DetectWall(Direction direction)
    {
        RaycastHit hit;

        switch (direction)
        {
            case Direction.Forward:
                return Physics.Raycast(transform.position, Vector3.forward, out hit, maxDistance, wallLayer);
            case Direction.Backward:
                return Physics.Raycast(transform.position, Vector3.back, out hit, maxDistance, wallLayer);
            case Direction.Left:
                return Physics.Raycast(transform.position, Vector3.left, out hit, maxDistance, wallLayer);
            case Direction.Right:
                return Physics.Raycast(transform.position, Vector3.right, out hit, maxDistance, wallLayer);
            default:
                return false;
        }
    }

    //public void CheckBrickRaycast(Direction direction)
    //{
    //    RaycastHit hit;

    //    switch (direction)
    //    {
    //        case Direction.Forward:
    //            if (Physics.Raycast(transform.position.y))
    //        case Direction.Backward:
    //        case Direction.Left:
    //        case Direction.Right:
    //        default:
    //            break;
    //    }
    //}

    public void DirectionUsingRaycast(Direction direction)
    {
        RaycastHit hit;

        Debug.Log("is detect wall: " + DetectWall(direction) + " with current direction:  " + currentDirection );

        //if (DetectWall(currentDirection))
        //{
        //    //currentDirection = Direction.None;
        //    return;
        //}


        switch (direction)
        {
            case Direction.Forward:
                if (Physics.Raycast(transform.position, Vector3.forward, out hit, maxDistance, pivotLayer))
                {
                    //Debug.DrawLine(new Vector3(transform.position.x, 0f, transform.position.z), transform.position + Vector3.forward * maxDistance, Color.red);
                    //Debug.Log(hit.collider.transform.position);
                    //pivots.Add(hit.collider.transform);
                    currentPivotPostion = hit.collider.transform;
                    //Move(direction, hit.collider.transform.position);
                }
                break;
            case Direction.Backward:
                if (Physics.Raycast(transform.position, Vector3.back, out hit, maxDistance, pivotLayer))
                {
                    //Move(direction, hit.collider.transform.position);
                    //pivots.Add(hit.collider.transform);
                    currentPivotPostion = hit.collider.transform;
                }
                break;
            case Direction.Left:
                if (Physics.Raycast(transform.position, Vector3.left, out hit, maxDistance, pivotLayer))
                {
                    //Move(direction, hit.collider.transform.position);
                    //pivots.Add(hit.collider.transform);
                    currentPivotPostion = hit.collider.transform;
                }
                break;
            case Direction.Right:
                if (Physics.Raycast(transform.position, Vector3.right, out hit, maxDistance, pivotLayer))
                {
                    //Move(direction, hit.collider.transform.position);
                    //pivots.Add(hit.collider.transform);
                    currentPivotPostion = hit.collider.transform;
                }
                break;
            default:
                rb.velocity = Vector3.zero;
                break;
        }
    }

    //public void Control()
    //{
    //    switch (currentDirection)
    //    {
    //        case Direction.Forward:
    //            //rb.velocity = new Vector3(0f, 0f, 1.5f);
    //            transform.position = Vector3.MoveTowards(transform.position, frontWall.position, moveSpeed*Time.deltaTime);
    //            //transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, storeMaxDistance), moveSpeed * Time.deltaTime);
    //            break;
    //        case Direction.Backward:
    //            //rb.velocity = new Vector3(0f, 0f, -1.5f);
    //            transform.position = Vector3.MoveTowards(transform.position, backWall.position, moveSpeed * Time.deltaTime);
    //            //transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, storeMaxDistance), moveSpeed * Time.deltaTime);
    //            break;
    //        case Direction.Right:
    //            //rb.velocity = new Vector3(1.5f, 0f, 0f);
    //            transform.position = Vector3.MoveTowards(transform.position, rightWall.position, moveSpeed * Time.deltaTime);
    //            break;
    //        case Direction.Left:
    //            //rb.velocity = new Vector3(-1.5f, 0f, 0f);
    //            transform.position = Vector3.MoveTowards(transform.position, leftWall.position, moveSpeed * Time.deltaTime);
    //            break;
    //        default:
    //            rb.velocity = Vector3.zero;
    //            break;
    //    }
    //}

    public void AddBrick()
    {
        //brickCount++;
        //Debug.LogError("current brick count: " + brickCount);
        bricks.Add(Instantiate(brick, transform.position, transform.rotation));
    }

    public void RemoveBrick()
    {

    }

    public void ClearBrick()
    {

    }

    //private void checkRaycast()
    //{
    //    RaycastHit hit;
    //    //Debug.DrawRay(transform.position, Vector3.forward, Color.green);
    //    //int maxDistance = 1;
    //    Debug.DrawLine(transform.position, transform.position + Vector3.forward * maxDistance, Color.red);
    //    if (Physics.Raycast(transform.position, Vector3.forward, out hit, maxDistance, wallLayer))
    //    {
    //        //Debug.Log("raycast hit xDDDD");
    //        //transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, maxDistance), moveSpeed * Time.deltaTime);
    //        Debug.Log(hit.collider.name);
    //        //storeMaxDistance = maxDistance;
    //        maxDistance = 0f;
    //        //currentDirection = Direction.Forward;
    //    }
    //    else
    //    {
    //        Debug.LogError("increase maxDistance:   " + maxDistance);
    //        maxDistance++;
    //    }
    //} 

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
