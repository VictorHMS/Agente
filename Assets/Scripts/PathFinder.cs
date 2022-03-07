using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    public GameControler controle;
    List<Tuple<int,int,int>> getAdjCells(int lin, int col)
    {
        List<Tuple<int, int, int>> adj = new List<Tuple<int, int, int>>();
        if (controle.exist(lin + 1, col)) adj.Add(new Tuple<int, int, int>(controle.getPeso(lin + 1, col), lin + 1, col));
        if (controle.exist(lin - 1, col)) adj.Add(new Tuple<int, int, int>(controle.getPeso(lin - 1, col), lin - 1, col));
        if (controle.exist(lin, col + 1)) adj.Add(new Tuple<int, int, int>(controle.getPeso(lin, col + 1), lin, col + 1));
        if (controle.exist(lin, col - 1)) adj.Add(new Tuple<int, int, int>(controle.getPeso(lin, col - 1), lin, col - 1));
        return adj;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
