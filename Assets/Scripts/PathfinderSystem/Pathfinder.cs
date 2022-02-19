using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Pathfinder
{
    private readonly NodeMap _map;
    private readonly PathRequestManager _pathRequestManager;

    public static void Create(NodeMap map) => new Pathfinder(map);
    
    private Pathfinder(NodeMap map)
    {
        _map = map;
        _pathRequestManager = new PathRequestManager(this);
    }
    
    public async void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        var waypoints = new List<Vector3>();
        var pathSuccess = false;

        var startNode = _map.NodeFromWorldPoint(startPos);
        var targetNode = _map.NodeFromWorldPoint(targetPos);

        if (startNode.Walkable && targetNode.Walkable)
        {
            var openSet = new Heap<Node>(_map.MaxSize);
            var closedSet = new HashSet<Node>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                var currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    pathSuccess = true;
                    break;
                }

                foreach (Node neighbour in _map.GetFourNeighbours(currentNode))
                {
                    if (!neighbour.Walkable || closedSet.Contains(neighbour)) continue;

                    var newMovementCostToNeighbour = currentNode.GCost + GetDistance(currentNode, neighbour);
                    if (newMovementCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour))
                    {
                        neighbour.GCost = newMovementCostToNeighbour;
                        neighbour.HCost = GetDistance(neighbour, targetNode);
                        neighbour.Parent = currentNode;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                    }
                }
            }
        }

        await Task.Yield();
        
        if (pathSuccess) waypoints = RetracePath(startNode, targetNode);
        _pathRequestManager.FinishedProcessingPath(waypoints, pathSuccess);
    }

    private List<Vector3> RetracePath(Node startNode, Node endNode)
    {
        var path = new List<Node>();
        var currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }
        
        var waypoints = GetPath(path);
        waypoints.Reverse();
        return waypoints;
    }

    private List<Vector3> GetPath(List<Node> path)
    {
        var waypoints = new List<Vector3>();

        foreach (var node in path)
        {
            waypoints.Add(node.WorldPosition);
        }

        return waypoints;
    }
    
    private int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.X - nodeB.X);
        int dstY = Mathf.Abs(nodeA.Y - nodeB.Y);

        if (dstX > dstY)
        {
            return 14 * dstY + 10 * (dstX - dstY);
        }
        return 14 * dstX + 10 * (dstY - dstX);
    }
}