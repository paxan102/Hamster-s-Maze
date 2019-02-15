using System.Collections.Generic;
using UnityEngine;

class PathFinder
{
    public PathFinder(int mazeHeight, int mazeWidth, List<List<Vector2>> horizontalWalls, List<List<Vector2>> verticalWalls)
    {
        this.horizontalWallPoints = horizontalWalls;
        this.verticalWallPoints = verticalWalls;

        for(int heightIdx = 0; heightIdx < mazeHeight; heightIdx++)
        {
            cells.Add(new List<Cell>());

            for (int widthIdx = 0; widthIdx < mazeWidth; widthIdx++)
            { 
                var cell = new Cell();
                cell.Init(heightIdx, widthIdx);

                if (heightIdx == 0)
                    cell.BlockDown();
                if (heightIdx == mazeHeight - 1)
                    cell.BlockUp();
                if (widthIdx == 0)
                    cell.BlockLeft();
                if (widthIdx == mazeWidth - 1)
                    cell.BlockRight();

                cells[heightIdx].Add(cell);
            }
        }

        PassCell(0, 0);
    }

    public void MakeMaze()
    {
        int direction = Random.Range(0, 2);

        if (direction == (int)Direction.UP)
            NextStep(GoUp(cells[0][0]));
        if (direction == (int)Direction.RIGHT)
            NextStep(GoRight(cells[0][0]));
    }

    #region Private

    private List<List<Cell>> cells = new List<List<Cell>>();
    private List<List<Vector2>> horizontalWallPoints;
    private List<List<Vector2>> verticalWallPoints;

    private void NextStep(Cell cell)
    {
        if (cell.IsBlocked())
            return;

        int direction = Random.Range(0, 4);

        bool pass = false;
        while (!pass)
        {
            if (direction == (int)Direction.UP)
            {
                if (cell.GetDirectionsBlocks()[(int)Direction.UP])
                    direction = (int)Direction.RIGHT;
                else
                {
                    NextStep(GoUp(cell));
                    pass = true;
                }
            }

            if (direction == (int)Direction.RIGHT)
            {
                if (cell.GetDirectionsBlocks()[(int)Direction.RIGHT])
                    direction = (int)Direction.DOWN;
                else
                {
                    NextStep(GoRight(cell));
                    pass = true;
                }
            }

            if (direction == (int)Direction.DOWN)
            {
                if (cell.GetDirectionsBlocks()[(int)Direction.DOWN])
                    direction = (int)Direction.LEFT;
                else
                {
                    NextStep(GoDown(cell));
                    pass = true;
                }
            }

            if (direction == (int)Direction.LEFT)
            {
                if (cell.GetDirectionsBlocks()[(int)Direction.LEFT])
                    direction = (int)Direction.UP;
                else
                {
                    NextStep(GoLeft(cell));
                    pass = true;
                }
            }
        }
        
        if (cell.IsBlocked())
            return;

        NextStep(cell);
    }
    
    private Cell GoUp(Cell cell)
    {
        var height = cell.GetHeight();
        var width = cell.GetWidth();
        horizontalWallPoints[height][width] = Vector2.zero;
        PassCell(height + 1, width);
        return cells[height + 1][width];
    }

    private Cell GoDown(Cell cell)
    {
        var height = cell.GetHeight();
        var width = cell.GetWidth();
        horizontalWallPoints[height - 1][width] = Vector2.zero;
        PassCell(height - 1, width);
        return cells[height - 1][width];
    }
    
    private Cell GoRight(Cell cell)
    {
        var height = cell.GetHeight();
        var width = cell.GetWidth();
        verticalWallPoints[height][width] = Vector2.zero;
        PassCell(height, width + 1);
        return cells[height][width + 1];
    }

    private Cell GoLeft(Cell cell)
    {
        var height = cell.GetHeight();
        var width = cell.GetWidth();
        verticalWallPoints[height][width - 1] = Vector2.zero;
        PassCell(height, width - 1);
        return cells[height][width - 1];
    }

    private void PassCell(int height, int width)
    {
        if (height != 0)
            cells[height - 1][width].BlockUp();
        if (height != cells.Count - 1)
            cells[height + 1][width].BlockDown();
        if (width != 0)
            cells[height][width - 1].BlockRight();
        if (width != cells[height].Count - 1)
            cells[height][width + 1].BlockLeft();
    }

    #endregion
}

enum Direction
{
    UP,
    RIGHT,
    DOWN,
    LEFT
}