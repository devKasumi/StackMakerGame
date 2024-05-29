using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [SerializeField] private LayerMask brickLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask pivotLayer;
    [SerializeField] private LayerMask topPivotLayer;
    [SerializeField] private LayerMask backpivotLayer;
    [SerializeField] private LayerMask leftPivotLayer;
    [SerializeField] private LayerMask rightPivotLayer;
    [SerializeField] private Brick brick;
    [SerializeField] private Transform player;
    //[SerializeField] private Transform frontWall;
    //[SerializeField] private Transform backWall;
    //[SerializeField] private Transform leftWall;
    //[SerializeField] private Transform rightWall;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxDistance = Mathf.Infinity;
    [SerializeField] private float maxDistanceForBrick = 5f;
    [SerializeField] private float maxDistanceForWall = 2f;

    private int axisDistance = 2;

    private Vector3 currentTargetPosition;

    private List<Brick> bricks = new List<Brick>();

    private bool isRaycastHitBrick;

    private Rigidbody rb;

    private Vector2 startTouchPoint;
    //private Vector2 

    private Direction currentDirection = Direction.None;

    private int brickCount = 0;

    private int raycastHitCount = 0;

    private bool isStopping = false;

    private Transform currentPivotPostion = null;

    private List<Transform> pivots = new List<Transform>();

    private bool isMoving = false;

    //private float storeMaxDistance = 0f;

    private Vector3 raycastPosition;

    private Transform currentBrickposition;

    private List<Transform> listBricks = new List<Transform>();

    private bool isFinishingDetect;

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
        //currentPivotPostion.position = transform.position;
        //listBricks = new List<Transform>(); 
        //raycastPosition = new Vector3(transform)
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            startTouchPoint = Input.mousePosition;   
        }

        if (Input.GetMouseButtonUp(0))
        {
            //if (currentTargetPosition != Vector3.zero)
            //{
            //    if (Vector3.Distance(transform.position, currentTargetPosition) < 0.02f)
            //    {
            //        Debug.Log("reach pivot position !!!");
            //    }
            //    else return;
            //}

            Vector2 currentMousePoint = Input.mousePosition;
            Vector2 distance = currentMousePoint - startTouchPoint;

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
                //isMoving = true;
            }
            else if (Mathf.Abs(distance.x) < Mathf.Abs(distance.y))
            {
                //Debug.Log("vuot len or xuong  !!!");
                if (currentMousePoint.y - startTouchPoint.y > 0)
                {
                    //Debug.LogError("vuot len tren !!!!");
                    //Debug.LogError("current direction: forward");
                    currentDirection = Direction.Forward;
                }
                else
                {
                    currentDirection = Direction.Backward;
                }
            }

            DirectionUsingRaycast(currentDirection);


            //if (DetectWall(currentDirection))
            //{
            //    Debug.Log("detect wall!!!");
            //    currentPivotPostion = null;
            //    //currentDirection = Direction.None;
            //    isMoving = true;
            //    //return;
            //}

            
        }

        //RaycastHit wallHit;
        //bool isWallDetected = Physics.Raycast(transform.position, Vector3.forward, out wallHit, maxDistance, wallLayer);
        //Debug.DrawLine(transform.position, transform.position + Vector3.forward * maxDistance, Color.red);
        //if (!isWallDetected && currentPivotPostion)
        //{
        //    currentPivotPostion = null;
        //    Debug.LogError("wall detectedddddd !!!!");
        //    DirectionUsingRaycast(currentDirection);
        //}






        //if (currentDirection != Direction.None)
        //{
        //    DirectionUsingRaycast(currentDirection);

        //}
        //transform.position = Vector3.MoveTowards(transform.position, currentBrickposition.position, moveSpeed * Time.deltaTime);    

        if (currentTargetPosition != Vector3.zero)
        {
            //Debug.Log("name:  " + currentPivotPostion.name.ToString());
            Move(currentTargetPosition);

        }
        //Move(currentTargetPosition);

        //if (isMoving)
        //{
        //    Debug.Log("player moving!!!!!");
        //    Move(currentDirection);
        //}
        //if (pivots.Count > 0)
        //{
        //    Move(pivots[0].position);
        //    pivots.Clear();
        //}

        //if (currentBrickposition)
        //{
        //    MoveToBrick(currentBrickposition.position);
        //}

        //RaycastOnBrick(currentDirection);

        //if (isFinishingDetect)
        //{
        //    if (listBricks.Count > 0)
        //    {
        //        Debug.LogError("moving base on list bricks !!!!");
        //        transform.position = Vector3.MoveTowards(transform.position, listBricks[listBricks.Count - 1].position, moveSpeed * Time.deltaTime);
        //        //listBricks.Clear(); 
        //    }
        //}



    }



    public void OnInit()
    {

    }

    public void Move(Vector3 pivotPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, pivotPosition, moveSpeed * Time.deltaTime);
        //isMoving = true;
        //isMoving = false;
    }

    public void MoveToBrick(Vector3 brickPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, brickPosition, moveSpeed * Time.deltaTime);

    }

    //public bool DetectWall(Direction direction)
    //{
    //    RaycastHit hit;

    //    switch (direction)
    //    {
    //        case Direction.Forward:
    //            return Physics.Raycast(transform.position, Vector3.forward, out hit, maxDistanceForWall, wallLayer);
    //        case Direction.Backward:
    //            return Physics.Raycast(transform.position, Vector3.back, out hit, maxDistanceForWall, wallLayer);
    //        case Direction.Left:
    //            return Physics.Raycast(transform.position, Vector3.left, out hit, maxDistanceForWall, wallLayer);
    //        case Direction.Right:
    //            return Physics.Raycast(transform.position, Vector3.right, out hit, maxDistanceForWall, wallLayer);
    //        default:
    //            return false;
    //    }
    //}

    public void DirectionUsingRaycast(Direction direction)
    {
        RaycastHit hit;

        //Debug.Log("is detect wall: " + DetectWall(direction) + " with current direction:  " + currentDirection );

        //if (DetectWall(currentDirection))
        //{
        //    //currentDirection = Direction.None;
        //    return;
        //}

        //if (DetectWall(direction)) return;



        switch (direction)
        {
            case Direction.Forward:
                if (Physics.Raycast(transform.position, Vector3.forward, out hit, maxDistance, topPivotLayer))
                {
                    Transform hitCollider = hit.collider.transform;
                    currentTargetPosition = new Vector3(hitCollider.position.x, hitCollider.position.y, hitCollider.position.z - axisDistance);
                }
                break;
            case Direction.Backward:
                if (Physics.Raycast(transform.position, Vector3.back, out hit, maxDistance, backpivotLayer))
                {
                    //currentPivotPostion = hit.collider.transform;
                    Transform hitCollider = hit.collider.transform;
                    currentTargetPosition = new Vector3(hitCollider.position.x, hitCollider.position.y, hitCollider.position.z + axisDistance);
                }
                break;
            case Direction.Left:
                if (Physics.Raycast(transform.position, Vector3.left, out hit, maxDistance, leftPivotLayer))
                {
                    //currentPivotPostion = hit.collider.transform;
                    Transform hitCollider = hit.collider.transform;
                    currentTargetPosition = new Vector3(hitCollider.position.x + axisDistance, hitCollider.position.y, hitCollider.position.z);
                }
                break;
            case Direction.Right:
                if (Physics.Raycast(transform.position, Vector3.right, out hit, maxDistance, rightPivotLayer))
                {
                    Transform hitCollider = hit.collider.transform;
                    currentTargetPosition = new Vector3(hitCollider.position.x - axisDistance, hitCollider.position.y, hitCollider.position.z);
                }
                break;
            default:
                //rb.velocity = Vector3.zero;
                break;
        }
    }

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


    private void OnTriggerEnter(Collider other)
    {
        //if (other.CompareTag("Wall"))
        //{
        //    rb.velocity = Vector3.zero;
        //    currentDirection = Direction.None;
        //}
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
