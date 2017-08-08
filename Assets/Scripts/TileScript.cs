using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileScript : MonoBehaviour {

    public Point GridPosition { get; private set; }

    private SpriteRenderer spriteRenderer;

    //determines if tile is occupied - used for pathfinding and tower placement
    public bool IsEmpty { get; set; }

    //red color for when can't place a tower
    private Color32 fullColor = new Color32(255, 118, 118, 255);

    //color for when a tower can be placed
    private Color32 emptyColor = new Color32(96, 255, 90, 255);

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
        spriteRenderer = GetComponent<SpriteRenderer>();	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //establish the tiles's position, add it to the dictionary
    public void Setup(Point GridPos, Vector3 worldPos, Transform parent)
    {
        this.GridPosition = GridPos;
        transform.position = worldPos;
        transform.SetParent(parent);    //to make the GameObject hierarchy cleaner
        TileManager.Instance.TileDict.Add(GridPos, this);
        IsEmpty = true;
    }

    public void OnMouseOver()
    {
        //place tower only if not over a button
        if (!EventSystem.current.IsPointerOverGameObject() &&
            GameManager.Instance.ClickedButton != null)
        {
            //highlight tiles if empty
            if (IsEmpty)
                ColorTile(emptyColor);

            //if not empty highlight in red, if empty can place tower
            if (!IsEmpty)
                ColorTile(fullColor);
            else if (Input.GetMouseButtonDown(0))
                PlaceTower();
        }
    }

    private void OnMouseExit()
    {
        ColorTile(Color.white);
    }

    private void PlaceTower()
    {
        GameObject tower = Instantiate(GameManager.Instance.ClickedButton.TowerPrefab,
            transform.position, Quaternion.identity);

        //ensure lower on the screen is "closer" to the player - no weird overlapping
        tower.GetComponent<SpriteRenderer>().sortingOrder = GridPosition.Y;

        //set the tower as a child of the corresponding tile 
        tower.transform.SetParent(transform);

        //spend the resources and reset the button
        GameManager.Instance.BuyTower();

        ColorTile(Color.white);
        IsEmpty = false;
    }

    private void ColorTile(Color newColor)
    {
        spriteRenderer.color = newColor;
    }
}
