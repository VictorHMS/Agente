using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Guloso : PathFinder
{
    //public IPriorityQueue<Tuple <int, int, int> , int  > fila = new PriorityQueue<Tuple<int, int, int> , int >();
    public PriorityQueue<Tuple<int, int, int>> fila = new PriorityQueue<Tuple<int, int, int>>();
    public Dictionary<Vector3, Vector3> nodeParents = new Dictionary<Vector3, Vector3>();

    
    int accumulatedCost = 0;

    public override void init(Tuple<int, int> inicial) //inicial
    {
        controle = gameObject.GetComponent<GameControler>();
        int i = inicial.Item1;
        int j = inicial.Item2;
        visited = new bool[controle.getNRows(), controle.getNCols()];
        visited[i, j] = true;

        
        for (int linha = 0; linha < controle.getNRows(); linha++)
        {
            for (int coluna = 0; coluna < controle.getNCols(); coluna++)
            {
                
                visited[linha, coluna] = false;
            }
        }
        


        fila.Clear();
        fila.Enqueue(new Tuple<int, int, int>(controle.getPeso(i, j), i, j), 0);

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
                return next;
            }

            for (int i = 0; i < adj.Count; i++)
            {
                node = adj[i];
                if (!visited[node.Item2, node.Item3])
                {
                    visited[node.Item2, node.Item3] = true;
                    /*Debug.Log("Entrei");
                    Debug.Log("Val1: ");
                    Debug.Log(aux.Item1 + node.Item1);
                    Debug.Log("Val2: ");
                    Debug.Log(dist[node.Item2, node.Item3]);
                    */
                    Vector3 nodeInVect = new Vector3(node.Item2, node.Item3);
                    if (nodeParents.ContainsKey(nodeInVect))
                    {
                        nodeParents[nodeInVect] = new Vector3(aux.Item2, aux.Item3);
                    }
                    else
                    {
                        nodeParents.Add(nodeInVect, new Vector3(aux.Item2, aux.Item3));
                    }
                    
                    fila.Enqueue(new Tuple<int, int, int>(controle.getPeso(node.Item2, node.Item3), node.Item2, node.Item3), controle.getPeso(node.Item2, node.Item3));
                    //Debug
                    //Debug.Log("Adicionei: " + aux.Item2.ToString() + " " + aux.Item3.ToString() + " eh pai de " + node.Item2.ToString() + " " + node.Item3.ToString());

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