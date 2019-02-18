using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public void UnblockUp()
    {
        directionBlocks[0] = false;
    }

    public void UnblockRight()
    {
        directionBlocks[1] = false;
    }

    public void UnblockDown()
    {
        directionBlocks[2] = false;
    }

    public void UnblockLeft()
    {
        directionBlocks[3] = false;
    }

    public void BlockUp()
    {
        directionBlocks[0] = true;
    }

    public void BlockRight()
    {
        directionBlocks[1] = true;
    }

    public void BlockDown()
    {
        directionBlocks[2] = true;
    }

    public void BlockLeft()
    {
        directionBlocks[3] = true;
    }
    
    public void BlockAll()
    {
        directionBlocks = new List<bool> { true, true, true, true };
    } 

    public bool IsBlocked()
    {
        foreach (var block in directionBlocks)
        {
            if (!block)
                return false;
        }
        return true;
    }

    public List<bool> GetDirectionBlocks()
    {
        return directionBlocks;
    }

    public void SetPointInWorld(Vector2 pointInWorld)
    {
        this.pointInWorld = pointInWorld;
    }

    public Vector2 GetPointInWorld()
    {
        return pointInWorld;
    }

    public void SetIsFinish(bool isFinish)
    {
        this.isFinish = isFinish;
    }

    public bool GetIsFinish()
    {
        return isFinish;
    }

    #region private

    List<bool> directionBlocks = new List<bool>() { false, false, false, false }; //up, right, down, left
    private Vector2 pointInWorld;
    private bool isFinish = false;

    #endregion
}
