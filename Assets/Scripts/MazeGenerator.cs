using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum Wall
{
    //0000 -> No walls
    //1111 -> Left,right,up,down
    LEFT = 1,  //0001
    RIGHT = 2, //0010
    UP = 4,    //0100
    DOWN = 8,  //1000

    VISITED = 128 //1000 0000
}

public struct Position
{
    public int X;
    public int Y;
}

public struct Neighbour
{
    public Position Position;
    public Wall SharedWall;
}
public static class MazeGenerator
{

    private static Wall getOppositeWall(Wall wall)
    {
        switch (wall)
        {
            case Wall.RIGHT: return Wall.LEFT;
            case Wall.LEFT: return Wall.RIGHT;
            case Wall.UP: return Wall.DOWN;
            case Wall.DOWN: return Wall.UP;
            default: return Wall.LEFT;
        }
    }
    private static Wall[,] generateMaze(Wall[,] maze, int width, int height,int seed)
    {
        var rng = new System.Random(seed);
        var positionStack = new Stack<Position>();
        var position = new Position { X = rng.Next(0,width), Y = rng.Next(0, height) };

        maze[position.X, position.Y] |= Wall.VISITED; // 1000 1111
        positionStack.Push(position);

        while(positionStack.Count > 0)
        {
            var current = positionStack.Pop();
            var neighbours = getUnvisitedNeighbours(current, maze, width, height);
            if(neighbours.Count > 0)
            {
                positionStack.Push(current);

                var randIndex = rng.Next(0, neighbours.Count);
                var randomNeighbour = neighbours[randIndex];

                var nPosition = randomNeighbour.Position;
                maze[current.X, current.Y] &= ~randomNeighbour.SharedWall;
                maze[nPosition.X, nPosition.Y] &= ~getOppositeWall(randomNeighbour.SharedWall);

                maze[nPosition.X, nPosition.Y] |= Wall.VISITED;

                positionStack.Push(nPosition);
            }
        }

        return maze;
    }


    private static List<Neighbour> getUnvisitedNeighbours(Position p, Wall[,] maze, int width,int height)
    {
        var list = new List<Neighbour>();

        if(p.X>0) //left
        {
            if (!maze[p.X - 1, p.Y].HasFlag(Wall.VISITED))
            {
                list.Add(new Neighbour
                {
                    Position = new Position
                    {
                        X = p.X - 1,
                        Y = p.Y
                    },
                    SharedWall = Wall.LEFT
                });
            }
        }
        if (p.Y > 0) //DOWN 
        {
            if (!maze[p.X, p.Y -1].HasFlag(Wall.VISITED))
            {
                list.Add(new Neighbour
                {
                    Position = new Position
                    {
                        X = p.X,
                        Y = p.Y - 1
                    },
                    SharedWall = Wall.DOWN
                }); ;
            }
        }
        if (p.Y < height -1) //UP 
        {
            if (!maze[p.X, p.Y + 1].HasFlag(Wall.VISITED))
            {
                list.Add(new Neighbour
                {
                    Position = new Position
                    {
                        X = p.X,
                        Y = p.Y + 1
                    },
                    SharedWall = Wall.UP
                }); ;
            }
        }
        if (p.X < width -1) //RIGHT 
        {
            if (!maze[p.X +1, p.Y].HasFlag(Wall.VISITED))
            {
                list.Add(new Neighbour
                {
                    Position = new Position
                    {
                        X = p.X+1,
                        Y = p.Y
                    },
                    SharedWall = Wall.RIGHT
                }); ;
            }
        }
        return list;
    }

    public static Wall[,] Generate(int width, int height, int seed)
    {
        Wall[,] maze = new Wall[width, height];
        Wall initial = Wall.RIGHT | Wall.LEFT | Wall.UP | Wall.DOWN;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                maze[i, j] = initial; //all values 1111
            }
        }
        
        return generateMaze(maze,width,height,seed);
    }

  
}
