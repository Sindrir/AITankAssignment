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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Reset()
    {
        Nodes = GetComponentsInChildren<GraphNode>(true).ToList();
    }
    
    public GraphNode GetNextNode()
    {
        
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
