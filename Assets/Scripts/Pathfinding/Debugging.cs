using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class is used solely for debugging and can be safely deleted when no
//longer needed. I will likely save it for future use though.
public class Debugging : MonoBehaviour {

    [SerializeField]
    private TileScript goal;
    [SerializeField]
    private TileScript start;

    [SerializeField]
    private GameObject arrowPrefab;

	// Use this for initialization
	void Start () {
		
	}
	

    /*
	// Update is called once per frame
	void Update ()
    {
        ClickTile();

        if (Input.GetKeyDown(KeyCode.Space))
            Pathfinding.GetPath(start.GridPosition, goal.GridPosition);
	}

    private void ClickTile()
    {
        if (Input.GetMouseButtonDown(1))
        {
            //raycast is an invisible line, here it will be drawn based on mouse clicks
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), 
                Vector2.zero);

            if(hit.collider != null)
            {
                //if hit a tile, get a reference to that tile
                TileScript temp = hit.collider.GetComponent<TileScript>();

                if (temp != null)
                {
                    //if no start point chose yet, make it the start point...
                    if (start == null)
                    {
                        start = temp;
                        start.SpriteRenderer.color = new Color32(255, 132, 0, 255);
                        start.Debugging = true; //ensure color doesn't change back after mouse leaves
                    }
                    //...otherwise make it the endpoint
                    else if (goal == null)
                    {
                        goal = temp;
                        goal.SpriteRenderer.color = new Color32(255, 0, 0, 255);
                        goal.Debugging = true;//ensure color doesn't change back after mouse leaves
                    }
                }
            }
        }

    }//end clickTile

    */

    public void DebugPath(HashSet<Node> openList, HashSet<Node> closedList, 
        Stack<Node> path)
    {
        foreach (Node node in openList)
        {
            if (node.TileRef != start && node.TileRef != goal)
                node.TileRef.SpriteRenderer.color = Color.cyan;

            PointToParent(node, node.TileRef.WorldPosition);
        }

        foreach (Node node in closedList)
        {
            if (node.TileRef != start && node.TileRef != goal && !path.Contains(node))
                node.TileRef.SpriteRenderer.color = Color.blue;

            PointToParent(node, node.TileRef.WorldPosition);
        }

        foreach(Node node in path)
        {
            if(node.TileRef != start && node.TileRef != goal && !path.Contains(node))
            {
                node.TileRef.SpriteRenderer.color = Color.cyan;

            }

        }
    }//end DebugPath

    //spawn an arrow pointing to the parent node
    private void PointToParent(Node node, Vector2 position)
    {
        if (node.Parent != null)
        {
            GameObject arrow = (GameObject)Instantiate(arrowPrefab, position, Quaternion.identity);

            //point arrow right
            if ((node.GridPosition.X < node.Parent.GridPosition.X) &&
                (node.GridPosition.Y == node.Parent.GridPosition.Y))
                arrow.transform.eulerAngles = new Vector3(0, 0, 0);
            //point up right
            else if ((node.GridPosition.X < node.Parent.GridPosition.X) &&
                (node.GridPosition.Y > node.Parent.GridPosition.Y))
                arrow.transform.eulerAngles = new Vector3(0, 0, 45);
            //point up
            else if ((node.GridPosition.X == node.Parent.GridPosition.X) &&
                (node.GridPosition.Y > node.Parent.GridPosition.Y))
                arrow.transform.eulerAngles = new Vector3(0, 0, 90);
            //point top left
            else if ((node.GridPosition.X > node.Parent.GridPosition.X) &&
                (node.GridPosition.Y > node.Parent.GridPosition.Y))
                arrow.transform.eulerAngles = new Vector3(0, 0, 135);
            //point left
            else if ((node.GridPosition.X > node.Parent.GridPosition.X) &&
                (node.GridPosition.Y == node.Parent.GridPosition.Y))
                arrow.transform.eulerAngles = new Vector3(0, 0, 180);
            //point bottom left
            else if ((node.GridPosition.X > node.Parent.GridPosition.X) &&
                (node.GridPosition.Y < node.Parent.GridPosition.Y))
                arrow.transform.eulerAngles = new Vector3(0, 0, 225);
            //point down
            else if ((node.GridPosition.X == node.Parent.GridPosition.X) &&
                (node.GridPosition.Y < node.Parent.GridPosition.Y))
                arrow.transform.eulerAngles = new Vector3(0, 0, 270);
            //point down right
            else if ((node.GridPosition.X < node.Parent.GridPosition.X) &&
                (node.GridPosition.Y < node.Parent.GridPosition.Y))
                arrow.transform.eulerAngles = new Vector3(0, 0, 315);



        }

    }

}//end class
