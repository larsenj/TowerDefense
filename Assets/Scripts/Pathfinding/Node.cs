using System;
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

    //track path costs - F(x) = G(x) + H(x)
    public int G { get; set; }
    public int H { get; set; }
    public int F { get; set; }

    public Vector2 WorldPosition { get; set; }

    public Node(TileScript tileRef)
    {
        //set the tile that the node references, and its grid position
        this.TileRef = tileRef;
        this.GridPosition = tileRef.GridPosition;
        this.WorldPosition = tileRef.WorldPosition;
    }

    //get the H, G and F values
    public void CalcValues(Node parent, Node end, int gCost)
    {
        this.Parent = parent;
        this.G = gCost + parent.G;
        int HX = Math.Abs(GridPosition.X - end.GridPosition.X);
        int HY = Math.Abs(GridPosition.Y - end.GridPosition.Y);
        this.H = 10 * (HX + HY);

        this.F = G + H;
    }

}//end class
