using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bfs : PathFinder
{

    public Queue<Tuple<int, int, int>> fila = new Queue<Tuple<int, int, int>>(); //na tupla tem a posição a ser analizada e o ultimo índice analisado para poder continuar a iteração
    public Dictionary<Vector3, Vector3> nodeParents = new Dictionary<Vector3, Vector3>();
    int accumulatedCost;
    public override void init(Tuple<int, int> inicial) //inicial
    {
        controle = gameObject.GetComponent<GameControler>();
        int i = inicial.Item1;
        int j = inicial.Item2;
        visited = new bool[controle.getNRows(), controle.getNCols()];
        visited[i, j] = true;

        fila.Clear();
        fila.Enqueue(new Tuple<int, int, int>(0, i, j));

        terminei = false;

    }
    public Tuple<int, int, int> FindShortestPathBFS(Tuple<int, int, int> startPosition, Tuple<int, int, int> goalPosition)
    {

        float timeNow = Time.realtimeSinceStartup;

        Queue<Tuple<int, int, int>> queue = new Queue<Tuple<int, int, int>>();
        HashSet<Tuple<int, int, int>> exploredNodes = new HashSet<Tuple<int, int, int>>();
        queue.Enqueue(startPosition);

        while (queue.Count != 0)
        {
            Tuple<int, int, int> currentNode = queue.Dequeue();

            if (currentNode == goalPosition)
            {
                return currentNode;
            }

            List<Tuple<int, int, int>> nodes = getAdjCells(currentNode.Item2, currentNode.Item3);

            foreach (Tuple<int, int, int> node in nodes)
            {
                if (!exploredNodes.Contains(node))
                {
                    //Mark the node as explored
                    exploredNodes.Add(node);

                    //Store a reference to the previous node
                    nodeParents.Add(new Vector3(node.Item2, node.Item3), new Vector3(currentNode.Item2, currentNode.Item3) );

                    //Add this to the queue of nodes to examine
                    queue.Enqueue(node);
                }
            }
        }

        return startPosition;
    }
    public override Tuple<int, int> iteration(Tuple<int, int> destino)
    {

        Tuple<int, int, int> aux = new Tuple<int, int, int>(-1, -1, -1);
        Tuple<int, int> next = new Tuple<int, int>(-1, -1);
        Tuple<int, int, int> node = new Tuple<int, int, int>(-1, -1, -1);
        List<Tuple<int, int, int>> adj = new List<Tuple<int, int, int>>();
        if (fila.Count != 0)
        {
            aux = fila.Dequeue();
            adj = getAdjCells(aux.Item2, aux.Item3);

            next = new Tuple<int, int>(aux.Item2, aux.Item3);
            if (next.Item1 == destino.Item1 && next.Item2 == destino.Item2)
            {
                terminei = true;
            }

            for (int i=0; i<adj.Count; i++)
            {
                node = adj[i];
                if(!visited[node.Item2, node.Item3])
                {
                    Vector3 nodeInVect = new Vector3(node.Item2, node.Item3);
                    if (nodeParents.ContainsKey(nodeInVect))
                    {
                        nodeParents[nodeInVect] = new Vector3(aux.Item2, aux.Item3);
                    }
                    else
                    {
                        nodeParents.Add(nodeInVect, new Vector3(aux.Item2, aux.Item3));
                    }

                    visited[node.Item2, node.Item3] = true;
                    fila.Enqueue(new Tuple<int, int, int>(0, node.Item2, node.Item3));
                }
            }
            return next;

            /*
            if (adj.Count > aux.Item1)
            {
                int val = 0;
                bool deu = false;
                while (val + aux.Item1 < adj.Count)
                {
                    node = adj[aux.Item1 + val];
                    if (!visited[node.Item2, node.Item3])
                    {
                        deu = true;
                        break;
                    }
                    val++;
                }
                //debug:
                if (aux.Item2 == -1) Debug.Log("Nao atualizou");

                //fila.Enqueue(new Tuple<int, int, int>(aux.Item1 + val + 1, aux.Item2, aux.Item3));
                
                if (deu)
                {
                    Vector3 nodeInVect = new Vector3(node.Item2, node.Item3);
                    if (nodeParents.ContainsKey(nodeInVect))
                    {
                        nodeParents[nodeInVect] = new Vector3(aux.Item2, aux.Item3);
                    }
                    else { 
                        nodeParents.Add(nodeInVect, new Vector3(aux.Item2, aux.Item3));
                    }
                    
                    visited[node.Item2, node.Item3] = true;
                    
                    fila.Enqueue(new Tuple<int, int, int>(0, node.Item2, node.Item3));

                    
                    //debug:
                    if (node.Item2 == -1) Debug.Log("Nao atualizou o 2");

                    return next;
                }

            }*/
        }

        terminei = true;

        return next;

    }
}
