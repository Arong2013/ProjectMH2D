using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AstarNode
{
    public bool walkable;
    public Vector2Int worldPosition;
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;
    public AstarNode parent;
    int heapIndex;

    public AstarNode(bool _walkable, Vector2Int _worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(AstarNode nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}
