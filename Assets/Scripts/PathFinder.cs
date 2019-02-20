using System.Collections.Generic;
using UnityEngine;

class PathFinder
{
    public PathFinder(int mazeHeight, int mazeWidth, List<List<Vector2>> horizontalWalls, List<List<Vector2>> verticalWalls)
    {
        horizontalWallPoints = horizontalWalls;
        verticalWallPoints = verticalWalls;

        for(int heightIdx = 0; heightIdx < mazeHeight; heightIdx++)
        {
            cellsForGen.Add(new List<Cell>());

            for (int widthIdx = 0; widthIdx < mazeWidth; widthIdx++)
            { 
                var cell = new Cell();

                if (heightIdx == 0)
                    cell.BlockDown();
                if (heightIdx == mazeHeight - 1)
                    cell.BlockUp();
                if (widthIdx == 0)
                    cell.BlockLeft();
                if (widthIdx == mazeWidth - 1)
                    cell.BlockRight();

                cellsForGen[heightIdx].Add(cell);
            }
        }

        for (int heightIdx = 0; heightIdx < mazeHeight * 2 - 1; heightIdx++)
        {
            cellsForPlayer.Add(new List<Cell>());

            for (int widthIdx = 0; widthIdx < mazeWidth * 2 - 1; widthIdx++)
            {
                var cell = new Cell();
                cell.BlockAll();
                cellsForPlayer[heightIdx].Add(cell);
            }
        }
    }

    public List<List<Cell>> MakeMaze()
    {
        PassCell(0, 0);

        int direction = Random.Range(0, 2);
        if (direction == (int)Direction.UP)
        {
            directionOnStart = Direction.UP;
            NextStep(GoUp(cellsForGen[currentHeight][currentWidth]));
        }
        if (direction == (int)Direction.RIGHT)
        {
            directionOnStart = Direction.RIGHT;
            NextStep(GoRight(cellsForGen[currentHeight][currentWidth]));
        }

        finishHeight *= 2;
        finishWidth *= 2;

        cellsForPlayer[finishHeight][finishWidth].SetIsFinish(true);

        return cellsForPlayer;
    }

    public Vector2 GetFinishCell()
    {
        return new Vector2(finishHeight, finishWidth);
    }

    public Direction GetDirectionOnStart()
    {
        return directionOnStart;
    }

    #region Private

    private List<List<Cell>> cellsForGen = new List<List<Cell>>();
    private List<List<Cell>> cellsForPlayer = new List<List<Cell>>();
    private List<List<Vector2>> horizontalWallPoints;
    private List<List<Vector2>> verticalWallPoints;
    private int currentHeight = 0;
    private int currentWidth = 0;
    private int currentLength = 0;
    private int maxLength = 0;
    private int finishHeight = 0;
    private int finishWidth = 0;
    private Direction directionOnStart;

    private void NextStep(Cell cell)
    {
        currentLength++;

        if (cell.IsBlocked())
        {
            if (maxLength < currentLength)
            {
                finishHeight = currentHeight;
                finishWidth = currentWidth;
                maxLength = currentLength;
            }
            return;
        }
                
        var height = currentHeight;
        var width = currentWidth;

        var directions = new List<int>();
        for(int idx = 0; idx < 4; idx++)
            if (!cell.GetDirectionBlocks()[idx])
                directions.Add(idx);

        int direction = directions[Random.Range(0, directions.Count)];

        bool pass = false;
        while (!pass)
        {
            if (direction == (int)Direction.UP)
            {
                if (cell.GetDirectionBlocks()[(int)Direction.UP])
                    direction = (int)Direction.RIGHT;
                else
                {
                    NextStep(GoUp(cell));
                    pass = true;
                }
            }

            if (direction == (int)Direction.RIGHT)
            {
                if (cell.GetDirectionBlocks()[(int)Direction.RIGHT])
                    direction = (int)Direction.DOWN;
                else
                {
                    NextStep(GoRight(cell));
                    pass = true;
                }
            }

            if (direction == (int)Direction.DOWN)
            {
                if (cell.GetDirectionBlocks()[(int)Direction.DOWN])
                    direction = (int)Direction.LEFT;
                else
                {
                    NextStep(GoDown(cell));
                    pass = true;
                }
            }

            if (direction == (int)Direction.LEFT)
            {
                if (cell.GetDirectionBlocks()[(int)Direction.LEFT])
                    direction = (int)Direction.UP;
                else
                {
                    NextStep(GoLeft(cell));
                    pass = true;
                }
            }
        }

        currentLength--;

        if (cell.IsBlocked())
            return;
        
        currentHeight = height;
        currentWidth = width;

        NextStep(cell);
    }

    private Cell GoUp(Cell cell)
    {
        horizontalWallPoints[currentHeight][currentWidth] = Vector2.zero;
        
        cellsForPlayer[currentHeight * 2][currentWidth * 2].UnblockUp();
        cellsForPlayer[currentHeight * 2 + 1][currentWidth * 2].UnblockDown();
        cellsForPlayer[currentHeight * 2 + 1][currentWidth * 2].UnblockUp();
        cellsForPlayer[currentHeight * 2 + 2][currentWidth * 2].UnblockDown();

        currentHeight++;
        PassCell(currentHeight, currentWidth);
        return cellsForGen[currentHeight][currentWidth];
    }

    private Cell GoDown(Cell cell)
    {
        horizontalWallPoints[currentHeight - 1][currentWidth] = Vector2.zero;

        cellsForPlayer[currentHeight * 2][currentWidth * 2].UnblockDown();
        cellsForPlayer[currentHeight * 2 - 1][currentWidth * 2].UnblockUp();
        cellsForPlayer[currentHeight * 2 - 1][currentWidth * 2].UnblockDown();
        cellsForPlayer[currentHeight * 2 - 2][currentWidth * 2].UnblockUp();

        currentHeight--;
        PassCell(currentHeight, currentWidth);
        return cellsForGen[currentHeight][currentWidth];
    }
    
    private Cell GoRight(Cell cell)
    {
        verticalWallPoints[currentHeight][currentWidth] = Vector2.zero;

        cellsForPlayer[currentHeight * 2][currentWidth * 2].UnblockRight();
        cellsForPlayer[currentHeight * 2][currentWidth * 2 + 1].UnblockLeft();
        cellsForPlayer[currentHeight * 2][currentWidth * 2 + 1].UnblockRight();
        cellsForPlayer[currentHeight * 2][currentWidth * 2 + 2].UnblockLeft();

        currentWidth++;
        PassCell(currentHeight, currentWidth);
        return cellsForGen[currentHeight][currentWidth];
    }

    private Cell GoLeft(Cell cell)
    {
        verticalWallPoints[currentHeight][currentWidth - 1] = Vector2.zero;

        cellsForPlayer[currentHeight * 2][currentWidth * 2].UnblockLeft();
        cellsForPlayer[currentHeight * 2][currentWidth * 2 - 1].UnblockRight();
        cellsForPlayer[currentHeight * 2][currentWidth * 2 - 1].UnblockLeft();
        cellsForPlayer[currentHeight * 2][currentWidth * 2 - 2].UnblockRight();

        currentWidth--;
        PassCell(currentHeight, currentWidth);
        return cellsForGen[currentHeight][currentWidth];
    }

    private void PassCell(int height, int width)
    {
        if (height != 0)
            cellsForGen[height - 1][width].BlockUp();
        if (height != cellsForGen.Count - 1)
            cellsForGen[height + 1][width].BlockDown();
        if (width != 0)
            cellsForGen[height][width - 1].BlockRight();
        if (width != cellsForGen[height].Count - 1)
            cellsForGen[height][width + 1].BlockLeft();
    }

    #endregion
}

public enum Direction
{
    UP,
    RIGHT,
    DOWN,
    LEFT
}