using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour {

    public Point GridPosition { get; private set; }

    //get the center of the tile
    public Vector2 WorldPosition
    {
        get
        {
            //get half the height, and half the width, and add to top-left point (origin)
            float halfX = GetComponent<SpriteRenderer>().bounds.size.x/2;
            float halfY = GetComponent<SpriteRenderer>().bounds.size.y/2;

            return new Vector2(transform.position.x + halfX, transform.position.y - halfY);
        }

    }
                                       
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Setup(Point GridPos, Vector3 worldPos)
    {
        this.GridPosition = GridPos;
        transform.position = worldPos;
    }
}
