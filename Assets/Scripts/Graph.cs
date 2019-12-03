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
    public List<GraphNode> AStar(GraphNode startNode, GraphNode endNode)
    {

        List<GraphNode> openSet = new List<GraphNode>();
        GraphNode[] cameFrom = new GraphNode[100];
        List<GraphNode> closedSet = new List<GraphNode>();
        openSet.Add(startNode);
        List<GraphNode> testCameFrom = new List<GraphNode>();
        for (int i = 0; i < 200; i++) testCameFrom.Add(null);
        //Debug.Log(testCameFrom.Count);
        float[] gScore = new float[100];
        float[] fScore = new float[100];
        for (int i = 0; i < 100; i++)
        {
            gScore[i] = 1000000;
            fScore[i] = 1000000;
        }
        gScore[startNode.id] = 0;
        fScore[startNode.id] = h(startNode, endNode);
        //cameFrom.Add(startNode);

        var counter = 0;
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
            //Debug.Log("Current: " + current.id);
            if (current == endNode)
            {
                // TODO Reconstruct the path
               // Debug.Log("winning current " + current.id);
               var path = ReconstructPath(cameFrom, current);
                //Debug.Log(path.Count);
               // Debug.Log("Path:");
                //foreach (var l in path)
                 //   Debug.Log(l.id);
               // Debug.Log("The A* path");
                return path;
            }
            
            openSet.Remove(current);
            //cameFrom.Add(current);
            if(!closedSet.Contains(current))
                closedSet.Add(current);
            foreach (var node in current.Adjacent)
            {
                if (closedSet.Contains(node))
                {
                    continue;
                }

                var edge = GetEdge(current, node);
                var tentativeScore = gScore[current.id] + edge.Weight;
                if(tentativeScore < gScore[node.id])
                {
                    //if (!cameFrom.Contains(current))
                    //    cameFrom.Add(current);
                    //else cameFrom[counter] = current;

                    //cameFrom[counter] = current;
                    cameFrom[node.id] = current;
                    gScore[node.id] = tentativeScore;
                    fScore[node.id] = gScore[node.id] + h(node, endNode);
                    node.parent = current;

                    if (!openSet.Find(x => x.id == node.id))
                    {
                        openSet.Add(node);
                    }
                }

            }

            //debugger(closedSet, "closedSet ");
            counter++;
        }
        return null;
    }

    private List<GraphNode> ReconstructPath(GraphNode[] cameFrom, GraphNode current)
    {
        List<GraphNode> totalPath = new List<GraphNode>();
        totalPath.Add(current);

        while (current)
        {
            current = cameFrom[current.id];
            totalPath.Insert(0, current);
        }


        //foreach (var node in cameFrom)
        //{
        //    totalPath.Add(node);
        //totalPath.Insert(0, node);
        //}
        //totalPath.Add(current);
        /*
        GraphNode currentNode = end;

        while (currentNode != start)
        {
            totalPath.Add(currentNode);
            currentNode = currentNode.parent;
        }
        totalPath.Reverse();
        */
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

    private void debugger(List<GraphNode> l, string txt)
    {
        foreach (var n in l) Debug.Log(txt + n.id);
    }

    private void OnDrawGizmos()
    {
        var path = AStar(Nodes[3], Nodes[18]);
        var count = 0;
        for (var i = 0; i < path.Count; i++)
        {
            if (count != 0 && path[i - 1] != null)
            {
                Debug.DrawLine(path[i].transform.position, path[i - 1].transform.position, Color.blue);
            }
            count++;
        }
    }
}
