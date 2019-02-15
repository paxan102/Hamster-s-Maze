using System.Collections.Generic;

class Cell
{
    public void Init(int height, int width)
    {
        this.height = height;
        this.width = width;
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
    
    public bool IsBlocked()
    {
        foreach (var block in directionBlocks)
        {
            if (!block)
                return false;
        }
        return true;
    }

    public int GetHeight()
    {
        return height;
    }

    public int GetWidth()
    {
        return width;
    }

    public List<bool> GetDirectionsBlocks()
    {
        return directionBlocks;
    }

    #region private

    List<bool> directionBlocks = new List<bool>() { false, false, false, false }; //up, right, down, left

    private int width;
    private int height;

    #endregion
}
