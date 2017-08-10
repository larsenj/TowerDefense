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

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        ClickTile();	
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

    }

}
