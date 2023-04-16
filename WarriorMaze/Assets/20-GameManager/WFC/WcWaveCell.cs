using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WcWaveCell 
{
    public int X { get; set; }
    public int Y { get; set; }
    
    private List<int> availableTiles;
    private Vector3 position;

    public int GetEntropy() => availableTiles.Count;
    //public List<int> GetAvailableTiles() => availableTiles;
    public int GetCollapsedTile() => availableTiles[0];
    
    public bool IsCollapsed() => GetEntropy() == 1;
    public bool IsLessThanValidCell(int value) => (IsLessThan(value) && IsValidEntropy());
    public bool IsEqualTo(int value) => (value == GetEntropy());

    public bool IsInError() => (availableTiles.Count == 0);

    public bool IsValidEntropy() => GetEntropy() > 0;
    
    private bool IsLessThan(int value) => (GetEntropy() < value);

    public WcWaveCell(int x, int y, List<int> availableTiles)
    {
        this.availableTiles = availableTiles;
        this.position = new Vector3(x, 0.0f, y);
        this.X = x;
        this.Y = y;
    }

    public void apply(List<int> rules)
    {
        availableTiles = new List<int>(availableTiles).Intersect(rules).ToList<int>();
    }

    public void Collapse()
    {
        int index = availableTiles[Random.Range(0, availableTiles.Count)];

        availableTiles.Clear();
        availableTiles.Add(index);
    }
}
