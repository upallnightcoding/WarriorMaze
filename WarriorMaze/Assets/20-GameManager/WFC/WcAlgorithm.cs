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
        Tuple.Create( 0,  1, WcDirection.NORTH),    // North
        Tuple.Create( 0, -1, WcDirection.SOUTH),    // South
        Tuple.Create( 1,  0, WcDirection.EAST),     // East
        Tuple.Create(-1,  0, WcDirection.WEST)      // West
    };

    public WcAlgorithm(WcTileManager tileMngr) 
    {
        this.tileMngr = tileMngr;
        this.width = tileMngr.Width;
        this.height = tileMngr.Height;
    }

    public void RunAlgorithm(int iteration)
    {
        InitializeWFC();

        for (int i = 0; i < iteration; i++) {
            WcWaveCell cell = FindLowestEntropyCell();

            cell.Collapse();

            PropagateEntropy(cell);
            //PropagateEntropy();
        }

        RenderWave();
    }

    private void RenderWave()
    {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                WcWaveCell cell = GetWaveCell(x, y);

                Vector3 position = new Vector3(x, 0.0f, y);

                GameObject preFab;

                if (cell.IsCollapsed()) {
                    int index = cell.GetCollapsedTile();
                    WcTileModel model = tileMngr.GetTile(index);
                    preFab = model.GetPreFab();
                } else if (cell.IsInError()) {
                    preFab = tileMngr.GetError();
                } else {
                    preFab = tileMngr.GetBlank();
                }

                UnityEngine.Object.Instantiate(preFab, position, Quaternion.identity);
            }
        }
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

    private void PropagateEntropy(WcWaveCell startingPoint)
    {
        Stack<WcWaveCell> stack = new Stack<WcWaveCell>();
        int count = 0;

        stack.Push(startingPoint);

        Debug.Log($"(Starting Point: {startingPoint.X} {startingPoint.Y}) {startingPoint.GetCollapsedTile()}");

        while ((stack.Count != 0) && (count++ < 500)) {
            WcWaveCell pivot = stack.Pop();

            foreach (Tuple<int, int, WcDirection> neighbor in neighbors) {
                WcWaveCell target = CellLookUp(pivot, neighbor);
                if ((target != null) && (target.IsCollapsed())) {
                    stack.Push(target);
                }
            }
        }

        Debug.Log($"Count: {count}");
    }

    private void PropagateEntropy1(WcWaveCell cell)
    {
        foreach (Tuple<int, int, WcDirection> neighbor in neighbors) {
            CellLookUp(cell, neighbor);
        }
    }

    private WcWaveCell CellLookUp(WcWaveCell pivot, Tuple<int, int, WcDirection> neighbor) {
        int x = pivot.X + neighbor.Item1;
        int y = pivot.Y + neighbor.Item2;
        WcDirection direction = neighbor.Item3;

        WcWaveCell target = GetWaveCell(x, y);

        if ((target != null) && (!target.IsInError()) && (!target.IsCollapsed())) {
            List<int> offsetAvailTiles = target.GetAvailableTiles();

            int id = pivot.GetCollapsedTile();
            WcTileModel model = tileMngr.GetTile(id);
            List<int> rules = model.GetRules(direction);

            target.SetAvailableTiles(rules);
        } else {
            target = null;
        }

        return(target);
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
                Debug.Log($"({cell.X} {cell.Y}) {cell.IsCollapsed()} {cell.GetEntropy()}");

                if (!cell.IsCollapsed()) {
                    if (cell.IsLessThanValidCell(lowestEntropy)) {
                        lowestEntropy = cell.GetEntropy();
                        minCellList.Clear();
                        minCellList.Add(cell);
                        Debug.Log($"({cell.X} {cell.Y}) Lowest: {lowestEntropy}");
                    } else if (cell.IsEqualTo(lowestEntropy)) {
                        minCellList.Add(cell);
                        Debug.Log($"({cell.X} {cell.Y}) Add: {lowestEntropy}");
                    }
                }
            }
        }

        WcWaveCell minimal = minCellList[UnityEngine.Random.Range(0, minCellList.Count)];
        Debug.Log($"Minimal: ({minimal.X} {minimal.Y})");

        return(minimal);
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
