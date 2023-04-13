using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WcTileIndex 
{
    public int X { get; private set; }
    public int Y { get; private set; }

    public WcTileIndex(int x, int y) 
    {
        this.X = x;
        this.Y = y;
    }

    public override bool Equals(object obj)
    {
        return obj is WcTileIndex index &&
               X == index.X &&
               Y == index.Y;
    }

    public override int GetHashCode()
    {
        int hashCode = -1831622508;
        hashCode = hashCode * -1521134295 + X.GetHashCode();
        hashCode = hashCode * -1521134295 + Y.GetHashCode();
        return hashCode;
    }
}
