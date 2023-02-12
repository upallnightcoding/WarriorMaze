using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject mazeWallFw;
    [SerializeField] private GameObject buildingColumnPreFab;
    [SerializeField] private GameObject wallSegment01PreFab;
    [SerializeField] private GameObject wallSegment02PreFab;

    [SerializeField] private GameObject mazePathFloorFw;
    [SerializeField] private GameObject buildingFloor01PreFab;

    [SerializeField] private GameObject[] walls;

    private Framework framework = null;

    // Start is called before the first frame update
    void Start()
    {
        framework = new Framework();

        GameObject wall1 = framework
            .Blueprint(mazeWallFw)
            .Assemble(buildingColumnPreFab, "ColumnAnchor")
            .Assemble(walls, "Wall1Anchor")
            .Assemble(walls, "Wall2Anchor")
            .Build();

        GameObject wall2 = framework
            .Blueprint(mazeWallFw)
            .Assemble(buildingColumnPreFab, "ColumnAnchor")
            .Assemble(walls, "Wall1Anchor")
            .Assemble(walls, "Wall2Anchor")
            .Build();

        GameObject wall3 = framework
            .Blueprint(mazeWallFw)
            .Assemble(buildingColumnPreFab, "ColumnAnchor")
            .Assemble(walls, "Wall1Anchor")
            .Assemble(walls, "Wall2Anchor")
            .Build(new Vector3(0.0f, 90.0f, 0.0f));

        GameObject wall4 = framework
            .Blueprint(mazeWallFw)
            .Assemble(buildingColumnPreFab, "ColumnAnchor")
            .Assemble(walls, "Wall1Anchor")
            .Assemble(walls, "Wall2Anchor")
            .Build(new Vector3(0.0f, 90.0f, 0.0f));

        GameObject floor = framework
            .Blueprint(mazePathFloorFw)
            .Assemble(buildingFloor01PreFab, "CenterAnchor")
            .Assemble(wall1, "EastAnchor")
            .Assemble(wall2, "WestAnchor")
            .Assemble(wall3, "NorthAnchor")
            .Assemble(wall4, "SouthAnchor")
            .Assemble(buildingColumnPreFab, "NorthEastAnchor")
            .Assemble(buildingColumnPreFab, "NorthWestAnchor")
            .Assemble(buildingColumnPreFab, "SouthEastAnchor")
            .Assemble(buildingColumnPreFab, "SouthWestAnchor")
            .Build();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
