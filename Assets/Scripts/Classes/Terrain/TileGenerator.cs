using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour {

    public static TileGenerator instance;

    public GameObject tilePref;//Reference to our Tile templete
	//Map/Graph
	public static List<List<Tile>> map = new List<List<Tile>>(); //All the tiles of our map
	public static int MapHeight, MapWidth; //Width and height of our map that we can access from outher classes => (static)

    [SerializeField] int tilesPerFrame = 300;

    private void Awake()//A function that is called at the creation of this object in game
    {
        if(instance == null) { instance = this; }
        if(instance != this) { Destroy(this); }
        MapHeight = 25;
        MapWidth = 50;
		StartCoroutine(GenerateMap(MapHeight,MapWidth));
        
    }

    //Create a map based on the desired width and height

    IEnumerator GenerateMap(int rows, int columns)
    {
        int count = 0;
        for (int i = 0; i < columns; i++)
        {
            //Create a temporary list for every column
			List<Tile> lst = new List<Tile> ();
            for (int j = 0; j < rows; j++)
            {
                //Instantiating = creating an instance from a template
				GameObject tileInst = Instantiate(tilePref, new Vector3(i, j, 0), Quaternion.identity, transform);
                //GameObjects have many components on them that describe the attributes and behaviour of it
                //Here we are interested in the Tile script
                Tile t = tileInst.GetComponent<Tile> ();
                //Set x and y position of a Tile as well as setting isWalkable to true - our default value
				t.x = i;
				t.y = j;
                t.isWalkable = true;
                //Add the reference to the Tile component of the object we have just created to a list lst
				lst.Add (t);
                //Debug.Log(string.Format("Making a tile {0}, {1}.", t.x, t.y));
                int rand = Random.Range(0, 10);
                //if(rand < 5) { t.SetDifficulty(5f); }
                //if(rand < 1) { t.ChangeWalkable(false); }
                count++;
                if(count % tilesPerFrame == 0)
                {
                    yield return null;
                }
            }
            //Add the list for the full column to the map - we end up wit ha list of lists of tiles
			map.Add (lst);   
        }
        GetNeighbours(MapHeight, MapWidth);
    }

    //A function that generates a list of all neghbours of a certain tile


    //A function that gets the neighbours for all tiles in a map
    void GetNeighbours(int rows, int columns)
    {
		for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                //Set the list of neighbours in a variable fromthe Tile script, so that we don't have to call it every time => sacrificing memory for speed 
                //map[i][j].GetComponent<Tile>().neigh = GetNeighbour(map[i][j].GetComponent<Tile>());
                map[i][j].GetComponent<Tile>().CalculateNeighbours(map);
            }
        }
    }
}

