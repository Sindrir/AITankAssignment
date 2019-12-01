using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : MonoBehaviour
{
    public GraphNode current;
    public GraphNode enemyNode;

    public List<GraphNode> path;
    private Graph  graphScript;
    private Kill killScript;
    // Start is called before the first frame update
    void Start()
    {
        path = new List<GraphNode>();
        graphScript = GameObject.Find("Graph").GetComponent<Graph>();  
        
        killScript = GetComponent<Kill>();
        enemyNode = killScript.lastSeen;
        current = killScript.currentNode;
    }

    // Update is called once per frame
    void Update()
    {
        enemyNode = killScript.lastSeen;
        current = killScript.currentNode;
        if(enemyNode && current && killScript.canSeeTarget)
            path = graphScript.AStar(current, enemyNode);
       
    }
}
