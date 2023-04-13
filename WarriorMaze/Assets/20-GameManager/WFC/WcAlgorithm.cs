using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WcAlgorithm 
{
    private WcTileManager tileMngr;
    private int width;
    private int height;

    private IDictionary<WcTileIndex, WcWaveCell> wave;

    private Tuple<int, int, WcDirection>[] neighbors = {
        Tuple.Create( 0, -1, WcDirection.NORTH),    // North
        Tuple.Create( 0,  1, WcDirection.SOUTH),    // South
        Tuple.Create( 1,  0, WcDirection.EAST),     // East
        Tuple.Create(-1,  0, WcDirection.WEST)      // West
    };

    public WcAlgorithm(WcTileManager tileMngr, int width, int height) 
    {
        this.tileMngr = tileMngr;
        this.width = width;
        this.height = height;
    }

    public void RunAlgorithm()
    {
        InitializeWFC();

        WcWaveCell cell = FindLowestEntropyCell();

        cell.Collapse();

        PropagateEntropy(cell);
    }

    private void InitializeWFC() 
    {
        wave = new Dictionary<WcTileIndex, WcWaveCell>();

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                WcTileIndex index = new WcTileIndex(x, y);
                WcWaveCell cell = new WcWaveCell(x, y, tileMngr.GetAllTilesIds());
                wave.Add(index, cell);
            }
        }
    }                  

    private void PropagateEntropy(WcWaveCell cell)
    {
        foreach (Tuple<int, int, WcDirection> neighbor in neighbors) {
            CellLookUp(cell, neighbor);
        }
    }

    private bool PropagateEntropy()
    {
        int countNotCollapsed = 0;

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
               WcWaveCell cell = GetWaveCell(x, y);

               if (!cell.IsCollapsed()) {
                    foreach (Tuple<int, int, WcDirection> neighbor in neighbors) {
                        CellLookUp(cell, neighbor);
                    }
                    countNotCollapsed++;
               } 
            }
        }

        return(countNotCollapsed > 0);
    }

    private void CellLookUp(WcWaveCell cell, Tuple<int, int, WcDirection> neighbor) {
        int x = cell.X + neighbor.Item1;
        int y = cell.Y + neighbor.Item2;
        WcDirection direction = neighbor.Item3;

        WcWaveCell offsetCell = GetWaveCell(x, y);

        if (offsetCell != null) {
            List<int> offsetAvailTiles = offsetCell.GetAvailableTiles();

            int id = cell.GetCollapsedTile();
            WcTileModel model = tileMngr.GetTile(id);
            List<int> rules = model.GetRules(direction);

            offsetCell.SetAvailableTiles(rules);
        }
    }

    private void Collapse(WcWaveCell cell) {
        cell.Collapse();
    }

    private WcWaveCell FindLowestEntropyCell() {
        List<WcWaveCell> minCellList = new List<WcWaveCell>();
        int lowestEntropy = tileMngr.GetnTiles();

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                WcWaveCell cell = GetWaveCell(x, y);

                if (cell.IsEqualTo(lowestEntropy)) {
                    minCellList.Add(cell);
                } else if (cell.IsLessThanValidCell(lowestEntropy)) {
                    lowestEntropy = cell.GetEntropy();
                    minCellList.Clear();
                    minCellList.Add(cell);
                }
            }
        }

        return(minCellList[UnityEngine.Random.Range(0, minCellList.Count)]);
    }

    private WcWaveCell GetWaveCell(int x, int y) {
        WcWaveCell cell = null;

        if (!wave.TryGetValue(new WcTileIndex(x, y), out cell)) {
            cell = null;
        }

        return(cell);
    }
}

public enum WcDirection { 
    NORTH, 
    SOUTH, 
    EAST, 
    WEST 
}
