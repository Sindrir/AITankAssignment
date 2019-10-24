using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Graph : MonoBehaviour
{
    public List<GraphNode> Nodes;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Reset()
    {
        Nodes = GetComponentsInChildren<GraphNode>(true).ToList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
