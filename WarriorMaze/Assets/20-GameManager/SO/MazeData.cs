using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MazeData", menuName = "Maze Wizard/MazeData")]
public class MazeData : ScriptableObject
{
    public static readonly uint NW = 8;
    public static readonly uint SW = 4;
    public static readonly uint SE = 2;
    public static readonly uint NE = 1;

    public static readonly uint N = 8;
    public static readonly uint S = 4;
    public static readonly uint E = 2;
    public static readonly uint W = 1;

    private readonly string CENTER_ANCHOR = "CenterAnchor";
    private readonly string NORTH_ANCHOR = "NorthAnchor";
    private readonly string SOUTH_ANCHOR = "SouthAnchor";
    private readonly string EAST_ANCHOR = "EastAnchor";
    private readonly string WEST_ANCHOR = "WestAnchor";

    private readonly string NORTH_EAST_ANCHOR = "NorthEastAnchor";
    private readonly string NORTH_WEST_ANCHOR = "NorthWestAnchor";
    private readonly string SOUTH_EAST_ANCHOR = "SouthEastAnchor";
    private readonly string SOUTH_WEST_ANCHOR = "SouthWestAnchor";

    [Header("Maze Data")]
    public int height;
    public int width;
    public int cellSize;

    [Header("Framework")]
    public GameObject mazeWallFw;
    public GameObject mazePathFloorFw;

    [Header("PreFabs")]
    public GameObject buildingColumnPreFab;
    public GameObject buildingFloor01PreFab;

    [Space]
    public GameObject[] mazeWallsSegmentsPreFab;

    private MazeGenerator maze; 

    public MazeGenerator GetMaze() { return(maze); }

    public MazeGenerator GenerateMaze()
    {
        maze = new MazeGenerator(width, height);

        maze.Generate();

        return(maze);
    }

    public void CreatePath(Framework framework, MazeCell mazeCell, Vector3 position, uint columns, uint walls)
    {
        GameObject northWall = (mazeCell.HasNorthWall() && ((walls & N) > 0)) ? CreateNorthSouthWall(framework) : null;
        GameObject southWall = (mazeCell.HasSouthWall() && ((walls & S) > 0)) ? CreateNorthSouthWall(framework) : null;
        GameObject eastWall  = (mazeCell.HasEastWall() &&  ((walls & E) > 0)) ? CreateEastWestWall(framework) : null;
        GameObject westWall  = (mazeCell.HasWestWall() &&  ((walls & W) > 0)) ? CreateEastWestWall(framework) : null;

        mazeCell.Position = position;

        GameObject path = framework
            .Blueprint(mazePathFloorFw)
            .Assemble(buildingFloor01PreFab, CENTER_ANCHOR)
            .Assemble(northWall, NORTH_ANCHOR)
            .Assemble(southWall, SOUTH_ANCHOR)
            .Assemble(eastWall, EAST_ANCHOR)
            .Assemble(westWall, WEST_ANCHOR)
            .Assemble(buildingColumnPreFab, NORTH_EAST_ANCHOR, (columns & NE) > 0)
            .Assemble(buildingColumnPreFab, NORTH_WEST_ANCHOR, (columns & NW) > 0)
            .Assemble(buildingColumnPreFab, SOUTH_EAST_ANCHOR, (columns & SE) > 0)
            .Assemble(buildingColumnPreFab, SOUTH_WEST_ANCHOR, (columns & SW) > 0)
            .Position(position)
            .Build();
    }

    private GameObject CreateNorthSouthWall(Framework framework)
    {
        GameObject wall = framework
            .Blueprint(mazeWallFw)
            .Assemble(buildingColumnPreFab, "ColumnAnchor")
            .Assemble(mazeWallsSegmentsPreFab, "Wall1Anchor")
            .Assemble(mazeWallsSegmentsPreFab, "Wall2Anchor")
            .Build(new Vector3(0.0f, 90.0f, 0.0f));

        return(wall);
    }

    private GameObject CreateEastWestWall(Framework framework)
    {
        GameObject wall = framework
            .Blueprint(mazeWallFw)
            .Assemble(buildingColumnPreFab, "ColumnAnchor")
            .Assemble(mazeWallsSegmentsPreFab, "Wall1Anchor")
            .Assemble(mazeWallsSegmentsPreFab, "Wall2Anchor")
            .Build();

        return(wall);
    }
}
