using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//node class used for pathfinding - each tile is a node
public class Node 
{
    public Point GridPosition { get; set; }

    public TileScript TileRef { get; set; }

    public Node(TileScript tileRef)
    {
        //set the tile that the node references, and its grid position
        this.TileRef = tileRef;
        this.GridPosition = tileRef.GridPosition;
    }

}//end class
