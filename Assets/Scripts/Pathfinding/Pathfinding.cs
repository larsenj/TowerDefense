using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//static class used for pathfinding
public static class Pathfinding
{
    //stores the nodes
    private static Dictionary<Point, Node> nodes;

    private static void CreateNodes()
    {
        nodes = new Dictionary<Point, Node>();

        //iterate through the tiles in the level, adding them to the dictionary
        foreach(TileScript tile in TileManager.Instance.TileDict.Values)
        {
            nodes.Add(tile.GridPosition, new Node(tile));
        }
    }

}
