using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] GameObject[] tilePrefabs;

    [SerializeField] CameraMovement cameraMovement;

    [SerializeField] Transform map;


    Point blueSpawn, redSpawn;
    [SerializeField] GameObject bluePortalPrefab;
    [SerializeField] GameObject redPortalPrefab;

    public Portal BluePortal { get; set; }

    Point mapSize;

    Stack<Node> path;

    public Stack<Node> Path
    {
        get
        {
            if(path == null)
            {
                GeneratePath();
            }

            return new Stack<Node>(new Stack<Node>(path));
        }
    }

    public Point BlueSpawn
    {
        get
        {
            return blueSpawn;
        }
    }
    public Point RedSpawn
    {
        get
        {
            return redSpawn;
        }
    }

    public Dictionary<Point, TileScript> Tiles { get; set; }
    public float TileSize
    {
        get { return tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x; }
    }
    void Start()
    {
        Point p = new Point(0, 0);
        Debug.Log(p.X);
        CreateLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TestValue(Point p)
    {
        p.X = 3;
    }

    void CreateLevel()
    {
        Tiles = new Dictionary<Point, TileScript>();

        string[] mapData = ReadLevelText();

        mapSize = new Point(mapData[0].ToCharArray().Length, mapData.Length);

        int mapX = mapData[0].ToCharArray().Length;
        int mapY = mapData.Length - 1;

        Vector3 maxTile = Vector3.zero;

        Vector3 worldStartPos = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));
        for(int y = 0; y < mapY; y++)
        {
            char[] newTiles = mapData[y].ToCharArray();
            for (int x = 0; x < mapX; x++)
            {
                PlaceTile(newTiles[x].ToString(),x, y, worldStartPos);
            }
        }

        maxTile = Tiles[new Point(mapX - 1, mapY - 1)].transform.position;

        cameraMovement.SetLimits(new Vector3(maxTile.x + TileSize, maxTile.y - TileSize));
        SpawnPortals();
    
    
    }
    void PlaceTile(string tileType, int x, int y, Vector3 worldStartPos)
    {
        int tileIndex = int.Parse(tileType);
        TileScript newTile = Instantiate(tilePrefabs[tileIndex]).GetComponent<TileScript>();

        newTile.SetUp(new Point(x, y), new Vector3(worldStartPos.x + TileSize * x, worldStartPos.y - TileSize * y, 0), map);

      

    }

    string[] ReadLevelText()
    {
        TextAsset bindData = Resources.Load("Level") as TextAsset;
        string data = bindData.text.Replace(Environment.NewLine, string.Empty);
        return data.Split('-');
    }

    private void SpawnPortals()
    {
        blueSpawn = new Point(0,0);
        GameObject tmp = (GameObject)Instantiate(bluePortalPrefab, Tiles[blueSpawn].GetComponent<TileScript>().WorldPosition, Quaternion.identity);
        BluePortal = tmp.GetComponent<Portal>();
        BluePortal.name = "BluePortal";
        
        
        redSpawn = new Point(11, 9);
        Instantiate(redPortalPrefab, Tiles[redSpawn].GetComponent<TileScript>().WorldPosition, Quaternion.identity);
        //redSpawn = new Point(11, 2);
        //Instantiate(redPortalPrefab, Tiles[redSpawn].GetComponent<TileScript>().WorldPosition, Quaternion.identity);
    }

    public bool InBounds(Point position)
    {
        return position.X >= 0 && position.Y >= 0 && position.X < mapSize.X && position.Y < mapSize.Y;
    }

    public void GeneratePath()
    {
        path = AStar.GetPath(blueSpawn, redSpawn);
    }
}
