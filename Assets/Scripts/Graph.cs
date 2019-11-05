using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Graph : MonoBehaviour
{
    [System.Serializable]
    public class Edge
    {
        public GraphNode StartNode;

        public GraphNode EndNode;

        [Range(0f, 10f)]
        public int Weight;
    }

    public List<GraphNode> Nodes;
    public List<Edge> Edges; 

    private void Reset()
    {
        Nodes = GetComponentsInChildren<GraphNode>(true).ToList();
    }

    /*
    public GraphNode GetNextNode()
    {
        
    }
    */
    public GraphNode AStar(GraphNode startNode, GraphNode endNode)
    {

        List<GraphNode> openSet = new List<GraphNode>();
        List<GraphNode> cameFrom = new List<GraphNode>();
        openSet.Add(startNode);

        float[] gScore = new float[100];
        float[] fScore = new float[100];
        for (int i = 0; i < 100; i++)
        {
            gScore[i] = 1000;
            fScore[i] = 1000;
        }
        gScore[startNode.id] = 0;
        fScore[startNode.id] = h(startNode, endNode);

        while (openSet.Any())
        {
            var lowIndex = 0;
            for (var i = 0; i < openSet.Count; i++)
            {
                if (fScore[i] < fScore[lowIndex])
                {
                    lowIndex = i;
                }
            }
            var current = openSet[lowIndex];
            if (current == endNode)
            {
                // TODO Reconstruct the path
            }

            openSet.Remove(current);
            foreach (var node in current.Adjacent)
            {
                var edge = GetEdge(current, node);
                var tentativeScore = gScore[current.id] + edge.Weight;
                if (tentativeScore < gScore[node.id])
                {
                    cameFrom[node.id] = current;
                    gScore[node.id] = tentativeScore;
                    fScore[node.id] = gScore[node.id] + h(current, node);
                    if (!openSet.Find(x => x.id == node.id))
                    {
                        openSet.Add(node);
                    }
                }
            }
        }
        return null;
    }

    private List<GraphNode> ReconstructPath(List<GraphNode> cameFrom, GraphNode current)
    {
        List<GraphNode> totalPath = new List<GraphNode>();
        totalPath.Add(current);
        foreach (var node in cameFrom)
        {
            totalPath.Insert(0, node);
        }
        return totalPath;
    }

    private Edge GetEdge(GraphNode start, GraphNode end)
    {
        foreach (var _egde in Edges)
        {
            if (_egde.StartNode == start && _egde.EndNode == end)
            {
                return _egde;
            }
        }
        return null;
    }

    private float h(GraphNode start, GraphNode end)
    {
        var targetDistance = end.gameObject.transform.position - start.transform.position;
        var distance = Mathf.Sqrt(Mathf.Pow(targetDistance.x, 2) + Mathf.Pow(targetDistance.z, 2));
        return distance;
    }
}
