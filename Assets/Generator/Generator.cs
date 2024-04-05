using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using UnityEngine.XR;
using UnityEngine.Tilemaps;

public class Generator : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField, Range(0,100)] private int fillRate;

    int[,] grid;

    public Tilemap tilemap;
    public TileBase terrain_0;

    private void Start() {
        GenerateGrid();
    }

    private void GenerateGrid() {
        grid = new int[width, height];
        System.Random rand = new System.Random();
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                grid[x,y] = (rand.Next(0,100) < fillRate) ? 1: 0;
            }
        }
    }

    private void CellularAutomata() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                int walls = GetSurroundingWalls(x, y);
                if (walls > 4)
                    grid[x,y] = 1;
                else if(walls < 4)
                    grid[x,y] = 0;
            }
        }
    }

    private int GetSurroundingWalls(int gridX, int gridY) {
        int surroundingWalls = 0;
        for(int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++) {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++) {
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height) {
                    if (neighbourX != gridX || neighbourY != gridY)
                    {
                        surroundingWalls += grid[neighbourX, neighbourY];
                    }
                }
                else {
                    surroundingWalls++;
                }
            }
        }

        return surroundingWalls;
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Debug.Log("CA");
            CellularAutomata();
            UpDateTilemap();
        }
        else if (Input.GetMouseButtonDown(1)) {
            Debug.Log("init");
            GenerateGrid();
        }
    }


    private void OnDrawGizmos() {
        if (grid != null) {
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    Gizmos.color = (grid[x, y] == 1) ? Color.black : Color.white;
                    Vector3 pos = new Vector3(-width / 2 + x + .5f, -height / 2 + y + .5f, 0);
                    Gizmos.DrawCube(pos, Vector3.one);
                }
            }
        }
    }

    private void UpDateTilemap() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                Vector3Int pos = new Vector3Int(x, y, 0) - new Vector3Int(width/2, height/2, 0);
                if (grid[x, y] == 1) {
                    
                    tilemap.SetTile(pos, terrain_0);
                }
                else
                {
                    tilemap.SetTile(pos, null);
                }
            }
        }
    }

}
