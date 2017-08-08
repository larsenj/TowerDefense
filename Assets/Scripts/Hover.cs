using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class creates an icon of the selected tower attached to the mouse 
//pointer when the user selects a tower
public class Hover : Singleton<Hover>
{

    private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        FollowMouse();	
	}

    //tracks the coordinates of the mouse
    private void FollowMouse()
    {
        if (spriteRenderer.enabled)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
    }

    //activates the hoverig icon - called from PickTower() in the GameManager
    public void Activate(Sprite sprite)
    {
        this.spriteRenderer.sprite = sprite;
        spriteRenderer.enabled = true;
    }

    //
    public void Deactivate()
    {
        spriteRenderer.enabled = false;
        GameManager.Instance.ClickedButton = null;
    }
}
