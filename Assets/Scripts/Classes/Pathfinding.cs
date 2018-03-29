using System.Collections.Generic;
using UnityEngine;
using System;

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

        public bool isWalkable { get; set; }
        public float gCost { get; private set; }
        public float hCost { get; private set; }
        public float fCost { get { return gCost + hCost; } }
    }

    public class Pathfinding
    {
        static float steppenalty = 0.01f; //Adds a small difficulty penalty to every step, making the pathfinding prefer shorter path when there are 2 paths of otherwise same difficulty 

        public static List<T> AStar<T>(List<List<T>> Graph, T source, T target) where T : ITile
        {
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
            //UnityEngine.Debug.Log("Checked all options in " + sw.ElapsedMilliseconds + " ms.");
            return new List<T>();
        }

        private static T MinValue<T>(List<T> list, Dictionary<T, float> fscore)
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

        private static List<T> reconstruct_path<T>(Dictionary<T, T> cameFrom, T current) where T : ITile
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
        
    }
}

