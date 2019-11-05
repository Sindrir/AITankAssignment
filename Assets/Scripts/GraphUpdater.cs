using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
public class GraphUpdater : MonoBehaviour
{
    [SerializeField]
    private Graph _graph;

    [SerializeField]
    private int _timesliceInterval;

    [SerializeField]
    private int _currentFrame;

    // Start is called before the first frame update
    void Start()
    {
        if (_graph == null)
            _graph = GetComponent<Graph>();

        _currentFrame = 0;

        // timeslices should happen every other frame
        if (_timesliceInterval <= 0)
            _timesliceInterval = 10;
    }

    // Update is called once per frame
    void Update()
    {
        if (_graph == null)
            return;

        _currentFrame++;

        // Wait for the frame count to surpass the timeslice interval, so we do not execute the update every frame
        if (_currentFrame < _timesliceInterval)
            return;
        else
            _currentFrame = 0;

        // loop through all graph nodes
        foreach (var node in _graph.Nodes)
        {
            // look through the adjacent nodes
            foreach(var adjacentNode in node.Adjacent)
            {
                if (node == adjacentNode)
                    continue;

                if (_graph.Edges == null)
                    continue;

                // find an existing directional edge that is cached in the graph
                var existingEdge = _graph.Edges.Find(edge => edge.EndNode == adjacentNode
                   && edge.StartNode == node);

                // create the directional edge if its not cached
                if(existingEdge == null)
                {
                    var edge = new Graph.Edge();
                    edge.StartNode = node;
                    edge.EndNode = adjacentNode;
                    edge.Weight = 1;

                    _graph.Edges.Add(edge);
                }
            }
        }
    }
}
