using System.Collections.Generic;
using System.Linq;

public class WcTileManager 
{
    public List<int> GetAllTilesIds() => models.Select(t => t.GetId()).ToList<int>();

    public int GetnTiles() => models.Length;

    public WcTileModel GetTile(int id) => models.First(tile => (tile.GetId() == id));

    private WcTileModel[] models;

    public WcTileManager(WcTileModel[] models)
    {
        this.models = models;
    }
}
