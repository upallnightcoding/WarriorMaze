using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WcTileManager 
{
    public List<int> GetAllTilesIds() => models.Select(t => t.GetId()).ToList<int>();

    public int GetnTiles() => models.Length;

    public WcTileModel GetTile(int id) => models.First(tile => (tile.GetId() == id));

    public GameObject GetError() => error;

    public GameObject GetBlank() => blank;

    public int Width { get; set;}
    public int Height { get; set; }

    private WcTileModel[] models;

    private GameObject error;
    private GameObject blank;

    public WcTileManager(int width, int height, WcTileModel[] models, GameObject error, GameObject blank)
    {
        this.Width = width;
        this.Height = height;
        this.models = models;
        this.error = error;
        this.blank = blank;
    }

    public List<int> GetRules(WcWaveCell cell, WcDirection direction)
    {
        int id = cell.GetCollapsedTile();
        WcTileModel model = GetTile(id);
        return(model.GetRules(direction));
    }
}
