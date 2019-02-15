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
        heightOfWall = horizontalWallPref.size.y * horizontalWallPref.transform.localScale.y;
        widthOfCrossWall = crossWallPref.size.x * crossWallPref.transform.localScale.x;
    }

    public float GetMazeHeight()
    {
        return mazeHeight * (heightOfWall + widthOfCrossWall) + widthOfCrossWall;
    }

    public float GetMazeWidth()
    {
        return mazeWidth * (widthOfWall + widthOfCrossWall) + widthOfCrossWall;
    }

    public void MakeMaze()
    {
        MakeMazePatternAndAddWallPointsInLists();
        var pathFinder = new PathFinder(mazeHeight, mazeWidth, horizontalWallPoints, verticalWallPoints);
        pathFinder.MakeMaze();
        MakeWallsFromLists();
    }

    #endregion

    #region Private
    
    private Quaternion ROTATE_TO_VERTICAL = Quaternion.Euler(new Vector3(0, 0, 90));

    private float heightOfWall;
    private float widthOfWall;
    private float widthOfCrossWall;
    private List<List<Vector2>> horizontalWallPoints = new List<List<Vector2>>();
    private List<List<Vector2>> verticalWallPoints = new List<List<Vector2>>();
    
    private void MakeWallsFromLists()
    {
        foreach(var wallPoints in horizontalWallPoints)
        {
            foreach(var wallPoint in wallPoints)
            {
                if (wallPoint != Vector2.zero)
                    MakeHorizontalWall(wallPoint);
            }
        }

        foreach (var wallPoints in verticalWallPoints)
        {
            foreach (var wallPoint in wallPoints)
            {
                if (wallPoint != Vector2.zero)
                    MakeVerticalWall(wallPoint);
            }
        }
    }

    private void MakeMazePatternAndAddWallPointsInLists()
    {
        for(int heightIdx = 0; heightIdx < mazeHeight + 1; heightIdx++)
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
                MakeCrossWall(CalculatePoint(TypeOfWall.CROSS, widthIdx, heightIdx));
                return;
            }

            MakeCrossWall(CalculatePoint(TypeOfWall.CROSS, widthIdx, heightIdx));
            var wallPoint = CalculatePoint(TypeOfWall.HORIZONTAL, widthIdx, heightIdx);

            if (heightIdx == 0 || heightIdx == mazeHeight)
                MakeHorizontalWall(wallPoint);
            else
                horizontalWallPoints[heightIdx - 1].Add(wallPoint);
        }
    }

    private void CalculateAndAddInListVerticalWallPoints(int heightIdx)
    {
        verticalWallPoints.Add(new List<Vector2>());

        for (int widthIdx = 0; widthIdx < mazeWidth + 1; widthIdx++)
        {    
            var wallPoint = CalculatePoint(TypeOfWall.VERTICAL, widthIdx, heightIdx);
            if (widthIdx == 0 || widthIdx == mazeWidth)            
                MakeVerticalWall(wallPoint);
            else      
                verticalWallPoints[heightIdx].Add(wallPoint);
        }
    }

    private Vector3 CalculatePoint(TypeOfWall type, float widthIdx, float heightIdx)
    {
        if (type == TypeOfWall.CROSS)
            return new Vector3((widthOfCrossWall + widthOfWall) * widthIdx, (widthOfCrossWall + widthOfWall) * heightIdx);

        if (type == TypeOfWall.HORIZONTAL)
            return new Vector3(((widthOfCrossWall + widthOfWall) * widthIdx) + widthOfCrossWall, (widthOfCrossWall + widthOfWall) * heightIdx);

        if (type == TypeOfWall.VERTICAL)
            return new Vector3((widthOfCrossWall + widthOfWall) * widthIdx, ((widthOfCrossWall + widthOfWall) * heightIdx) + widthOfCrossWall);

        return Vector3.zero;
    }

    private SpriteRenderer MakeHorizontalWall(Vector2 point)
    {
        return Instantiate(horizontalWallPref, point, new Quaternion(), transform);
    }

    private SpriteRenderer MakeVerticalWall(Vector2 point)
    {
        var wall = Instantiate(horizontalWallPref, point, ROTATE_TO_VERTICAL, transform);
        wall.flipY = true;
        return wall;
    }

    private void MakeCrossWall(Vector2 point)
    {
        Instantiate(crossWallPref, point, new Quaternion(), transform);
    }

    #endregion
}

enum TypeOfWall
{
    HORIZONTAL,
    VERTICAL,
    CROSS
}