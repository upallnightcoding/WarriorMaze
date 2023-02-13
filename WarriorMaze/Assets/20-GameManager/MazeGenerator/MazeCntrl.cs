using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCntrl : MonoBehaviour
{
    [SerializeField] private MazeData mazeData;

    private Framework framework = null;

    // Start is called before the first frame update
    void Start()
    {
        framework = new Framework();

        Debug.Log("Maze has been created ...");
        MazeGenerator maze = mazeData.GenerateMaze();

        Display(maze);
    }

    private void Display(MazeGenerator maze)
    {
        Vector3 position = Vector3.zero;
        float cellSize = mazeData.cellSize;

        for (int row = 0; row < maze.Height; row++) 
        {
            for (int col = 0; col < maze.Width; col++)
            {
                CreateMazePath(maze, col, row, position);
                position.x += cellSize;
            }

            position.x = 0.0f;
            position.z += cellSize;
        }
    }

    private void CreateMazePath(MazeGenerator maze, int col, int row, Vector3 position) 
    {
        MazeCell mazeCell = maze.GetMazeCell(col, row);

        if ((mazeCell != null) && (mazeCell.IsVisited()))
        {
            mazeCell.Position = position;

            GameObject north = (mazeCell.HasNorthWall()) ? mazeData.CreateNorthSouthWall(framework) : null;
            GameObject south = (mazeCell.HasSouthWall()) ? mazeData.CreateNorthSouthWall(framework) : null;
            GameObject east = (mazeCell.HasEastWall()) ? mazeData.CreateEastWestWall(framework) : null;
            GameObject west = (mazeCell.HasWestWall()) ? mazeData.CreateEastWestWall(framework) : null;

            mazeData.CreatePath(framework, north, south, east, west, position);
        }
    }
}
