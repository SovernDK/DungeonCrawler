using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;

namespace Crawler 
{
    public class Pathfinding : MonoBehaviour
    {
        private Grid grid;

        public void Initialize(Grid grid)
        {
            this.grid = grid;
        }

        public Dictionary<Tile, Tile> BreadthFirst(Vector3Int start, Vector3Int goal)
        {
            Queue<Tile> frontier = new Queue<Tile>();
            frontier.Enqueue(grid.Map[start]);
            Dictionary<Tile, Tile> cameFrom = new Dictionary<Tile, Tile>();
            cameFrom[grid.Map[start]] = grid.Map[start];

            while(frontier.Count > 0)
            {
                Tile current = frontier.Dequeue();

                if(current == grid.Map[goal]) break;

                foreach(Tile next in grid.GetPassableNeighbours(current))
                {
                    if(!cameFrom.ContainsKey(next) && !next.IsOccupied)
                    {
                        frontier.Enqueue(next);
                        cameFrom[next] = current;
                    }
                }
            }   
            
            return cameFrom;
        }

        public Dictionary<Tile, Tile> BreadthFirstEarlyDistance(Vector3Int start, int limit)
        {
            Queue<Tile> frontier = new Queue<Tile>();
            frontier.Enqueue(grid.Map[start]);
            Dictionary<Tile, Tile> cameFrom = new Dictionary<Tile, Tile>();
            cameFrom[grid.Map[start]] = grid.Map[start];

            while(frontier.Count > 0)
            {
                Tile current = frontier.Dequeue();
                if(Vector3Int.Distance(start, current.Coordinates) > limit) break;

                foreach(Tile next in grid.GetPassableNeighbours(current))
                {
                    if(!cameFrom.ContainsKey(next) && !next.IsOccupied)
                    {
                        frontier.Enqueue(next);
                        cameFrom[next] = current;
                    }
                }
            }   
            
            return cameFrom;
        }

        public bool ActorIsDistanceAway(Vector3Int start, Vector3Int actorCoordinates, int distance)
        {
            if(Vector3Int.Distance(start, actorCoordinates) <= distance)
            {
                return true;
            }

            return false;
        }

        public Queue<Tile> CreatePath(Vector3Int start, Vector3Int goal)
        {
            Dictionary<Tile, Tile> cameFrom = BreadthFirst(start, goal);

            Tile node = grid.Map[goal];
            Queue<Tile> path = new Queue<Tile>();

            while(node != grid.Map[start])
            {
                path.Enqueue(node);
                if (!cameFrom.ContainsKey(node))
                {
                    break; // If the node is not in the dictionary, break the loop as we can go no further
                }
                node = cameFrom[node];
            }
            path.Enqueue(grid.Map[start]);
            
            return new Queue<Tile>(path.ToArray().Reverse().ToArray());
        }
    }
}