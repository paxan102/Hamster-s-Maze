using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [HideInInspector] public UnityEvent OnFinish = new UnityEvent();
    [SerializeField] private float timeBetweenMoves;

    public void InitAndSpawn(List<List<Cell>> cells)
    {
        if (!rb)
            rb = GetComponent<Rigidbody2D>();

        this.cells = cells;
        currentHeight = 0;
        currentWidth = 0;        
        gameObject.SetActive(true);
        rb.MovePosition(cells[0][0].GetPointInWorld());

        Invoke(MOVE, timeBetweenMoves);
    }

    #region private

    private string MOVE = "Move";
    private string INVOKE_ON_FINISH = "InvokeOnFinish";

    private Rigidbody2D rb;
    private List<List<Cell>> cells;
    private int currentHeight = 0;
    private int currentWidth = 0;

    private void InvokeOnFinish()
    {
        gameObject.SetActive(false);
        OnFinish.Invoke();
    }

    private void Move()
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
            Invoke(MOVE, timeBetweenMoves);
            return;
        }

        currentHeight += dirY;
        currentWidth += dirX;
        var newPoint = cells[currentHeight][currentWidth].GetPointInWorld();
        rb.MovePosition(newPoint);

        if (cells[currentHeight][currentWidth].GetIsFinish())
        {
            Invoke(INVOKE_ON_FINISH, 0.2f);
            return;
        }

        Invoke(MOVE, timeBetweenMoves);
    }

    #endregion
}
