using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TowerDefenseEngine
{
    public enum DisplayRegion
    {
        Passable,
        Start,
        Finish,
        NotPassable,
        InPath,
        Closed
    }

    public enum TileType
    {
        Passable,
        NotPassable
    }
    public class Tile
    {
        public TileType Type
        {
            get;
            set;
        }

        public DisplayRegion Region
        {
            get;
            set;
        }

        public Point MapLocation
        {
            get;
            private set;
        }

        public List<Tile> AdjacencyList
        {
            get;
            private set;
        }

        public int DistanceFromStart
        {
            get;
            set;
        }

        public int DistanceToEnd
        {
            get;
            set;
        }

        public Tile Parent
        {
            get;
            set;
        }

        public int TileCode
        {
            get;
            private set;
        }

        public int Heuristic
        {
            get { return DistanceFromStart + DistanceToEnd; }
        }

        public Tile(Point loc, int code)
        {
            MapLocation = loc;
            Type = TileType.Passable;
            DistanceFromStart = 0;
            TileCode = code;
        }

        public Tile(TileType type, Point loc, int code)
        {
            MapLocation = loc;
            Type = type;
            DistanceFromStart = 0;
            TileCode = code;
        }

        public void AddToAdjacencyList(Tile t)
        {
            if (t == null) return;
            if (AdjacencyList == null)
                AdjacencyList = new List<Tile>();

            AdjacencyList.Add(t);
        }

        public override string ToString()
        {
            return MapLocation.ToString() + " a:" + AdjacencyList.Count + " c:" + TileCode.ToString();
        }


    }
}
