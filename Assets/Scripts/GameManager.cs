using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public TowerButton ClickedButton { get; set; }

    //track, modify, and display the currency
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

    //tracks lives. Not currently significant.
    private int lives;
    public int Lives
    {
        get
        {
            return lives;
        }
        set
        {
            Debug.Log(lives);
            this.lives = value;
            //livesText.text = value.ToString();

            //if the player runs out of lives, call GameOver function - note
            //that functions can be called from within properties
            if (this.lives <= 0)
            {
                this.lives = 0;
                GameOver();
            }
        }
    }
    [SerializeField]
    private Text livesText;

    //property to access the pool of game objects
    public Objectpool Pool { get; set; }

    //tracks the waves
    private int wave = 0;
    [SerializeField]
    private Text waveText;
    [SerializeField]
    private GameObject waveButton;

    //track active mobs
    private List<Mobs> activeMobs = new List<Mobs>();
    //returns true if there are still mobs in the active list
    public bool waveActive
    {
        get
        {
            return activeMobs.Count > 0;
        }
    }

    private bool gameOver = false; //tell if the game is over
    [SerializeField]
    private GameObject gameOverMenu;

  /*
    //quit and play again buttons and variables
    [SerializeField]
    private GameObject playAgainButton;
    [SerializeField]
    private GameObject quitButton;
 */

    private void Awake()
    {
        Pool = GetComponent<Objectpool>();
    }

    // Use this for initialization
    void Start () {
        gameOver = false;
        Lives = 2;
        Currency = 500;	
	}
	
	// Update is called once per frame
	void Update () {
        HandleEscape();	
	}

    //choses the current/active tower based on clicked UI button
    public void PickTower(TowerButton towerButton)
    {
        //check if enough currency to purchase tower and if waves aren't active
        if (Currency >= towerButton.Price && !waveActive)
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

    //starts the wave of mobs
    public void StartWave()
    {
        //increment wave counter
        wave++;
        waveText.text = string.Format("Wave: <color=red>{0}</color>", wave);

        /*
        MonoBehaviour.StartCoroutine returns a Coroutine. Instances of this 
        class are only used to reference these coroutines and do not hold 
        any exposed properties or functions.

        A coroutine is a function that can suspend its execution(yield) until 
        the given given YieldInstruction finishes.
        */
       StartCoroutine(SpawnWave());

        //hide the wave button
        waveButton.SetActive(false);

    }//end StartWave

    //coroutine for spawning mobs incrementally
    private IEnumerator SpawnWave()
    {
        TileManager.Instance.GeneratePath();

        for (int i = 0; i < wave; i++)
        {


            //determine which mob to spawn
            int mobIndex = Random.Range(0, 3);
            //int mobIndex = 0;
            string type = string.Empty;
            switch (mobIndex)
            {
                case 0:
                    type = "RedMob";
                    break;
                case 1:
                    type = "BlueMob";
                    break;
                case 2:
                    type = "YellowMob";
                    break;
                default:
                    type = "RedMob";
                    break;
            }
            //instantiate and return a game object
            Mobs mob = Pool.GetObject(type).GetComponent<Mobs>();
            mob.Spawn();
            //add the mob to the list
            activeMobs.Add(mob);
            //wait X.X seconds
            yield return new WaitForSeconds(1.0f);
        }//end for i < wave
    }//end SpawnWave

    public void RemoveMob(Mobs mob)
    {
        activeMobs.Remove(mob);

        //re-enable the button if no more active mobs
        if (!waveActive)
            waveButton.SetActive(true);
    }//end RemoveMob

    public void GameOver()
    {
        //set gameover to true and activate the game over menu
        if (!gameOver)
        {
            gameOver = true;
            gameOverMenu.SetActive(true);
            waveButton.SetActive(false);
        }
    }//end GameOver

    //reloads the scene to play again
    public void PlayAgain()
    {
        Time.timeScale = 1; //in case want to implement pausing later
        //reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }//end PlayAgain

    //exits the program
    public void Quit()
    {
        Application.Quit();
    }//end Quit

}//end class
