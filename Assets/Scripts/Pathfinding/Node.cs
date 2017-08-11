using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//node class used for pathfinding - each tile is a node
public class Node 
{
    public Point GridPosition { get; set; }

    //track the script corresponding to the tile
    public TileScript TileRef { get; set; }

    //track the parent node
    public Node Parent { get; set; }

    //track g-cost
    public int G { get; set; }

    public Node(TileScript tileRef)
    {
        //set the tile that the node references, and its grid position
        this.TileRef = tileRef;
        this.GridPosition = tileRef.GridPosition;
    }

    public void CalcValues(Node parent, int gCost)
    {
        this.Parent = parent;
        this.G = gCost + parent.G;
    }

}//end class
