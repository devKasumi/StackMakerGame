using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    private const float CONST_MAXDISTANCE = Mathf.Infinity;
    private const int CONST_AXISDISTANCE = 2;

    //[SerializeField] private Bridge bridge;
    [SerializeField] private LayerMask pivotLayer;
    [SerializeField] private Transform player;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float originPlayerImagePos;
    [SerializeField] private float firstBrickPosition;

    private List<Bridge> bridges = new List<Bridge>();
    private List<Brick> bricks = new List<Brick>();
    private Quaternion forwardRotation = Quaternion.Euler(0f, -150f, 0f);
    private Quaternion backwardRotation = Quaternion.Euler(0f, 30f, 0f);
    private Quaternion leftRotation = Quaternion.Euler(0f, -240f, 0f);
    private Quaternion rightRotation = Quaternion.Euler(0f, -60f, 0f);
    private Vector3 originalPosition = Vector3.zero;
    private Vector3 currentTargetPosition;
    private Vector2 startTouchPoint;
    private Direction currentDirection = Direction.None;
    private bool isFinishingLevel = false;

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
        OnInit();
    }

    // Update is called once per frame
    void Update()
    {
        ControlPlayerMovement();
    }

    public void OnInit()
    {
        transform.position = originalPosition;
        player.position = originalPosition;
        player.rotation = forwardRotation;
        currentDirection = Direction.None;
        currentTargetPosition = Vector3.zero;
        isFinishingLevel = false;
        bricks = new List<Brick>();
    }

    public void OnDespawn()
    {
        ClearBrick();
        OnInit();
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
                    Debug.Log("reach pivot position !!!");
                    if (isFinishingLevel) return;
                }
                else return;
            }

            Debug.Log("switch direction !!!");

            Vector2 currentMousePoint = Input.mousePosition;
            Vector2 distance = currentMousePoint - startTouchPoint;

            if (Mathf.Abs(distance.x) > Mathf.Abs(distance.y))
            {
                if (currentMousePoint.x - startTouchPoint.x > 0)
                {
                    currentDirection = Direction.Right;
                    player.rotation = rightRotation;
                }
                else
                {
                    currentDirection = Direction.Left;
                    player.rotation = leftRotation;
                }
            }
            else if (Mathf.Abs(distance.x) < Mathf.Abs(distance.y))
            {
                if (currentMousePoint.y - startTouchPoint.y > 0)
                {
                    currentDirection = Direction.Forward;
                    player.rotation = forwardRotation;
                }
                else
                {
                    currentDirection = Direction.Backward;
                    player.rotation = backwardRotation;
                }
            }

            DirectionUsingRaycast(currentDirection);

        }

        if (currentTargetPosition != Vector3.zero)
        {
            Move(currentTargetPosition);
        }
    }

    public void Move(Vector3 pivotPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, pivotPosition, moveSpeed * Time.deltaTime);
    }

    public void DirectionUsingRaycast(Direction direction)
    {
        RaycastHit hit;

        switch (direction)
        {
            case Direction.Forward:
                if (Physics.Raycast(transform.position, Vector3.forward, out hit, CONST_MAXDISTANCE, pivotLayer))
                {
                    Transform hitCollider = hit.collider.transform;
                    currentTargetPosition = new Vector3(hitCollider.position.x, hitCollider.position.y, hitCollider.position.z - CONST_AXISDISTANCE);
                }
                break;
            case Direction.Backward:
                if (Physics.Raycast(transform.position, Vector3.back, out hit, CONST_MAXDISTANCE, pivotLayer))
                {
                    Transform hitCollider = hit.collider.transform;
                    currentTargetPosition = new Vector3(hitCollider.position.x, hitCollider.position.y, hitCollider.position.z + CONST_AXISDISTANCE);
                }
                break;
            case Direction.Left:
                if (Physics.Raycast(transform.position, Vector3.left, out hit, CONST_MAXDISTANCE, pivotLayer))
                {
                    Transform hitCollider = hit.collider.transform;
                    currentTargetPosition = new Vector3(hitCollider.position.x + CONST_AXISDISTANCE, hitCollider.position.y, hitCollider.position.z);
                }
                break;
            case Direction.Right:
                if (Physics.Raycast(transform.position, Vector3.right, out hit, CONST_MAXDISTANCE, pivotLayer))
                {
                    Transform hitCollider = hit.collider.transform;
                    currentTargetPosition = new Vector3(hitCollider.position.x - CONST_AXISDISTANCE, hitCollider.position.y, hitCollider.position.z);
                }
                break;
            default:
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
            Brick currentBrick = bricks[bricks.Count - 1];
            DestroyBrick(currentBrick);
            currentBrick.transform.parent = null;
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
        //if (bricks.Count > 0)
        //{
        //    for (int i = bricks.Count - 1; i >= 0; i--)
        //    {
        //        DestroyBrick(bricks[i]);
        //    }
        //}
        //bricks.Clear();
        //

        if (bricks.Count > 0)
        {
            for (int i = 0; i < bricks.Count; i++)
            {
                Destroy(bricks[i].gameObject);
            }
            bricks.Clear();
        }
    }

    public void DestroyBrick(Brick brick)
    {
        bricks.Remove(brick);
        Destroy(brick.gameObject);
    }

    public void FinishLevel()
    {
        isFinishingLevel = true;
    }

    public void AddBrige(Bridge bridge)
    {
        bridges.Add(bridge);
    }

    public void AddBrickForBridge(Brick brick)
    {
        bridges[LevelManager.GetInstance.GetCurrentLevelIndex()].AddBrick(brick); 
    }

    public bool NotEnoughBrickBridge()
    {
        return bridges[LevelManager.GetInstance.GetCurrentLevelIndex()].NotEnoughBrick();
    }
}
