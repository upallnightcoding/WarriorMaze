using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeIndex 
{
    public int Col { get; private set; }
    public int Row { get; private set; }

    public MazeIndex(int col, int row) 
    {
        this.Col = col;
        this.Row = row;
    }

    public override bool Equals(object obj)
    {
        return obj is MazeIndex index &&
               Col == index.Col &&
               Row == index.Row;
    }

    public override int GetHashCode()
    {
        int hashCode = -1831622508;
        hashCode = hashCode * -1521134295 + Col.GetHashCode();
        hashCode = hashCode * -1521134295 + Row.GetHashCode();
        return hashCode;
    }
}
