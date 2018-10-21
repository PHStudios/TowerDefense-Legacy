using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TowerDefenseEngine
{
    public class Pathfinding
    {
        public List<Tile> path;

        public void AStar(Tile start, Tile end)
        {
            List<Tile> open = new List<Tile>();
            open.Add(start);

            List<Tile> closed = new List<Tile>();

            Tile l = null;
            while (l != end && open.Count != 0 && end.Parent == null)
            {
                l = FindBestNode(open);
                if (l.Equals(end))
                    continue;
                else
                {
                    open.Remove(l);
                    closed.Add(l);
                    l.Region = DisplayRegion.Closed;

                    foreach (Tile adj in l.AdjacencyList)
                    {
                        if (adj.Type == TileType.NotPassable || closed.Contains(adj))
                            continue;
                        if (!open.Contains(adj))
                        {
                            AddToOpenList(open, adj, l, end);
                            adj.Parent = l;
                        }
                        else
                        {
                            int newDistance = l.DistanceFromStart + DistanceSquared(l, adj);
                            if (newDistance < adj.DistanceFromStart)
                            {
                                adj.Parent = l;
                                adj.DistanceFromStart = newDistance;
                            }
                        }
                    }
                }
            }

            if (end.Parent != null)
                GeneratePath(start, end);
            else
                throw new Exception("Check to make sure that your path is a valid path (monsters can travel along the path from start to finish)");

            start.Region = DisplayRegion.Start;
        }

        private void GeneratePath(Tile start, Tile end)
        {
            path = new List<Tile>();
            Tile p = end.Parent;
            while (p != start)
            {
                path.Insert(0, p);
                p.Region = DisplayRegion.InPath;
                p = p.Parent;
            }
        }

        private void AddToOpenList(List<Tile> open, Tile adj, Tile l, Tile end)
        {
            adj.DistanceFromStart = l.DistanceFromStart + DistanceSquared(l, adj);
            adj.DistanceToEnd = DistanceSquared(adj, end);
            open.Add(adj);
        }

        private Tile FindBestNode(List<Tile> open)
        {
            Tile lowest = open[0];
            Tile n = null;

            for (int i = 1; i < open.Count; i++)
            {
                n = open[i];
                if (n.Heuristic < lowest.Heuristic)
                    lowest = n;
                else if (n.Heuristic == lowest.Heuristic)
                {
                    if (lowest.DistanceToEnd > n.DistanceToEnd)
                        lowest = n;
                    else if (lowest.DistanceToEnd == n.DistanceToEnd && lowest.DistanceFromStart > n.DistanceFromStart)
                        lowest = n;
                }
            }

            return lowest;
        }

        private int DistanceSquared(Tile a, Tile b)
        {
            int deltaX = b.MapLocation.X - a.MapLocation.X;
            int deltay = b.MapLocation.Y - a.MapLocation.Y;

            return (deltaX * deltaX) + (deltay * deltay);
        }
    }
}
