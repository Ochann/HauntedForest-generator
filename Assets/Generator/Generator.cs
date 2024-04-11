using UnityEngine;
using System;
using UnityEngine.Tilemaps;
using NavMeshPlus.Components;

public class Generator : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField, Range(0, 100)] private int forestFillRate;
    [SerializeField, Range(0, 100)] private int dirtFillRate;

    [SerializeField] private Tilemap forestMap;
    [SerializeField] private Tilemap dirtMap;

    [SerializeField] private TileBase dirt;
    [SerializeField] private RuleTile grass;

    [SerializeField] private RuleTile treetop;

    [SerializeField] private int adventurerNum = 1;
    [SerializeField] private int forestSpiritNum = 1;
    [SerializeField] private GameObject adventurerPrefab;
    [SerializeField] private GameObject forestSpiritPrefab;
    [SerializeField] private GameObject treasurePrefab;
    [SerializeField] private GameObject playerPrefab;

    [SerializeField] private Manager ref_manager;
    [SerializeField] private NavMeshSurface navMesh;

    private int[,] grid;
    private int[,] forestGrid;

    private enum Map
    {
        Forest,
        Dirt
    }

    private void Start()
    {
        GenerateMap();

        SpawnAgents();

        SpawnItems();
    }

    // Generate Map
    private void GenerateMap()
    {
        GenerateGrid(Map.Forest);
        for (int i = 0; i < 7; i++)
        {
            CellularAutomata();
        }
        UpDateTilemap(Map.Forest);

        GenerateGrid(Map.Dirt);
        for (int i = 0; i < 7; i++)
        {
            CellularAutomata();
        }
        UpDateTilemap(Map.Dirt);
    }

    private void GenerateGrid(Map type)
    {
        if (type == Map.Dirt) forestGrid = grid;
        grid = new int[width, height];
        System.Random rand = new System.Random();
        int fillRate = (type == Map.Forest) ? forestFillRate : dirtFillRate;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == width-1 || y == 0 || y == height - 1) grid[x, y] = 1;
                else grid[x, y] = (rand.Next(0, 100) < fillRate) ? 1 : 0;
            }
        }
    }

    private void CellularAutomata()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int walls = GetSurroundingWalls(x, y);
                if (walls > 4)
                    grid[x, y] = 1;
                else if (walls < 4)
                    grid[x, y] = 0;
            }
        }
    }

    private int GetSurroundingWalls(int gridX, int gridY)
    {
        int surroundingWalls = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height)
                {
                    if (neighbourX != gridX || neighbourY != gridY)
                    {
                        surroundingWalls += grid[neighbourX, neighbourY];
                    }
                }
                else
                {
                    surroundingWalls++;
                }
            }
        }
        return surroundingWalls;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Regenerate");
            ref_manager.RemoveAllObjects();
            GenerateMap();
            SpawnAgents();
            SpawnItems();
        }
    }


    private void OnDrawGizmos()
    {
        if (grid != null)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    switch (forestGrid[x, y])
                    {
                        case 0:
                            Gizmos.color = (grid[x, y] == 1) ? Color.grey : Color.white; break;
                        case 1:
                            Gizmos.color = Color.black; break;
                    }
                    Vector3 pos = new Vector3(-width / 2 + x + .5f, -height / 2 + y + .5f, 0);
                    Gizmos.DrawCube(pos, Vector3.one);
                }
            }
        }
    }

    private void UpDateTilemap(Map type)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0) - new Vector3Int(width / 2, height / 2, 0);
                if (type == Map.Forest)
                {
                    if (grid[x, y] == 1) forestMap.SetTile(pos, treetop);
                    else forestMap.SetTile(pos, null);
                }
                else if (type == Map.Dirt)
                {
                    if (grid[x, y] == 1) dirtMap.SetTile(pos, dirt);
                    else dirtMap.SetTile(pos, grass);
                }
            }
        }

        if(type == Map.Forest)
        {
            navMesh.BuildNavMesh();
        }
    }


    // Spawn Agents
    private void SpawnAgents()
    {
        for(int i = 1; i <= adventurerNum;  i++)
        {
            Vector3 pos;
            while (true)
            {
                int x = UnityEngine.Random.Range(0, width);
                int y = UnityEngine.Random.Range(0, height);
                if (forestGrid[x, y] == 0)
                {
                    pos = new Vector3(x, y, 0) - new Vector3Int(width / 2, height / 2, 0);
                    break;
                }
            }
            GameObject obj = Instantiate(adventurerPrefab, pos, new Quaternion());
            obj.name = adventurerPrefab.name + "_" + i;
            ref_manager.AddObject(obj);
        }

        for (int i = 1; i <= forestSpiritNum; i++)
        {
            GameObject obj = Instantiate(forestSpiritPrefab, new Vector3(i*8f, i*8f, 0f), new Quaternion());
            obj.name = forestSpiritPrefab.name + "_" + i;
            ref_manager.AddObject(obj);
        }
    }

    private void SpawnItems()
    {
        GameObject obj1 = Instantiate(treasurePrefab, new Vector3(0f, 0f, 0f), new Quaternion());
        ref_manager.AddObject(obj1);

        GameObject obj2 = Instantiate(playerPrefab, new Vector3(0f, 0f, 0f), new Quaternion());
        obj2.name = playerPrefab.name; 
        ref_manager.AddObject(obj2);
        

    }
}

