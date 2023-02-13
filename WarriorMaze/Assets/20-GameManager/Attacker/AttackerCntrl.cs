using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackerCntrl : MonoBehaviour
{
    [SerializeField] MazeData mazeData;
    [SerializeField] GameObject orc;

    private MazeGenerator maze;

    private bool created = false;

    // Start is called before the first frame update
    void Start()
    {
        maze = mazeData.GetMaze();
    }

    // Update is called once per frame
    void Update()
    {
        if (!created) 
        {
            created = true;
            Instantiate(orc, maze.GetMazeCell(1, 0).Position, Quaternion.identity);
        }
    }
}
