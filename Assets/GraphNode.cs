using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphNode : MonoBehaviour
{
    [SerializeField]
    public List<GraphNode> Adjacent;

    [SerializeField]
    private string _id;

    [Range(0, 10)]
    public int ExampleInteger;

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
                Debug.DrawLine(node.transform.position, transform.position, Color.red);
            }
    }
}
