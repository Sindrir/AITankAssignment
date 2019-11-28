using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : MonoBehaviour
{
    [SerializeField] GraphNode start;
    [SerializeField] GraphNode end;
    private Graph  graphScript;
    // Start is called before the first frame update
    void Start()
    {
        graphScript = GameObject.Find("Graph").GetComponent<Graph>();
     // A* ---> index out of range   graphScript.AStar(start, end);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
