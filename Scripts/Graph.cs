using System.Collections.Generic;
using UnityEngine;

public class Graph<T>
{
    public List<Node<T>> Nodes;

    public Graph()
    {
        Nodes = new List<Node<T>>();
    }

    public Node<T> AddNode(T value, Vector3 position, Color nodeColor)
    {
        var node = new Node<T>(value, position, nodeColor);
        Nodes.Add(node);
        return node;
    }

    public void AddEdge(Node<T> from, Node<T> to)
    {
        from.Neighbors.Add(to);
        to.Neighbors.Add(from);
    }
}
