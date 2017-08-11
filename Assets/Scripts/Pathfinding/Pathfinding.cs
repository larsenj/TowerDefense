using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Static class used for pathfinding

1. Add start node to OPEN list
2. Look at neighbors, ignore unwalkable
3. Add neighbors to OPEN list
4. Set current node (cur) as parent
5. Move cur from OPEN to CLOSED list
6. Score nodes => F(x) = G(x) * H(x)
      - G(x) is cost to get to node
      - H(x) is cost to beeline to end
7. Select node w/ smallest score from OPEN
8. Move smallest from OPEN to CLOSED
9. Examine neighbors, ignore unwalkable and CLOSED. Add new neighbors to
     OPEN. Check if nodes already in OPEN and if current node is better
     than parent
10. Repeat from step 7
 */
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

    //generates the path
    public static void GetPath(Point start)
    {
        //creates nodes if needed
        if (nodes == null)
            CreateNodes();

        //OPEN list - HashSet is an unordered collection of unique elements,
        //indexing elements by hash
        HashSet<Node> openList = new HashSet<Node>();

        //CLOSED list
        HashSet<Node> closedList = new HashSet<Node>();

        //create reference to starting node
        Node curr = nodes[start];

        //1. Add start node to OPEN list
        openList.Add(curr);

        //2. Look at neighbors, ignore unwalkable
        for(int x = -1; x <= 1; x++)
        {
            for(int y = -1; y <= 1; y++)
            {

                Point neighPos = new Point(curr.GridPosition.X - x, curr.GridPosition.Y - y);

                if (TileManager.Instance.TileDict.ContainsKey(neighPos))
                {
                    //6. Score nodes => F(x) = G(x) * H(x)
                    int gCost = 0;
                    if (Math.Abs(x - y) == 1) //horizontal or vertical
                        gCost = 10;
                    else //diagonal
                        gCost = 14;

                    if (TileManager.Instance.TileDict[neighPos].IsWalkable &&
                        neighPos != curr.GridPosition)
                    {
                        //3.Add neighbors to OPEN
                        Node neighbor = nodes[neighPos];
                        if(!openList.Contains(neighbor))
                        {
                            openList.Add(neighbor);
                        }
                        //FOR DEBUGGING ONLY:
                        //neighbor.TileRef.SpriteRenderer.color = Color.black;

                        //4. Set current node (cur) as parent
                        neighbor.CalcValues(curr, gCost);
                    }
                }//end if neighbor in tile dictionary
            }//end for y
        }//end for x

        //5. Move cur from OPEN to CLOSED list
        openList.Remove(curr);
        closedList.Add(curr);

        //FOR DEBUGGING ONLY:
        GameObject.Find("Debugging").GetComponent<Debugging>().DebugPath(openList, closedList);
    }

}


