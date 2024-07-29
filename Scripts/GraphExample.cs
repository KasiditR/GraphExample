using System.Collections.Generic;
using UnityEngine;

public enum ChamberType
{
    Normal,
    Contest,
    Horde,
    Prophet,
    Blacksmith,
    Random,
    Shop,
    Boss,
}

public class GraphExample : MonoBehaviour
{
    private System.Random random = new System.Random();
    private Graph<string> graph;

    private void Start()
    {
        GenerateMap();
    }

    private void GenerateMap()
    {
        graph = new Graph<string>();
        float zOffset = 2f;

        List<Node<string>> previousFloorNodes = new List<Node<string>>();

        for (int floor = 1; floor <= 21; floor++)
        {
            int numChambers;
            if (floor == 1)
            {
                numChambers = 1;
            }
            else
            {
                numChambers = (floor == 11 || floor == 21) ? 1 : random.Next(2, 5);
            }

            List<Node<string>> currentFloorNodes = new List<Node<string>>();
            for (int index = 1; index <= numChambers; index++)
            {
                ChamberType chamberType = GenerateChamber(floor, index);
                float xOffset = (index - 1 - (numChambers - 1) / 2.0f) * 2.0f;
                Vector3 position = new Vector3(xOffset, 0, floor * zOffset);
                Node<string> node = graph.AddNode($"Floor {floor}", position, GetChamberColor(chamberType));
                // Node<string> node = graph.AddNode($"Floor {floor}, Chamber {index}: {chamberType}", position, GetChamberColor(chamberType));
                currentFloorNodes.Add(node);
            }

            if (previousFloorNodes.Count > 0)
            {
                if (previousFloorNodes.Count == 1)
                {
                    foreach (Node<string> currentNode in currentFloorNodes)
                    {
                        graph.AddEdge(previousFloorNodes[0], currentNode);
                    }
                }
                else
                {
                    int currentFloorIndex = 0;

                    foreach (var previousNode in previousFloorNodes)
                    {
                        if (currentFloorIndex < currentFloorNodes.Count)
                        {
                            graph.AddEdge(previousNode, currentFloorNodes[currentFloorIndex]);
                        }
                        else
                        {
                            graph.AddEdge(previousNode, currentFloorNodes[currentFloorNodes.Count - 1]);
                        }

                        currentFloorIndex++;
                    }

                    while (currentFloorIndex < currentFloorNodes.Count)
                    {
                        graph.AddEdge(previousFloorNodes[previousFloorNodes.Count - 1], currentFloorNodes[currentFloorIndex]);

                        currentFloorIndex++;
                    }

                }
            }

            previousFloorNodes = currentFloorNodes;
        }
    }
    private Color GetChamberColor(ChamberType chamberType)
    {
        return chamberType switch
        {
            ChamberType.Normal => Color.red,
            ChamberType.Contest => Color.gray,
            ChamberType.Horde => Color.blue,
            ChamberType.Prophet => Color.green,
            ChamberType.Blacksmith => Color.magenta,
            ChamberType.Random => new Color(0.5f, 0f, 0.5f, 1f),
            ChamberType.Shop => Color.yellow,
            ChamberType.Boss => Color.black,
            _ => Color.white,
        };
    }


    private ChamberType GenerateChamber(int floor, int index)
    {
        if (floor == 1 && index == 1)
        {
            return ChamberType.Normal;
        }
        else if (floor == 11 && index == 1)
        {
            return ChamberType.Shop;
        }
        else if (floor == 21 && index == 1)
        {
            return ChamberType.Boss;
        }
        else
        {
            Dictionary<ChamberType, double> chamberRates = new Dictionary<ChamberType, double>
            {
                { ChamberType.Normal, 0.70 },
                { ChamberType.Contest, 0.05 },
                {ChamberType.Horde, 0.03 },
                { ChamberType.Prophet, 0.10 },
                { ChamberType.Blacksmith, 0.05 },
                { ChamberType.Random, 0.02 },
                { ChamberType.Shop, 0.05 }
            };

            double totalWeight = 0;
            foreach (var weight in chamberRates.Values)
            {
                totalWeight += weight;
            }

            double rand = random.NextDouble() * totalWeight;
            double cumulativeWeight = 0;
            foreach (var chamber in chamberRates)
            {
                cumulativeWeight += chamber.Value;
                if (rand < cumulativeWeight)
                {
                    return chamber.Key;
                }
            }

            return ChamberType.Normal;
        }
    }

    private void OnDrawGizmos()
    {
        if (graph == null)
        {
            return;
        }

        foreach (Node<string> node in graph.Nodes)
        {
            Gizmos.color = node.NodeColor;
            Gizmos.DrawSphere(node.Position, 0.2f);
            UnityEditor.Handles.Label(new Vector3(-10, 0, node.Position.z), $"{node.Value}");
            foreach (Node<string> neighbor in node.Neighbors)
            {
                Gizmos.color = new Color(1, 0, 0, 0.25f);
                Gizmos.DrawLine(node.Position, neighbor.Position);
            }
        }
    }
}
