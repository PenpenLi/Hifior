using UnityEngine;
using System.Collections;

public class AStarNode
{
    public bool _canWalk;
    public int gCost;
    public int hCost;
    public int _gridX, _gridY;
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
    public AStarNode parent;
    public AStarNode(bool CanWakl, int x, int y)
    {
        _canWalk = CanWakl;
        _gridX = x;
        _gridY = y;
    }

}
