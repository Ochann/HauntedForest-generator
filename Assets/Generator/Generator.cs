using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class Generator : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField, Range(0,100)] private int fillRate;

    int[,] grid;

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        grid = new int[width, height];
        System.Random rand = new System.Random();
        for (int x = 0; x < width; x++) 
        {
            for (int y = 0; y < height; y++)
            {
                grid[x,y] = (rand.Next(0,100) < fillRate) ? 1: 0;
            }
        }
    }


    //int size = 5;
    //void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        Debug.Log("-1");
    //        size --;
    //    }
    //}

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawCube(new Vector3(0, 0, 0), new Vector3(size,size,size));
    //}

    void OnDrawGizmos()
    {
        if (grid != null)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Gizmos.color = (grid[x, y] == 1) ? Color.black : Color.white;
                    Vector3 pos = new Vector3(-width / 2 + x + .5f, 0, -height / 2 + y + .5f);
                    Gizmos.DrawCube(pos, Vector3.one);
                }
            }
        }
    }

}
