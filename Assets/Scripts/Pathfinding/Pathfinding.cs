using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public static Stack<Node> GetPath(Point start, Point end)
    {
        //creates nodes if needed
        if (nodes == null)
            CreateNodes();

        //OPEN list - HashSet is an unordered collection of unique elements,
        //indexing elements by hash
        HashSet<Node> openList = new HashSet<Node>();

        //CLOSED list
        HashSet<Node> closedList = new HashSet<Node>();

        //stack used to track the path to the end
        Stack<Node> pathStack = new Stack<Node>();

        //create reference to starting node
        Node curr = nodes[start];

        //1. Add start node to OPEN list
        openList.Add(curr);

    
        //10. Repeat from step 7
        //repeat as long as items remain in the OPEN list
        while (openList.Count > 0)
        {
            //2. Look at neighbors, ignore unwalkable
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {

                    Point neighPos = new Point(curr.GridPosition.X - x, curr.GridPosition.Y - y);

                    if (TileManager.Instance.TileDict.ContainsKey(neighPos))
                    {
                        //6. Score nodes => F(x) = G(x) * H(x)
                        int gCost = 0;
                        if (Math.Abs(x - y) == 1) //horizontal or vertical
                            gCost = 10;
                        else //diagonal
                        {
                            //if the diagonal is blocked, skip this iteration
                            //of the whole loop
                            if (!ConnectedDiag(curr, nodes[neighPos]))
                                continue;
                            gCost = 14;

                        }
                        //9. ...ignore unwalkable...
                        if (TileManager.Instance.TileDict[neighPos].IsWalkable &&
                            neighPos != curr.GridPosition)
                        {
                            //3.Add neighbors to OPEN
                            Node neighbor = nodes[neighPos];


                            //9. ...Check if nodes already in OPEN... 
                            if (openList.Contains(neighbor))
                            {
                                //check if current node is a better parent (lower score)
                                if (curr.G + gCost < neighbor.G)
                                    neighbor.CalcValues(curr, nodes[end], gCost);

                            }
                            //9. ...ignore CLOSED...
                            else if (!closedList.Contains(neighbor))
                            {
                                openList.Add(neighbor);
                                neighbor.CalcValues(curr, nodes[end], gCost);//also sets curr as parent
                            }
                        }
                    }//end if neighbor in tile dictionary
                }//end for y
            }//end for x

            //5. Move cur from OPEN to CLOSED list
            //8. Move smallest from OPEN to CLOSED
            openList.Remove(curr);
            closedList.Add(curr);

            //7. Select node w/ smallest score from OPEN
            if (openList.Count > 0)
            {
                //lambda expression - look at every node, n, using n.F order them,
                //select first on the list (lowest value)
                curr = openList.OrderBy(n => n.F).First();
            }

            //10. quit if goal is found
            if (curr == nodes[end])
            {
                //push all the nodes into the stack, backtracking to the starting one
                while (curr.GridPosition != start)
                {
                    pathStack.Push(curr);
                    curr = curr.Parent;
                }
                break;
            }


        }//end while

        return pathStack;

        //FOR DEBUGGING ONLY:
        //GameObject.Find("Debugging").GetComponent<Debugging>().DebugPath(openList, closedList, pathStack);
    }//end GetPath

    private static bool ConnectedDiag(Node curr, Node neighbor)
    {
        Point direction = neighbor.GridPosition - curr.GridPosition;

        //first point to check for occupancy
        Point first = new Point(curr.GridPosition.X + direction.X, 
            curr.GridPosition.Y);
        //second point to check for occupancy
        Point second = new Point(curr.GridPosition.X, curr.GridPosition.Y 
            + direction.Y);

        //if within bounds but not walkable, return false
        if(TileManager.Instance.TileDict.ContainsKey(first) && 
            !TileManager.Instance.TileDict[first].IsWalkable)
            return false;
        if (TileManager.Instance.TileDict.ContainsKey(second) &&
            !TileManager.Instance.TileDict[second].IsWalkable)
            return false;

        return true;

    }//end ConnectedDiag

}//end class

