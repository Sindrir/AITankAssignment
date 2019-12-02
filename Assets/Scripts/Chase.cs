using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : MonoBehaviour
{
    public GraphNode current;
    public GraphNode enemyNode;

    public List<GraphNode> path;
    public List<GraphNode> oldPath;
    private Graph  graphScript;
    private Kill killScript;
    public bool pathChanged;
    // Start is called before the first frame update
    void Start()
    {
        path = new List<GraphNode>();
        graphScript = GameObject.Find("Graph").GetComponent<Graph>();  
        
        killScript = GetComponent<Kill>();
        enemyNode = killScript.lastSeen;
        current = killScript.currentNode;
        pathChanged = false;
    }

    // Update is called once per frame
    void Update()
    {
        oldPath = new List<GraphNode>();
        if(path != null)oldPath.AddRange(path);
        pathChanged = false;

        enemyNode = killScript.lastSeen;
        current = killScript.currentNode;
        if(enemyNode && current && killScript.canSeeTarget){
            path = graphScript.AStar(current, enemyNode);
            pathChanged = false;
            if(!listEqual(oldPath, path)) pathChanged = true;
            GetComponent<Complete.TankMovement>().enabled = false;
            GetComponent<TankChase>().enabled = true;
        }
    }

    private bool listEqual(List<GraphNode> l1, List<GraphNode> l2){
        if(l1 != null && l2 != null){
            if(l1.Count != l2.Count) return false;
            for(int i = 0; i < l1.Count; i++){
                if(!l1[i] == l2[i]) return false;
            }

            return true;
        }return false;
    }
}
