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

    private Framework framework = null;

    // Start is called before the first frame update
    void Start()
    {
        framework = new Framework();

        GameObject wall1 = framework
            .Blueprint(mazeWallFw)
            .Construct(buildingColumnPreFab, "ColumnAnchor")
            .Construct(wallSegment01PreFab, "Wall1Anchor")
            .Construct(wallSegment02PreFab, "Wall2Anchor")
            .Build();

        GameObject wall2 = framework
            .Blueprint(mazeWallFw)
            .Construct(buildingColumnPreFab, "ColumnAnchor")
            .Construct(wallSegment01PreFab, "Wall1Anchor")
            .Construct(wallSegment02PreFab, "Wall2Anchor")
            .Build();

        GameObject wall3 = framework
            .Blueprint(mazeWallFw)
            .Construct(buildingColumnPreFab, "ColumnAnchor")
            .Construct(wallSegment01PreFab, "Wall1Anchor")
            .Construct(wallSegment02PreFab, "Wall2Anchor")
            .Build(new Vector3(0.0f, 90.0f, 0.0f));

        GameObject wall4 = framework
            .Blueprint(mazeWallFw)
            .Construct(buildingColumnPreFab, "ColumnAnchor")
            .Construct(wallSegment01PreFab, "Wall1Anchor")
            .Construct(wallSegment02PreFab, "Wall2Anchor")
            .Build(new Vector3(0.0f, 90.0f, 0.0f));

        GameObject floor = framework
            .Blueprint(mazePathFloorFw)
            .Construct(buildingFloor01PreFab, "CenterAnchor")
            .Construct(wall1, "EastAnchor")
            .Construct(wall2, "WestAnchor")
            .Construct(wall3, "NorthAnchor")
            .Construct(wall4, "SouthAnchor")
            .Construct(buildingColumnPreFab, "NorthEastAnchor")
            .Construct(buildingColumnPreFab, "NorthWestAnchor")
            .Build();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
