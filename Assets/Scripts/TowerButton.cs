using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerButton : MonoBehaviour
{
    [SerializeField]
    private GameObject towerPrefab;

    //save the icon of the selected tower
    [SerializeField]
    private Sprite sprite;

    //how much the tower will cost
    [SerializeField]
    private int price;
    //exposed property version of how much the tower costs
    public int Price
    {
        get
        {
            return price;
        }
    }    
    
    //text field displaying the price
    [SerializeField]
    private Text priceText;

    public GameObject TowerPrefab
    {
        get { return towerPrefab; }
    }

    //property to get the icon of the selected tower - called by the Hover class
    public Sprite Sprite
    {
        get
        {
            return sprite;
        }
    }

    // Use this for initialization
    void Start () {
        priceText.text = "$" + Price;
    }
	
	// Update is called once per frame
	void Update () {}
}
