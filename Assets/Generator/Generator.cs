using UnityEngine;
using System;
using UnityEngine.Tilemaps;

public class Generator : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField, Range(0, 100)] private int forestFillRate;
    [SerializeField, Range(0, 100)] private int dirtFillRate;

    int[,] grid;
    int[,] forestGrid;

    public Tilemap forestMap;
    public Tilemap dirtMap;

    public TileBase treetop;
    public TileBase dirt;
    public TileBase grass;

    enum Map
    {
        Forest,
        Dirt
    }

    private void Start()
    {
        GenerateMap();
    }

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
                grid[x, y] = (rand.Next(0, 100) < fillRate) ? 1 : 0;
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
            GenerateMap();
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
    }

}

