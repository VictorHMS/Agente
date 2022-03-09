using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bfs : PathFinder
{

    public Queue<Tuple<int, int, int>> fila = new Queue<Tuple<int, int, int>>(); //na tupla tem a posição a ser analizada e o ultimo índice analisado para poder continuar a iteração
    public Dictionary<Vector3, Vector3> nodeParents = new Dictionary<Vector3, Vector3>();

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
        }

        terminei = true;

        return next;

    }

    public override List<Vector3> getPath(Tuple<int, int> target)
    {
        List<Vector3> path = new List<Vector3>();

        var current = new Vector3(target.Item1, target.Item2);
        while (nodeParents.Count > 0)
        {
            path.Add(current);
            if (nodeParents.ContainsKey(current))
            {
                var aux = current;
                current = nodeParents[current];
                nodeParents.Remove(aux);
            }
            else
            {
                break;
            }

        }
        path.Reverse();

        return path;
    }
}
