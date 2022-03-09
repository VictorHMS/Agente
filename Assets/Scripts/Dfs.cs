using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Dfs : PathFinder
{
    public Stack<Tuple<int, int, int>> pilha = new Stack<Tuple<int, int, int>>(); //na tupla tem a posição a ser analizada e o ultimo índice analisado para poder continuar a iteração
    
    public override void init(Tuple<int, int> inicial) //inicial
    {
        controle = gameObject.GetComponent<GameControler>();
        int i = inicial.Item1;
        int j = inicial.Item2;

        visited = new bool[controle.getNRows(), controle.getNCols()];

        pilha.Clear();
        pilha.Push(new Tuple<int, int, int>(0, i, j));
        
        terminei = false;
        
    }

    public override Tuple<int, int> iteration(Tuple<int, int> destino)
    {

        Tuple<int, int, int> aux = new Tuple<int, int, int>(-1, -1, -1);
        Tuple<int, int> next = new Tuple<int, int>(-1, -1);
        Tuple<int, int, int> node = new Tuple<int, int, int>(-1, -1, -1);
        List<Tuple<int, int, int>> adj = new List<Tuple<int, int, int>>();
        while (pilha.Count != 0)
        {
            aux = pilha.Pop();
            adj = getAdjCells(aux.Item2, aux.Item3);
            if (adj.Count > aux.Item1)
            {
                int val = 0;
                bool deu = false;
                while (val+aux.Item1 < adj.Count)
                {
                    node = adj[aux.Item1 + val];
                    if (!visited[node.Item2, node.Item3])
                    {
                        deu = true;
                        break;
                    }
                    val++;
                }

                pilha.Push(new Tuple<int, int, int>(aux.Item1 + val + 1, aux.Item2, aux.Item3));

                if (deu)
                {
                    visited[node.Item2, node.Item3] = true;
                    next = new Tuple<int, int>(node.Item2, node.Item3);
                    pilha.Push(new Tuple<int, int, int>(0, node.Item2, node.Item3));

                    if (next.Item1 == destino.Item1 && next.Item2 == destino.Item2) {
                        terminei = true;
                    }

                    return next;
                }

            }
        }

        terminei = true;

        return next;

    }

    public override List<Vector3> getPath(Tuple<int, int> target) {
        List<Vector3> path = new List<Vector3>();
        while (pilha.Count > 0)
        {
            var elem = pilha.Pop();
            path.Add(new Vector3(elem.Item2, elem.Item3));
        }
        path.Reverse();

        return path;
    }
}
