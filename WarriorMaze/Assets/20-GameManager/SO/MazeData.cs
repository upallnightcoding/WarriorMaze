using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MazeData", menuName = "Maze Wizard/MazeData")]
public class MazeData : ScriptableObject
{
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

    public GameObject CreateNorthSouthWall(Framework framework)
    {
        GameObject wall = framework
            .Blueprint(mazeWallFw)
            .Assemble(buildingColumnPreFab, "ColumnAnchor")
            .Assemble(mazeWallsSegmentsPreFab, "Wall1Anchor")
            .Assemble(mazeWallsSegmentsPreFab, "Wall2Anchor")
            .Build(new Vector3(0.0f, 90.0f, 0.0f));

        return(wall);
    }

    public GameObject CreateEastWestWall(Framework framework)
    {
        GameObject wall = framework
            .Blueprint(mazeWallFw)
            .Assemble(buildingColumnPreFab, "ColumnAnchor")
            .Assemble(mazeWallsSegmentsPreFab, "Wall1Anchor")
            .Assemble(mazeWallsSegmentsPreFab, "Wall2Anchor")
            .Build();

        return(wall);
    }

    public void CreatePath(
        Framework framework, 
        GameObject northWall, GameObject southWall,
        GameObject eastWall, GameObject westWall,
        Vector3 position
    )
    {
         GameObject path = framework
            .Blueprint(mazePathFloorFw)
            .Assemble(buildingFloor01PreFab, "CenterAnchor")
            .Assemble(northWall, "NorthAnchor")
            .Assemble(southWall, "SouthAnchor")
            .Assemble(eastWall, "EastAnchor")
            .Assemble(westWall, "WestAnchor")
            .Assemble(buildingColumnPreFab, "NorthEastAnchor")
            .Assemble(buildingColumnPreFab, "NorthWestAnchor")
            .Assemble(buildingColumnPreFab, "SouthEastAnchor")
            .Assemble(buildingColumnPreFab, "SouthWestAnchor")
            .Position(position)
            .Build();
    }
}
