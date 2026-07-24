using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using static System.Math;

public class Stage
{
    private TileData[] map;
    private int width;
    private int height;
    private List<Bomb> bombs = new();
    private List<Enemy> enemies= new();

    public int Width { get { return width; } }
    public int Height { get { return height; } }
    public int Size { get { return map.Length; }}

    public Stage(int w, int h, TileData[] tiles)
    {
        map = tiles;
        width = w;
        height = h;
    }

    public Stage(int w, int h) : this(w, h, new TileData[w * h])
    {
    }

    public bool MoveUnit(Unit unit, GridCoords localDir)
    {
        GridCoords coords = ApplyTransform(unit.Position, localDir);
        int destination = TryGetIndexUnchecked(coords);

        if (map[destination].IsOccupied)
        {
            return false;
        }
        else
        {
            TileData tile = map[destination];
            tile.IsOccupied = true;
            map[destination] = tile;
            unit.Position = TryGetCoordsUnchecked(destination);
            return true;
        }
    }

    public bool ForceMoveUnit(Unit unit, int index)
    {
        GridCoords destination;
        if (!TryGetCoords(index, out destination))
            {
                return false;
            }
        return ForceMove(unit, destination, index);
    }

    public bool ForceMoveUnit(Unit unit, GridCoords destination)
    {
        int index;
        if (!TryGetIndex(destination, out index))
            {
                return false;
            }
        return ForceMove(unit, destination, index);        
    }

    private bool ForceMove(Unit unit, GridCoords destination, int index)
    {
        if (map[index].IsOccupied)
        {
            return false;
        }
        else
        {
            if (TryGetIndex(unit.Position, out int pos))
            {
                map[pos].IsOccupied = false;
            }
            
            TileData tile = map[index];
            tile.IsOccupied = true;
            map[index] = tile;
            unit.Position = destination;
            return true;
        }
    }

#region GridHelpers 
    public TileData GetTile(int index)
    {
        return map[index];
    }

    public TileData GetTile(GridCoords coords)
    {
        return map[coords.ToIndex(width)];
    }

    public void SetTile(int index, TileData tile)
    {
        map[index] = tile;
    }

    public bool IsOccupied(int index)
    {
        return map[index].IsOccupied;
    }

    public bool IsOccupied(GridCoords coords)
    {
        return map[coords.ToIndex(width)].IsOccupied;
    }

    public void SetOccupied(int index, bool occupied)
    {
        TileData tile = map[index];
        tile.IsOccupied = occupied;
        map[index] = tile;
    }

    public bool IsInBounds(GridCoords position)
    {
        return position.X >= 0 && position.X < width && position.Y >= 0 && position.Y < height;
    }

    public bool IsInBounds(int position)
    {
        return position >= 0 && position < map.Length;
    }

    public bool TryGetIndex(GridCoords position, out int index)
    {
        if (!IsInBounds(position))
        {
            index = -1;
            return false;
        }

        index = TryGetIndexUnchecked(position);
        return true;
    }

    public int TryGetIndexUnchecked(GridCoords position)
    {
        return position.X + position.Y * width;
    }

    public bool TryGetCoords(int position, out GridCoords coords)
    {
        if (!IsInBounds(position))
        {
            coords = new GridCoords(-1, -1);
            return false;
        }

        coords = TryGetCoordsUnchecked(position);
        return true;
    }

    public GridCoords TryGetCoordsUnchecked(int position)
    {
        return new GridCoords(position % width, position / width);
    }

    public GridCoords ApplyTransform(GridCoords origin, GridCoords transform)
    {
        int x = Clamp(origin.X + transform.X, 0, width - 1);
        int y = Clamp(origin.Y + transform.Y, 0, height - 1);

        return new GridCoords(x, y);
    }
#endregion GridHelpers
}