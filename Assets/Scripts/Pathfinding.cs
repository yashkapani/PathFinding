using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace AISandbox {
    public class Pathfinding : MonoBehaviour {
        public Grid grid;

        public Toggle showPathToggle;
        public Image triangleImage;

        private PriorityQueue<float, GridNode> OpenList;
        private IList<GridNode> CloseList;
        public Stack<GridNode> PathNodes;
        public Stack<GridNode> ResetNodes;

        GridNode destination;
        GridNode start;
        public GridNode nodeToseek;

        public bool pathFollowing;
        public bool showPath;
        public bool pathFound;

        private IActor _actor;

        private float GetHeuristic(GridNode current, GridNode goal)
        {
            return Vector3.Distance(current.transform.position, goal.transform.position); 
        }

        public void Initialize() {
            // Create and center the grid
            OpenList = new PriorityQueue<float, GridNode>();
            CloseList = new List<GridNode>();
            PathNodes = new Stack<GridNode>();
            ResetNodes = new Stack<GridNode>();
            _actor = GetComponent<IActor>();
        }

        private void AStar()
        {
            GridNode current = OpenList.DequeueValue();
            CloseList.Add(current);
            IList<GridNode> neighbours = current.GetNeighbors();
            foreach(GridNode neighbour in neighbours)
            {
                if (!neighbour.blocked)
                {
                    float cost = current.cost + neighbour.cost;
                    if (OpenList.Contains(neighbour) && cost < neighbour.cost)
                    {
                        OpenList.Remove(new KeyValuePair<float, GridNode>(neighbour.cost, neighbour));
                    }
                    else if (CloseList.Contains(neighbour) && cost < neighbour.cost)
                        CloseList.Remove(neighbour);
                    else if (!OpenList.Contains(neighbour) && !CloseList.Contains(neighbour))
                    {
                        neighbour.cost = cost;
                        float rank = neighbour.cost + GetHeuristic(neighbour, destination);
                        OpenList.Add(new KeyValuePair<float, GridNode>(rank, neighbour));
                        neighbour.parent = current;
                    }
                }
            }
        }
        public bool plotPath()
        {
            if(showPathToggle.isOn)
            {
                foreach(GridNode node in ResetNodes)
                {
                    node.showPath = true;
                }
            }
            else
            {
                foreach (GridNode node in ResetNodes)
                {
                    if(!node.showPath)
                        node.showPath = false;
                }
            }

            float distancefromSeekNode = Vector3.Distance(transform.position, nodeToseek.transform.position);

            if (distancefromSeekNode > 1)
            {
                if (nodeToseek.forest)
                {
                    _actor.Velocity /= 2.0f;
                }
                else if(nodeToseek.swamp)
                {
                    _actor.Velocity /= 3.0f;
                }
                else if (nodeToseek.road)
                {
                    _actor.Velocity /= 4.0f;
                }
                else
                {
                    _actor.Velocity = _actor.Velocity.normalized * 5.0f;
                }
                _actor.Velocity = _actor.Velocity + (Vector2)(nodeToseek.transform.position - transform.position);
                return false;
            }
            else
            {
                if (PathNodes.Count != 0)
                {
                    nodeToseek = PathNodes.Pop();
                    return false;
                }
                else
                {
                    start = null;
                    return true;
                }
            }
        }

        public void doAstar()
        {
            if (OpenList.Count == 0)
                start = null;
            else
            {
                while (OpenList.PeekValue() != destination)
                {
                    AStar();
                }
                pathFound = true;
                GridNode iterator = destination;
                while (iterator.parent != null)
                {
                    PathNodes.Push(iterator);
                    ResetNodes.Push(iterator);
                    GridNode oldNode = iterator;
                    iterator = iterator.parent;
                    oldNode.parent = null;
                }
            }
        }

        public void setDestination(GridNode i_destination)
        {
            destination = i_destination;
            Reset();
        }

        void Reset()
        {
            if (destination != null)
            {
                foreach (GridNode node in ResetNodes)
                {
                    node.showPath = false;
                }
                GridNode current = grid.GetNodeBasedOnPosition(transform.position);
                PathNodes.Clear();
                ResetNodes.Clear();
                OpenList.Clear();
                CloseList.Clear();
                start = current;
                float startPriority = GetHeuristic(start, destination);
                OpenList.Add(new KeyValuePair<float, GridNode>(startPriority, start));
                pathFollowing = false;
                pathFound = false;
            }
        }
    }
}