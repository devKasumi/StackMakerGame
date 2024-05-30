using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    //[SerializeField] private LayerMask brickLayer;
    //[SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask pivotLayer;
    [SerializeField] private Brick brick;
    [SerializeField] private Transform player;
    [SerializeField] private float moveSpeed;

    [SerializeField] private float originPlayerImagePos;
    [SerializeField] private float firstBrickPosition;

    [SerializeField] private GameObject bridge;
    private float maxDistance = Mathf.Infinity;

    private int axisDistance = 2;

    private Vector3 currentTargetPosition;

    private List<Brick> bricks = new List<Brick>();

    private Rigidbody rb;

    private Vector2 startTouchPoint;

    private Direction currentDirection = Direction.None;

    //[SerializeField] private bool isStoping;

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
        ControlPlayerMovement();
    }

    public void OnInit()
    {

    }

    public void ControlPlayerMovement()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startTouchPoint = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (currentTargetPosition != Vector3.zero)
            {
                if (Vector3.Distance(transform.position, currentTargetPosition) < 0.02f)
                {
                    //Debug.Log("reach pivot position !!!");
                }
                else return;
            }

            Vector2 currentMousePoint = Input.mousePosition;
            Vector2 distance = currentMousePoint - startTouchPoint;

            if (Mathf.Abs(distance.x) > Mathf.Abs(distance.y))
            {
                if (currentMousePoint.x - startTouchPoint.x > 0)
                {
                    currentDirection = Direction.Right;
                }
                else
                {
                    currentDirection = Direction.Left;
                }
            }
            else if (Mathf.Abs(distance.x) < Mathf.Abs(distance.y))
            {
                //Debug.Log("vuot len or xuong  !!!");
                if (currentMousePoint.y - startTouchPoint.y > 0)
                {
                    currentDirection = Direction.Forward;
                }
                else
                {
                    currentDirection = Direction.Backward;
                }
            }

            Debug.Log(currentDirection.ToString());

            DirectionUsingRaycast(currentDirection);

        }

        if (currentTargetPosition != Vector3.zero)
        {
            Move(currentTargetPosition);
        }
    }

    public void Move(Vector3 pivotPosition)
    {
        //Debug.Log("movinggggg");
        transform.position = Vector3.MoveTowards(transform.position, pivotPosition, moveSpeed * Time.deltaTime);
    }

    public void DirectionUsingRaycast(Direction direction)
    {
        RaycastHit hit;

        //Debug.DrawLine(transform.position, transform.position + Vector3.forward * maxDistance, Color.red);

        switch (direction)
        {
            case Direction.Forward:
                if (Physics.Raycast(transform.position, Vector3.forward, out hit, maxDistance, pivotLayer))
                {
                    //Debug.Log("pokemon xDDDD");
                    Transform hitCollider = hit.collider.transform;
                    currentTargetPosition = new Vector3(hitCollider.position.x, hitCollider.position.y, hitCollider.position.z - axisDistance);
                }
                break;
            case Direction.Backward:
                //Debug.Log("chelsea");
                if (Physics.Raycast(transform.position, Vector3.back, out hit, maxDistance, pivotLayer))
                {
                    //Debug.Log("backkkkk");
                    //currentPivotPostion = hit.collider.transform;
                    Transform hitCollider = hit.collider.transform;
                    currentTargetPosition = new Vector3(hitCollider.position.x, hitCollider.position.y, hitCollider.position.z + axisDistance);
                }
                break;
            case Direction.Left:
                if (Physics.Raycast(transform.position, Vector3.left, out hit, maxDistance, pivotLayer))
                {
                    //currentPivotPostion = hit.collider.transform;
                    Transform hitCollider = hit.collider.transform;
                    currentTargetPosition = new Vector3(hitCollider.position.x + axisDistance, hitCollider.position.y, hitCollider.position.z);
                }
                break;
            case Direction.Right:
                if (Physics.Raycast(transform.position, Vector3.right, out hit, maxDistance, pivotLayer))
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

    public void AddBrick(Brick brick)
    {
        bricks.Add(brick);
        StackBrick();
    }

    public void StackBrick()
    {
        Vector3 newPos = player.position;
        Transform brickTransform = bricks[bricks.Count - 1].transform;
        brickTransform.SetParent(this.transform);
        if (bricks.Count == 1)
        {
            newPos.y = originPlayerImagePos;
            brickTransform.localPosition = new Vector3(0f, firstBrickPosition, 0f);
        }
        else
        {
            newPos.y += 0.3f;
            brickTransform.localPosition = new Vector3(0f, firstBrickPosition + (bricks.Count - 1) * 0.3f, 0f);
        }
        player.position = newPos;

    }

    public void RemoveBrick()
    {
        if (bricks.Count > 0)
        {
            Debug.Log(bricks[bricks.Count - 1].gameObject.name.ToString());
            Brick currentBrick = bricks[bricks.Count - 1];
            bricks.Remove(currentBrick);
            currentBrick.transform.parent = null;
            Destroy(currentBrick.gameObject);
            Debug.LogError("count:  " + bricks.Count);
            Vector3 newPos = player.position;
            newPos.y -= 0.3f;
            player.position = newPos;
        } 
    }

    public int GetListBrickCount()
    {
        return bricks.Count;
    }

    public void StopMoving(Brick brick)
    {
        Vector3 newPos = brick.transform.position;
        newPos.y += 1f;
        currentTargetPosition = newPos;
    }

    public void ClearBrick()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        
    }
}
