using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MazeRenderer : MonoBehaviour
{
    //VARS

    [SerializeField][Min(0f)] private int Width = 10;
    [SerializeField][Min(0f)] private int Height = 10;
    [SerializeField][Min(0f)] private int Seed = 0;
    [SerializeField][Min(0f)] private int NumOfSaws;
    [SerializeField][Min(0f)] private int NumOfPoints;
    [SerializeField] [Min(0f)] private int NumOfHearts;

    private float size = 1f;

    [SerializeField] private Transform wallPrefab = null;
    [SerializeField] private Transform floorPrefab = null;
    [SerializeField] private Transform icyFloorPrefab = null;
    [SerializeField] private Transform honeyPrefab = null;
    [SerializeField] private Transform sawPrefab = null;
    [SerializeField] private Transform playerPrefab = null;
    [SerializeField] private Transform pointPrefab = null;
    [SerializeField] private Transform heartPrefab = null;

    [SerializeField] private Transform startObject = null;
    [SerializeField] private Transform endObject = null;


    void Start()
    {
        var maze = MazeGenerator.Generate(Width,Height,Seed);
        Draw(maze);
    }

    void GenerateSaws()
    {
        var r = new System.Random();
        for (int i = 0; i < NumOfSaws; i++)
        {
            int rX = r.Next((-Width - 1) / 2, Height / 2);
            int rY = r.Next((-Height - 1) / 2, Height / 2);
            var sawblade = Instantiate(sawPrefab, transform);
            var sawController = GetComponentInChildren<SawMovement>();
            if( i % 2==0 ) sawController.toggleXAxis();
            sawblade.position = new Vector3(rX, 0.5f, rY);
        }
    }

    void GeneratePoints()
    {
        var r = new System.Random();
        for (int i = 0; i < NumOfPoints; i++)
        {
            int rX = r.Next((-Width - 1) / 2, Height / 2);
            int rY = r.Next((-Height - 1) / 2, Height / 2);
            var point = Instantiate(pointPrefab, transform);
            point.position = new Vector3(rX, 0.25f, rY);
        }
    }
    void GenerateHearts()
    {
        var r = new System.Random(23);
        for (int i = 0; i < NumOfHearts; i++)
        {
            int rX = r.Next((-Width - 1) / 2, Height / 2);
            int rY = r.Next((-Height - 1) / 2, Height / 2);
            var point = Instantiate(heartPrefab, transform);
            point.position = new Vector3(rX, 0f, rY);
        }
    }

    void generateStartEnd()
    {
        var startPlatform = Instantiate(startObject, transform);
        startPlatform.position = new Vector3(0, 0, 0);

        var endPlatform = Instantiate(endObject, transform);
        var rnd = Random.Range(1, 4);
        switch (rnd)
        {
            case 1:
                endPlatform.position = new Vector3(-Width / 2, 0, (Height - 1) / 2);
                break;            
            case 2:
                endPlatform.position = new Vector3((Width - 1) / 2, 0, -Height / 2);
                break;            
            case 3:
                endPlatform.position = new Vector3((Width - 1) / 2, 0, (Height - 1) / 2);
                break;            
            case 4:
                endPlatform.position = new Vector3(-Width / 2, 0, -Height / 2);
                break;
        }
    }

    public void generatePlayer()
    {
        playerPrefab.position = new Vector3(0, 0, 0);
    }

    void placeIceFloor(Vector3 position)
    {
        var icyFloor = Instantiate(icyFloorPrefab, transform);
        icyFloor.localScale = new Vector3(icyFloor.localScale.x, icyFloor.localScale.y, icyFloor.localScale.z);
        icyFloor.position = position + new Vector3(0, -1, 0);
    }
    void placeHoney(Vector3 position)
    {
        var honeyFloor = Instantiate(honeyPrefab, transform);
        honeyFloor.localScale = new Vector3(honeyFloor.localScale.x, honeyFloor.localScale.y, honeyFloor.localScale.z);
        honeyFloor.position = position + new Vector3(0, -1, 0);
    }
    void placeFloor(Vector3 position)
    {
        var floor = Instantiate(floorPrefab, transform);
        floor.localScale = new Vector3(floor.localScale.x, floor.localScale.y, floor.localScale.z);
        floor.position = position + new Vector3(0, -1, 0);
    }

    private void Draw(Wall[,] maze)
    {

        generatePlayer();
        generateStartEnd();
        GenerateSaws();
        GeneratePoints();
        GenerateHearts();

        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                var cell = maze[i, j];
                var position = new Vector3(-Width / 2 + i, 1f, -Height / 2 + j);//OFFSET of the middle of the cell, so there's a path
                var rnd = Random.Range(1, 11);

                if(rnd > 6 && rnd <=9)
                {
                    placeIceFloor(position);
                }
                else if(rnd > 9)
                {
                    placeHoney(position);
                }
                else
                {
                    placeFloor(position);
                }

                if (cell.HasFlag(Wall.UP))
                {
                    var topWall = Instantiate(wallPrefab, transform) as Transform;
                    topWall.position = position + new Vector3(0, 0, size/2);
                    topWall.localScale = new Vector3(size, topWall.localScale.y, topWall.localScale.z);
                }

                if (cell.HasFlag(Wall.LEFT))
                {
                    var leftWall = Instantiate(wallPrefab, transform) as Transform;
                    leftWall.position = position + new Vector3(-size / 2, 0, 0);
                    leftWall.localScale = new Vector3(size, leftWall.localScale.y, leftWall.localScale.z);
                    leftWall.eulerAngles = new Vector3(0, 90, 0);
                }
                if (i == Width - 1)
                {
                    if (cell.HasFlag(Wall.RIGHT))
                    {
                        var rightWall = Instantiate(wallPrefab, transform) as Transform;
                        rightWall.position = position + new Vector3(size / 2, 0, 0);
                        rightWall.localScale = new Vector3(size, rightWall.localScale.y, rightWall.localScale.z);
                        rightWall.eulerAngles = new Vector3(0, 90, 0);
                    }
                }
                if (j == 0)
                {
                    if (cell.HasFlag(Wall.DOWN))
                    {
                        var bottomWall = Instantiate(wallPrefab, transform) as Transform;
                        bottomWall.position = position + new Vector3(0, 0, -size / 2);
                        bottomWall.localScale = new Vector3(size, bottomWall.localScale.y, bottomWall.localScale.z);
                    }
                }

            }
        }
    }
}
