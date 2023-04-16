using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WcTileModel", menuName = "Maze Wizard/Wc Tile")]
public class WcTileModel : ScriptableObject
{
    [SerializeField] private int id;
    [SerializeField] private GameObject preFab;
    [SerializeField] private List<int> tilesNorth;
    [SerializeField] private List<int> tilesSouth;
    [SerializeField] private List<int> tilesEast;
    [SerializeField] private List<int> tilesWest;

    public int GetId() => id;

    public GameObject GetPreFab() => preFab;

    public List<int> GetRules(WcDirection direction)
    {
        List<int> tiles = null;

        switch(direction) 
        {
            case WcDirection.NORTH:
                tiles = tilesNorth;
                break;
            case WcDirection.SOUTH:
                tiles = tilesSouth;
                break;
            case WcDirection.EAST:
                tiles = tilesEast;
                break;
            case WcDirection.WEST:
                tiles = tilesWest;
                break;
        }

        return(tiles);
    }
}
