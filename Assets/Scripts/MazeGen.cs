using System.Collections.Generic;
using UnityEngine;

public class MazeGen : MonoBehaviour
{
    #region Inspector's variables

    [SerializeField] private SpriteRenderer horizontalWallPref;
    [SerializeField] private SpriteRenderer crossWallPref;
    [SerializeField] private SpriteRenderer start;
    [SerializeField] private SpriteRenderer finish;
    [SerializeField] private int mazeHeight;
    [SerializeField] private int mazeWidth;

    #endregion

    #region Public

    public void Init()
    {
        widthOfWall = horizontalWallPref.size.x * horizontalWallPref.transform.localScale.x;
        widthOfCrossWall = crossWallPref.size.x * crossWallPref.transform.localScale.x;
    }

    public float GetMazeHeight()
    {
        return mazeHeight * (widthOfWall + widthOfCrossWall) + widthOfCrossWall;
    }

    public float GetMazeWidth()
    {
        return mazeWidth * (widthOfWall + widthOfCrossWall) + widthOfCrossWall;
    }

    public List<List<Cell>> GetCellsForPlayer()
    {
        return cellsForPlayer;
    }

    public void MakeNewMaze()
    {
        horizontalWallPoints = new List<List<Vector2>>();
        verticalWallPoints = new List<List<Vector2>>();
        cellsForPlayer = new List<List<Cell>>();
                
        MakeMazePatternAndAddWallPointsInLists();
        var pathFinder = new PathFinder(mazeHeight, mazeWidth, horizontalWallPoints, verticalWallPoints);
        cellsForPlayer = pathFinder.MakeMaze();
        AddWorldPointsToCells();
        MakeWallsFromLists();

        MakeStart();
        MakeFinish(pathFinder.GetFinishCell());
    }

    #endregion

    #region Private

    private float widthOfWall;
    private float widthOfCrossWall;
    private List<List<Vector2>> horizontalWallPoints;
    private List<List<Vector2>> verticalWallPoints;
    private List<List<Cell>> cellsForPlayer;
    private Vector2 spawnPlayerCoord;

    private void MakeStart()
    {
        Instantiate(start, CalculatePoint(TypeOfCell.EMPTY_CELL, 0, 0), new Quaternion(), transform);
    }

    private void MakeFinish(Vector2 cellCoord)
    {
        var y = cellCoord.y;
        var x = cellCoord.x;

        Instantiate(finish, CalculatePoint(TypeOfCell.EMPTY_CELL, y, x), new Quaternion(), transform);
    }

    private void AddWorldPointsToCells()
    {
        for(int heightIdx = 0; heightIdx < cellsForPlayer.Count; heightIdx++)
        {
            for (int widthIdx = 0; widthIdx < cellsForPlayer[heightIdx].Count; widthIdx++)
            {
                cellsForPlayer[heightIdx][widthIdx].SetPointInWorld(CalculatePoint(TypeOfCell.EMPTY_CELL, widthIdx, heightIdx));
            }
        }
    }

    private void MakeWallsFromLists()
    {
        foreach (var wallPoints in horizontalWallPoints)
        {
            foreach (var wallPoint in wallPoints)
            {
                if (wallPoint != Vector2.zero)
                    MakeWall(wallPoint);
            }
        }

        foreach (var wallPoints in verticalWallPoints)
        {
            foreach (var wallPoint in wallPoints)
            {
                if (wallPoint != Vector2.zero)
                    MakeWall(wallPoint);
            }
        }
    }

    private void MakeMazePatternAndAddWallPointsInLists()
    {
        for (int heightIdx = 0; heightIdx < mazeHeight + 1; heightIdx++)
        {
            if (heightIdx == mazeHeight)
            {
                CalculateAndAddInListHorizontalWallPoints(heightIdx);
                return;
            }

            CalculateAndAddInListHorizontalWallPoints(heightIdx);
            CalculateAndAddInListVerticalWallPoints(heightIdx);
        }
    }

    private void CalculateAndAddInListHorizontalWallPoints(int heightIdx)
    {
        if (!(heightIdx == 0 || heightIdx == mazeHeight))
            horizontalWallPoints.Add(new List<Vector2>());

        for (int widthIdx = 0; widthIdx < mazeWidth + 1; widthIdx++)
        {
            if (widthIdx == mazeWidth)
            {
                MakeCrossWall(CalculatePoint(TypeOfCell.CROSS, widthIdx, heightIdx));
                return;
            }

            MakeCrossWall(CalculatePoint(TypeOfCell.CROSS, widthIdx, heightIdx));
            var wallPoint = CalculatePoint(TypeOfCell.HORIZONTAL, widthIdx, heightIdx);

            if (heightIdx == 0 || heightIdx == mazeHeight)
                MakeWall(wallPoint);
            else
                horizontalWallPoints[heightIdx - 1].Add(wallPoint);
        }
    }

    private void CalculateAndAddInListVerticalWallPoints(int heightIdx)
    {
        verticalWallPoints.Add(new List<Vector2>());

        for (int widthIdx = 0; widthIdx < mazeWidth + 1; widthIdx++)
        {    
            var wallPoint = CalculatePoint(TypeOfCell.VERTICAL, widthIdx, heightIdx);
            if (widthIdx == 0 || widthIdx == mazeWidth)            
                MakeWall(wallPoint);
            else      
                verticalWallPoints[heightIdx].Add(wallPoint);
        }
    }

    private Vector3 CalculatePoint(TypeOfCell type, float widthIdx, float heightIdx)
    {
        if (type == TypeOfCell.CROSS)
            return new Vector2((widthOfCrossWall + widthOfWall) * widthIdx, (widthOfCrossWall + widthOfWall) * heightIdx);

        if (type == TypeOfCell.HORIZONTAL)
            return new Vector2(((widthOfCrossWall + widthOfWall) * widthIdx) + widthOfCrossWall, (widthOfCrossWall + widthOfWall) * heightIdx);

        if (type == TypeOfCell.VERTICAL)
            return new Vector2((widthOfCrossWall + widthOfWall) * widthIdx, ((widthOfCrossWall + widthOfWall) * heightIdx) + widthOfCrossWall);

        if (type == TypeOfCell.EMPTY_CELL)
            return new Vector3(widthOfWall * widthIdx + widthOfCrossWall, widthOfWall * heightIdx + widthOfCrossWall, 1);

        return Vector2.zero;
    }

    private void MakeWall(Vector2 point)
    {
        Instantiate(horizontalWallPref, point, new Quaternion(), transform);
    }

    private void MakeCrossWall(Vector2 point)
    {
        Instantiate(crossWallPref, point, new Quaternion(), transform);
    }

    #endregion
}

enum TypeOfCell
{
    HORIZONTAL,
    VERTICAL,
    CROSS,
    EMPTY_CELL
}