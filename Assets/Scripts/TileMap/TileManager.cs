using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : Singleton<TileManager>
{
    [SerializeField]
    private GameObject[] tiles;

    [SerializeField]
    private Transform map;

    //variables for setting up the creature startpoint
    private Point startSpawn;
    public Point StartSpawn { get { return startSpawn; } }
    [SerializeField]
    private GameObject startSpawnPrefab;

    public SpawnPoint StartPoint { get; set; }

    //variables for setting up the creature endpoints
    private Point endSpawn;
    [SerializeField]
    private GameObject endSpawnPrefab;

    //for determining if a point is outside the bounds of the map
    //private Point mapSize;

    private Stack<Node> path;

    public Stack<Node> Path
    {
        get
        {
            if (path == null)
                GeneratePath();

            //ensures each mob has its own stack
            //TODO: this is silly, just convert it to an array
            return new Stack<Node>(new Stack<Node>(path));
        }
    }


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

        //place the start and end points
        SpawnStartEnd();

        //max x and y values of the map
        //mapSize = new Point(mapData[0].ToCharArray().Length, mapData.Length);

    }//end CreateLevel

    //place tile at the given x/y location
    private void PlaceTile(int x, int y, Vector3 startPosition, string tileType)
    {
        //turn the char into a number
        int tileIndex = int.Parse(tileType);

        //create a new TileScript and set up the grid position
        TileScript newTile = Instantiate(tiles[tileIndex]).GetComponent<TileScript>();
        newTile.Setup(new Point(x, y), new Vector3(startPosition.x + 
            (TileSize * x), startPosition.y - (TileSize * y), 0), map);

        //add the new tile to the dictionary - moved to TileScript
        //TileDict.Add(new Point(x, y), newTile);
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
        startSpawn = new Point(0, 1);
        GameObject temp = (GameObject)Instantiate(startSpawnPrefab, 
            TileDict[startSpawn].GetComponent<TileScript>().WorldPosition, 
            Quaternion.identity);
        StartPoint = temp.GetComponent<SpawnPoint>();
        StartPoint.name = "Start";

        endSpawn = new Point(12, 9);
        //endSpawn = new Point(3, 6);   //closer endpoint for faster debugging
        Instantiate(endSpawnPrefab, TileDict[endSpawn].GetComponent<TileScript>().WorldPosition, 
            Quaternion.identity);

    }//end SpawnstartEnd

    public void GeneratePath()
    {
        //create the path stack
        path = Pathfinding.GetPath(startSpawn, endSpawn);
    }//end GeneratePath
}//end class
