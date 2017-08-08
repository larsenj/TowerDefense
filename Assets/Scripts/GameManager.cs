using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public TowerButton ClickedButton { get; set; }

    public int Currency
    {
        get
        {
            return currency;
        }

        set
        {
            currency = value;
            this.currencyText.text = " $ <color=white>" + value.ToString() + "</color>";
        }
    }

    private int currency;


    [SerializeField]
    private Text currencyText;

   
	// Use this for initialization
	void Start () {
        Currency = 5;	
	}
	
	// Update is called once per frame
	void Update () {
        HandleEscape();	
	}

    //choses the current/active tower based on clicked UI button
    public void PickTower(TowerButton towerButton)
    {
        //check if enough currency to purchase tower
        if (Currency >= towerButton.Price)
        {
            this.ClickedButton = towerButton;
            Hover.Instance.Activate(towerButton.Sprite);
        }
    }

    //spend money on buying a tower and resetting the selection
    public void BuyTower()
    {
        //double-check that there's enough currency
        if (Currency >= ClickedButton.Price)
        {
            //decrement currency by the cost of the tower
            Currency -= ClickedButton.Price;

            //after the tower is placed deactivate the hovering icon
            Hover.Instance.Deactivate();
        }
    }

    //deselect tower and turn off hover icon if user presses esc
    public void HandleEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Hover.Instance.Deactivate();
    }
}
