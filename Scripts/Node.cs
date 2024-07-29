using System.Collections.Generic;
using UnityEngine;

public class Node<T>
{
    public T Value;
    public Vector3 Position;
    public Color NodeColor;
    public List<Node<T>> Neighbors;

    public Node(T value, Vector3 position, Color nodeColor)
    {
        Value = value;
        Position = position;
        NodeColor = nodeColor;
        Neighbors = new List<Node<T>>();
    }
}
