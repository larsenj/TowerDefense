using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] tiles;

    //variables for setting up the creature startpoint
    private Point startSpawn;
    [SerializeField]
    private GameObject startSpawnPrefab;

    //variables for setting up the creature endpoints
    private Point endSpawn;
    [SerializeField]
    private GameObject endSpawnPrefab;

    //dictionary to hold all the tiles
    public Dictionary<Point, TileScript> TileDict { get; set; }

    //property for getting the size of the tile, so can be used elsewhere
    public float TileSize
    {
        get
        {
            return tiles[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        }
    }
	// Use this for initialization
	void Start ()
    {
        CreateLevel();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void CreateLevel()
    {
        //instantiate the tiles dictionary
        TileDict = new Dictionary<Point, TileScript>();

        //temporary array for map configuration
        string[] mapData = ReadLevelData();

        int mapXSize = mapData[0].ToCharArray().Length; //length of 1st item in mapData[]
        int mapYSize = mapData.Length;

        Vector3 maxTile = Vector3.zero;

        //get the upper-left corner of the screen
        Vector3 startPosition = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));

        for (int y = 0; y < mapYSize-1; y++)
        {
            //turn the string into a char[]
            char[] tileChars = mapData[y].ToCharArray();

            for (int x = 0; x < mapXSize; x++)
            {
                PlaceTile(x, y, startPosition, tileChars[x].ToString());
            }
        }//end for y

        maxTile = TileDict[new Point(mapXSize - 1, mapYSize - 2)].transform.position;

        SpawnStartEnd();

    }//end CreateLevel

    //place tile at the given x/y location
    private void PlaceTile(int x, int y, Vector3 startPosition, string tileType)
    {
        //turn the char into a number
        int tileIndex = int.Parse(tileType);

        //create a new TileScript and set up the grid position
        TileScript newTile = Instantiate(tiles[tileIndex]).GetComponent<TileScript>();
        newTile.Setup(new Point(x, y), new Vector3(startPosition.x + 
            (TileSize * x), startPosition.y - (TileSize * y), 0) );

        //add the new tile to the dictionary
        TileDict.Add(new Point(x, y), newTile);
    }//end PlaceTile

    //read the level design in from a text file
    //IMPORTANT: file MUST use Windows endlines!
    private string[] ReadLevelData()
    {
        //load the text file
        TextAsset bindData = Resources.Load("Level") as TextAsset;
        
        //remove newlines
        string data = bindData.text.Replace(Environment.NewLine, string.Empty);
        
        //split into separate strings on '-'
        return data.Split('-');
    }//end ReadLevelData

    private void SpawnStartEnd()
    {
        startSpawn = new Point(0, 0);
        Instantiate(startSpawnPrefab, TileDict[startSpawn].GetComponent<TileScript>().WorldPosition, Quaternion.identity);

        endSpawn = new Point(12, 9);
        Instantiate(endSpawnPrefab, TileDict[endSpawn].GetComponent<TileScript>().WorldPosition, Quaternion.identity);

    }
}//end class
