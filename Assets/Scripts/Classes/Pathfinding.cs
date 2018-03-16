using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Diagnostics;

namespace Path
{
    [System.Serializable]
    abstract public class ITile : MonoBehaviour, IComparable<ITile>
    {
        public abstract float TileDiff();
        public abstract List<ITile> GetNeighbours();
        public abstract Vector2 getPosition();
        public abstract void ChangeColourTo(Color col); //- for debugging

        public int CompareTo(ITile obj)
        {
            int compare = fCost.CompareTo(obj.fCost);
            if (compare == 0)
            {
                compare = hCost.CompareTo(obj.hCost);
            }
            return -compare;
        }

        public virtual void SetGScore(float score)
        {
            gCost = Mathf.RoundToInt(score);
        }
        public virtual void SetHScore(float score)
        {
            hCost = Mathf.RoundToInt(score);
        }

        public bool isWalkable;
        public float gCost;
        public float hCost;
        public float fCost { get { return gCost + hCost; } }
        public int HeapIndex;

        public ITile parent;
    }

    public class Pathfinding
    {
        static float steppenalty = 0.01f; //Adds a small difficulty penalty to every step, making the pathfinding prefer shorter path when there are 2 paths of otherwise same difficulty 

        public static void Dijkstra<T>(List<List<T>> Graph, T source, T target, out Dictionary<T, float> dist1, out Dictionary<T, T> prev1) where T : ITile
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            List<T> unvisited = new List<T>(); //Initialization of a list that will hold all unvisited Ts (a TO DO list in essence)

            Dictionary<T, float> dist = new Dictionary<T, float>(); //Dictionary that will hold a difficulty value for every checked T
            Dictionary<T, T> prev = new Dictionary<T, T>(); //Dictionary that will hold a previous T for every T (where do you have to come from)

            //The following loop goes through every T in the map
            for (int i = 0; i < TileGenerator.MapWidth; i++)
            {
                for (int j = 0; j < TileGenerator.MapHeight; j++)
                {
                    T v = Graph[i][j]; //Temporarily store the T refference in v
                    dist[v] = float.MaxValue; //Set the starting distance to a maximum value - we will change it later when we actually know the difficulty of getting to our goal
                    prev[v] = null; //Set previous T to null - right now we haven't visited this T yet.
                    unvisited.Add(v); //Add to the list of unvisited Ts
                }
            }
            dist[source] = 0; //Set the distance to our current position to 0 - we are already here

            //The following loop repeats until we checked all Ts (unless we break from it early, because we have found our target)
            while (unvisited.Count > 0)
            {
                T u = unvisited.OrderBy(n => dist[n]).First(); //Get the first T in unvisited list according to distance - First time this will be our position, because it has distance value of 0 and everone else has a distance value of maximum float value
                unvisited.Remove(u); //Remove u from the list of unvisited Ts

                if (u == target) // If u is the target return our lists by setting it to our OUT attributes and end pathfinding
                {
                    dist1 = dist;
                    prev1 = prev;
                    sw.Stop();
                    UnityEngine.Debug.Log("Found path in " + sw.ElapsedMilliseconds + " ms.");
                    return;
                }
                //If we're not at our target check all neighbours of the current target
                foreach (T v in u.GetNeighbours())
                {
                    float alt = dist[u] + v.TileDiff() + steppenalty;//Alt is distance difficulty from our current position to our neighbouring T
                    if (alt < dist[v])//If that distance (alt) is shorter than the previous one we've found a better way to reach the T
                    {
                        dist[v] = alt; //Change the distance difficulty to the new value of alt
                        prev[v] = u; //Set the new direction of getting to this neighbour to our current position
                    }
                }
            }
            //After we've checked all the Ts exit the function and update the dictionaries in the OUT attributes
            dist1 = dist;
            prev1 = prev;
            return;
        }

        public static List<T> AStar<T>(List<List<T>> Graph, T source, T target) where T : ITile
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            // The set of nodes already evaluated
            HashSet<T> closedSet = new HashSet<T>(); // TODO: Change to HashSet

            // The set of currently discovered nodes that are not evaluated yet.
            // Initially, only the start node is known.
            List<T> openSet = new List<T>(); // TODO: Change to PriorityQueue/Heap
            openSet.Add(source);

            // For each node, which node it can most efficiently be reached from.
            // If a node can be reached from many nodes, cameFrom will eventually contain the
            // most efficient previous step.
            Dictionary<T, T> cameFrom = new Dictionary<T, T>();

            // For each node, the cost of getting from the start node to that node.
            Dictionary<T, float> gScore = new Dictionary<T, float>();
            Dictionary<T, float> fScore = new Dictionary<T, float>();

            for (int i = 0; i < Graph.Count; i++)
            {
                for (int j = 0; j < Graph[i].Count; j++)
                {
                    gScore[Graph[i][j]] = Mathf.Infinity;
                    fScore[Graph[i][j]] = Mathf.Infinity;
                }
            }

            // The cost of going from start to start is zero.
            gScore[source] = 0;
            // For the first node, that value is completely heuristic.
            fScore[source] = heuristic_cost_estimate(source, target);

            while (openSet.Count != 0)
            {
                T current = MinValue(openSet, fScore); // <-- PROBLEM IS HERE

                if (current == target)
                {
                    sw.Stop();
                    //UnityEngine.Debug.Log("Found path in " + sw.ElapsedMilliseconds + " ms.");
                    return reconstruct_path(cameFrom, current);
                }

                openSet.Remove(current);
                closedSet.Add(current);

                foreach (T neighbour in current.GetNeighbours())
                {
                    if (closedSet.Contains(neighbour))
                    {
                        continue;
                    }
                    if (!openSet.Contains(neighbour) && neighbour.isWalkable)
                    {
                        openSet.Add(neighbour);
                    }

                    float tentative_gScore = gScore[current] + neighbour.TileDiff();
                    if (tentative_gScore >= gScore[neighbour])
                    {
                        continue;
                    }

                    cameFrom[neighbour] = current;
                    gScore[neighbour] = tentative_gScore;
                    neighbour.SetGScore(gScore[neighbour]);
                    neighbour.SetHScore(heuristic_cost_estimate(neighbour, target));
                    fScore[neighbour] = gScore[neighbour] + heuristic_cost_estimate(neighbour, target);
                }
            }
            sw.Stop();
            //UnityEngine.Debug.Log("Checked all options in " + sw.ElapsedMilliseconds + " ms.");
            return new List<T>();
        }

        public static IEnumerator AStarVisual<T>(List<List<T>> Graph, T source, T target) where T : ITile
        {
            // The set of nodes already evaluated
            List<T> closedSet = new List<T>(); // TODO: Change to HashSet

            // The set of currently discovered nodes that are not evaluated yet.
            // Initially, only the start node is known.
            List<T> openSet = new List<T>(); // TODO: Change to PriorityQueue/Heap
            openSet.Add(source);


            // For each node, which node it can most efficiently be reached from.
            // If a node can be reached from many nodes, cameFrom will eventually contain the
            // most efficient previous step.
            Dictionary<T, T> cameFrom = new Dictionary<T, T>();

            // For each node, the cost of getting from the start node to that node.
            Dictionary<T, float> gScore = new Dictionary<T, float>();
            Dictionary<T, float> fScore = new Dictionary<T, float>();

            for (int i = 0; i < Graph.Count; i++)
            {
                for (int j = 0; j < Graph[i].Count; j++)
                {
                    gScore[Graph[i][j]] = Mathf.Infinity;
                    fScore[Graph[i][j]] = Mathf.Infinity;
                }
            }

            // The cost of going from start to start is zero.
            gScore[source] = 0;
            // For the first node, that value is completely heuristic.
            fScore[source] = heuristic_cost_estimate(source, target);

            while (openSet.Count != 0)
            {
                T current = MinValue(openSet, fScore); // <-- PROBLEM IS HERE

                /*
                float minF = Mathf.Infinity;
                for (int i = 0; i < openSet.Count; i++)
                {
                    if (fScore[openSet[i]] < minF)
                    {
                        minF = fScore[openSet[i]];
                        current = openSet[i];
                        Debug.Log("Found a smaller fScore");
                    }
                }
                */
                if (current == target)
                {
                    yield break;
                }

                openSet.Remove(current);
                closedSet.Add(current);

                foreach (T neighbour in current.GetNeighbours())
                {
                    if (closedSet.Contains(neighbour))
                    {
                        neighbour.ChangeColourTo(Color.Lerp(neighbour.GetComponent<SpriteRenderer>().color, Color.green, 0.5f));
                        continue;
                    }
                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                        neighbour.ChangeColourTo(Color.Lerp(neighbour.GetComponent<SpriteRenderer>().color, Color.red, 0.5f));
                        yield return null;
                    }

                    float tentative_gScore = gScore[current] + neighbour.TileDiff();
                    if (tentative_gScore >= gScore[neighbour])
                    {
                        continue;
                    }
                    
                    cameFrom[neighbour] = current;
                    gScore[neighbour] = tentative_gScore;
                    neighbour.SetGScore(gScore[neighbour]);
                    neighbour.SetHScore(heuristic_cost_estimate(neighbour, target));
                    fScore[neighbour] = gScore[neighbour] + heuristic_cost_estimate(neighbour, target);

                    if (neighbour is Tile)
                    {
                        
                    }
                    neighbour.ChangeColourTo(Color.Lerp(neighbour.GetComponent<SpriteRenderer>().color,Color.red,0.5f));
                }
                yield return null;
            }
            yield return null;
        }

        private static T MinValue<T>(List<T> list, Dictionary<T,float> fscore)
        {

            T min = list[0];
            float minVal = fscore[min];

            for (int i = 0; i < list.Count; i++)
            {

                if (fscore[list[i]] < minVal)
                {
                    minVal = fscore[list[i]];
                    min = list[i];

                }
            }
            return min;
        }

        private static List<T> reconstruct_path<T>(Dictionary<T,T> cameFrom, T current) where T : ITile
        {
            List<T> path = new List<T>();
            path.Add(current);
            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                path.Add(current);
            }
            return path;
        }

        private static float heuristic_cost_estimate<T>(T source, T target) where T : ITile
        {
            Vector2 posS = source.getPosition();
            Vector2 posT = target.getPosition();
            float heuristic = Mathf.Abs(posT.x - posS.x) + Mathf.Abs(posT.y - posS.y);
            //heuristic = Vector3.Distance(source.getPosition(), target.getPosition());

            //Debug.Log("Const estimate: " + heuristic);

            return heuristic;
        }

        public static List<T> FindPath<T>(List<List<T>> grid, T startTile, T endTile) where T : ITile
        {
            //Debug.Log("Starting Find path");

            Heap<T> openSet = new Heap<T>(grid[0].Count * grid.Count);
            HashSet<T> closedSet = new HashSet<T>();
            openSet.Add(startTile);

            while (openSet.Count > 0)
            {
                //Debug.Log("Items in Open Set: " + openSet.Count);
                T currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == endTile)
                {
                    //Debug.Log("Got to the end.");
                    return RetracePath(startTile, endTile);
                }

                foreach (T neighbour in currentNode.GetNeighbours())
                {
                    if (!neighbour.isWalkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    float newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, endTile);
                        neighbour.parent = currentNode;
                        //Debug.Log("Added a parent" + neighbour.parent);

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                        else
                        {
                            openSet.UpdateItem(neighbour);
                        }
                    }
                }
            }

            return RetracePath(startTile, endTile);
        }

        static float GetDistance<T>(T nodeA, T nodeB) where T: ITile
        {
            Vector2 posA = nodeA.getPosition();
            Vector2 posB = nodeB.getPosition();
            int dstX = Mathf.RoundToInt(Mathf.Abs(posA.x - posB.x));
            int dstY = Mathf.RoundToInt(Mathf.Abs(posA.y - posB.y));

            if (dstX > dstY)
                return 14 * dstY + 10 * (dstX - dstY);
            return 14 * dstX + 10 * (dstY - dstX);
        }

        private static List<T> RetracePath<T>(T startTile, T endTile) where T : ITile
        {
            List<T> path = new List<T>();
            T currentTile = endTile;

            while (currentTile != startTile)
            {
                path.Add(currentTile);
                currentTile = currentTile.parent as T;
            }
            path.Reverse();

            return path;
        }

        public class Heap<T> where T : ITile
        {

            T[] items;
            int currentItemCount;

            public Heap(int maxHeapSize)
            {
                items = new T[maxHeapSize];
            }

            public void Add(T item)
            {
                item.HeapIndex = currentItemCount;
                items[currentItemCount] = item;
                SortUp(item);
                currentItemCount++;
            }

            public T RemoveFirst()
            {
                T firstItem = items[0];
                currentItemCount--;
                items[0] = items[currentItemCount];
                items[0].HeapIndex = 0;
                SortDown(items[0]);
                return firstItem;
            }

            public void UpdateItem(T item)
            {
                SortUp(item);
            }

            public int Count
            {
                get
                {
                    return currentItemCount;
                }
            }

            public bool Contains(T item)
            {
                return Equals(items[item.HeapIndex], item);
            }

            void SortDown(T item)
            {
                while (true)
                {
                    int childIndexLeft = item.HeapIndex * 2 + 1;
                    int childIndexRight = item.HeapIndex * 2 + 2;
                    int swapIndex = 0;

                    if (childIndexLeft < currentItemCount)
                    {
                        swapIndex = childIndexLeft;

                        if (childIndexRight < currentItemCount)
                        {
                            if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                            {
                                swapIndex = childIndexRight;
                            }
                        }

                        if (item.CompareTo(items[swapIndex]) < 0)
                        {
                            Swap(item, items[swapIndex]);
                        }
                        else
                        {
                            return;
                        }

                    }
                    else
                    {
                        return;
                    }

                }
            }

            void SortUp(T item)
            {
                int parentIndex = (item.HeapIndex - 1) / 2;

                while (true)
                {
                    T parentItem = items[parentIndex];
                    if (item.CompareTo(parentItem) > 0)
                    {
                        Swap(item, parentItem);
                    }
                    else
                    {
                        break;
                    }

                    parentIndex = (item.HeapIndex - 1) / 2;
                }
            }

            void Swap(T itemA, T itemB)
            {
                items[itemA.HeapIndex] = itemB;
                items[itemB.HeapIndex] = itemA;
                int itemAIndex = itemA.HeapIndex;
                itemA.HeapIndex = itemB.HeapIndex;
                itemB.HeapIndex = itemAIndex;
            }
        }
    }
}

