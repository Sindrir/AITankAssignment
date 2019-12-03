using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphNode : MonoBehaviour
{
    [SerializeField]
    public List<GraphNode> Adjacent;

    [SerializeField]
    public int id;

    [Range(0, 10)]
    public int ExampleInteger;

    public int gCost;
    public int hCost;
    public GraphNode parent;
    

    public bool HasAdjacent()
    {
        foreach (var node in Adjacent)
        {
            return true;
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        if (HasAdjacent())
            foreach (var node in Adjacent)
            {
                var position = node.transform.position;
                var position2 = transform.position;
                position.y = position.y - 1;
                position2.y = position2.y - 1;
                Debug.DrawLine(position, position2, Color.red);
            }
    }

}
