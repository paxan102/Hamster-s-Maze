using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [HideInInspector] public UnityEvent OnFinish = new UnityEvent();
    [SerializeField] private Transform middlePivot;
    [SerializeField] private float timeBetweenMoves;

    public Transform GetPivot()
    {
        return middlePivot;
    }

    public void InitAndSpawn(List<List<Cell>> cells, Direction directionOnStart)
    {
        if (!rb)
            rb = GetComponent<Rigidbody2D>();
        if (!animator)
            animator = GetComponent<Animator>();

        if (currentDirection != Direction.DOWN)
        {
            RotateOnDown();
            currentDirection = Direction.DOWN;
        }

        this.cells = cells;
        currentHeight = 0;
        currentWidth = 0;        
        gameObject.SetActive(true);
        gameObject.transform.position = cells[0][0].GetPointInWorld();
        //rb.MovePosition(cells[0][0].GetPointInWorld());

        if (directionOnStart == Direction.UP)
        {
            RotateOnUp();
            currentDirection = Direction.UP;
        }
        if (directionOnStart == Direction.RIGHT)
        {
            RotateOnRight();
            currentDirection = Direction.RIGHT;
        }

        Invoke(HANDLE_MOVE, timeBetweenMoves);
    }

    #region private

    private string HANDLE_MOVE = "HandleMove";
    private string ANIMATE_MOVE = "AnimateMove";
    private string INVOKE_ON_FINISH = "InvokeOnFinish";
    private string IS_MOVE = "IsMove";
    private Vector3 AXE_Z = new Vector3(0, 0, 1);
    private int ROTATE_ON_LEFT = 90;
    private int ROTATE_ON_RIGHT = -90;
    private int ROTATE_BACK = 180;

    private Rigidbody2D rb;
    private Animator animator;
    private List<List<Cell>> cells;
    private int currentHeight = 0;
    private int currentWidth = 0;
    private float currentDeltaY;
    private float currentDeltaX;
    private int currentFrameIdx = 0;
    private Direction currentDirection = Direction.DOWN;

    private void InvokeOnFinish()
    {
        gameObject.SetActive(false);
        OnFinish.Invoke();
    }

    private void HandleMove()
    {
        int dirY = 0;
        int dirX = 0;

        var directionBlocks = cells[currentHeight][currentWidth].GetDirectionBlocks(); //up, right, down, left

        if (Input.GetKey(KeyCode.UpArrow) && !directionBlocks[0])
            dirY += 1;
        if (Input.GetKey(KeyCode.RightArrow) && !directionBlocks[1])
            dirX += 1;
        if (Input.GetKey(KeyCode.DownArrow) && !directionBlocks[2])
            dirY -= 1;
        if (Input.GetKey(KeyCode.LeftArrow) && !directionBlocks[3])
            dirX -= 1;
        
        if ((dirX != 0 && dirY != 0) || (dirX == 0 && dirY == 0))
        {
            Invoke(HANDLE_MOVE, timeBetweenMoves);
            return;
        }

        if (dirY > 0)
        {            
            RotateOnUp();
            currentDirection = Direction.UP;
        }
        if (dirY < 0)
        {
            RotateOnDown();
            currentDirection = Direction.DOWN;            
        }
        if (dirX > 0)
        {
            RotateOnRight();
            currentDirection = Direction.RIGHT;            
        }
        if (dirX < 0)
        {
            RotateOnLeft();
            currentDirection = Direction.LEFT;          
        }

        Vector2 currentPoint = cells[currentHeight][currentWidth].GetPointInWorld();
        currentHeight += dirY; 
        currentWidth += dirX;
        var newPoint = cells[currentHeight][currentWidth].GetPointInWorld();

        currentDeltaY = (newPoint.y - currentPoint.y) / 8;
        currentDeltaX = (newPoint.x - currentPoint.x) / 8;

        Debug.Log(currentDeltaY);
        Debug.Log(currentDeltaX);

        animator.SetBool(IS_MOVE, true);
        AnimateMove();
    }

    private void AnimateMove()
    {
        var newY = gameObject.transform.position.y + currentDeltaY;
        var newX = gameObject.transform.position.x + currentDeltaX;
        rb.MovePosition(new Vector2(newX, newY));
        currentFrameIdx++;

        if(currentFrameIdx == 8)
        {
            currentFrameIdx = 0;
            currentDeltaY = 0;
            currentDeltaX = 0;
            animator.SetBool(IS_MOVE, false);

            if (cells[currentHeight][currentWidth].GetIsFinish())
            {
                Invoke(INVOKE_ON_FINISH, 0.2f);
                return;
            }

            Invoke(HANDLE_MOVE, timeBetweenMoves);
            return;
        }

        Invoke(ANIMATE_MOVE, 0.667f / 16);
    }

    private void RotateOnLeft()
    {
       if (currentDirection == Direction.UP)
            gameObject.transform.RotateAround(middlePivot.position, AXE_Z, ROTATE_ON_LEFT);
        if (currentDirection == Direction.DOWN)
            gameObject.transform.RotateAround(middlePivot.position, AXE_Z, ROTATE_ON_RIGHT);
        if (currentDirection == Direction.RIGHT)
            gameObject.transform.RotateAround(middlePivot.position, AXE_Z, ROTATE_BACK);
    }

    private void RotateOnRight()
    {
        if (currentDirection == Direction.UP)
            gameObject.transform.RotateAround(middlePivot.position, AXE_Z, ROTATE_ON_RIGHT);
        if (currentDirection == Direction.DOWN)
            gameObject.transform.RotateAround(middlePivot.position, AXE_Z, ROTATE_ON_LEFT);
        if (currentDirection == Direction.LEFT)
            gameObject.transform.RotateAround(middlePivot.position, AXE_Z, ROTATE_BACK);
    }

    private void RotateOnUp()
    {
        if (currentDirection == Direction.LEFT)
            gameObject.transform.RotateAround(middlePivot.position, AXE_Z, ROTATE_ON_RIGHT);
        if (currentDirection == Direction.RIGHT)
            gameObject.transform.RotateAround(middlePivot.position, AXE_Z, ROTATE_ON_LEFT);
        if (currentDirection == Direction.DOWN)
            gameObject.transform.RotateAround(middlePivot.position, AXE_Z, ROTATE_BACK);
    }

    private void RotateOnDown()
    {
        if (currentDirection == Direction.LEFT)
            gameObject.transform.RotateAround(middlePivot.position, AXE_Z, ROTATE_ON_LEFT);
        if (currentDirection == Direction.RIGHT)
            gameObject.transform.RotateAround(middlePivot.position, AXE_Z, ROTATE_ON_RIGHT);
        if (currentDirection == Direction.UP)
            gameObject.transform.RotateAround(middlePivot.position, AXE_Z, ROTATE_BACK);
    }

    #endregion
}
