﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileScript : MonoBehaviour {

    public Point GridPosition { get; private set; }

    private SpriteRenderer spriteRenderer;
    public SpriteRenderer SpriteRenderer
    {
        get
        {
            return spriteRenderer;
        }

        set
        {
            spriteRenderer = value;
        }
    }
    
    public bool Debugging { get; set; }

    //track if a tower is placed on the tile
    private Towers tileTower;

    //determines if tile is occupied - used for pathfinding and tower placement
    public bool IsEmpty { get; set; }

    //determines if tile is walkable by creatures
    public bool IsWalkable { get; set; }

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
        //IsWalkable = true;
        if (this.name == "green(Clone)")
            IsWalkable = true;
        else
            IsWalkable = false;
        //Debug.Log(this.name);
        //else
    }

    public void OnMouseOver()
    {
        //place tower only if not over a button
        if (!EventSystem.current.IsPointerOverGameObject() &&
            GameManager.Instance.ClickedButton != null)
        {
            //highlight tiles if empty
            if (IsEmpty && !Debugging)
                ColorTile(emptyColor);

            //if not empty highlight in red, if empty can place tower
            if (!IsEmpty && !Debugging || IsWalkable)
                ColorTile(fullColor);
            else if (Input.GetMouseButtonDown(0))
                PlaceTower();
        }
        //if tower not selected but player clicks a tower
        //TODO: remove this functionality in the future
        else if (!EventSystem.current.IsPointerOverGameObject() &&
            GameManager.Instance.ClickedButton == null &&
            Input.GetMouseButtonDown(0) )
        {
            if (tileTower != null)
                GameManager.Instance.SelectTower(tileTower);
            else
                GameManager.Instance.DeselectTower();
        }

    }

    private void OnMouseExit()
    {
        if(!Debugging)
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

        //establish reference to the tower on the tile
        this.tileTower = tower.transform.GetChild(0).GetComponent<Towers>();

        //spend the resources and reset the button
        GameManager.Instance.BuyTower();

        ColorTile(Color.white);
        IsEmpty = false;
        IsWalkable = false;
    }

    private void ColorTile(Color newColor)
    {
        spriteRenderer.color = newColor;
    }
}
