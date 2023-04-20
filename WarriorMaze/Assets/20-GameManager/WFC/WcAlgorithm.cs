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
    private List<GameObject> deleteList;

    private Tuple<int, int, WcDirection>[] neighbors = {
        Tuple.Create( 0,  1, WcDirection.NORTH),    // North
        Tuple.Create( 0, -1, WcDirection.SOUTH),    // South
        Tuple.Create( 1,  0, WcDirection.EAST),     // East
        Tuple.Create(-1,  0, WcDirection.WEST)      // West
    };

    private int Random(int n) => UnityEngine.Random.Range(0, n);

    public WcAlgorithm(WcTileManager tileMngr) 
    {
        this.tileMngr = tileMngr;
        this.width = tileMngr.Width;
        this.height = tileMngr.Height;

        this.deleteList = new List<GameObject>();
    }

    public void RunAlgorithm(int iteration)
    {
        bool stopIteration = false;

        InitializeWFC();

        for (int i = 0; (i < iteration) && !stopIteration; i++) {
            WcWaveCell cell = FindLowestEntropyCell();

            stopIteration = (cell == null);

            if (!stopIteration) {
                cell.Collapse();
                PropagateEntropy(cell);
            }
        }

        RenderWave();
    }

    private void RenderWave()
    {
        float step = 6.0f;
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                WcWaveCell cell = GetWaveCell(x, y);

                Vector3 position = new Vector3(x * step, 0.0f, y * step);

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

                RenderModel(preFab, position);
            }
        }
    }

    private void RenderModel(GameObject preFab, Vector3 position)
    {
        GameObject go = 
            UnityEngine.Object.Instantiate(preFab, position, Quaternion.identity);

        deleteList.Add(go);
    }

    private void InitializeWFC() 
    {
        wave = new Dictionary<WcTileIndex, WcWaveCell>();

        deleteList.ForEach((go) => UnityEngine.Object.Destroy(go));

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

        while ((stack.Count != 0) && (count++ < 500)) {
            WcWaveCell pivot = stack.Pop();

            foreach (Tuple<int, int, WcDirection> neighbor in neighbors) {
                WcWaveCell target = CellLookUp(pivot, neighbor);
                if ((target != null) && (target.IsCollapsed())) {
                    stack.Push(target);
                }
            }
        }
    }

    private WcWaveCell CellLookUp(WcWaveCell pivot, Tuple<int, int, WcDirection> neighbor) {
        int x = pivot.X + neighbor.Item1;
        int y = pivot.Y + neighbor.Item2;
        WcWaveCell target = GetWaveCell(x, y);

        if ((target != null) && (!target.IsInError()) && (!target.IsCollapsed())) {
            WcDirection direction = neighbor.Item3;
            List<int> rules = tileMngr.GetRules(pivot, direction);

            target.apply(rules);
        } else {
            target = null;
        }

        return(target);
    }

    private WcWaveCell FindLowestEntropyCell() {
        List<WcWaveCell> minCellList = new List<WcWaveCell>();
        int lowestEntropy = tileMngr.GetnTiles();

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                WcWaveCell cell = GetWaveCell(x, y);

                if (!cell.IsCollapsed()) {
                    if (cell.IsLessThanValidCell(lowestEntropy)) {
                        lowestEntropy = cell.GetEntropy();
                        minCellList.Clear();
                        minCellList.Add(cell);
                    } else if (cell.IsEqualTo(lowestEntropy)) {
                        minCellList.Add(cell);
                    }
                }
            }
        }

        WcWaveCell minimal = 
            (minCellList.Count != 0) ? minCellList[Random(minCellList.Count)] : null;

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
