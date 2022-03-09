using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    public GameControler controle;
    protected bool[,] visited;
    public bool terminei;
    public List<Tuple<int,int,int>> getAdjCells(int lin, int col)
    {
        controle = gameObject.GetComponent<GameControler>();
        List<Tuple<int, int, int>> adj = new List<Tuple<int, int, int>>();
        if (controle.exist(lin + 1, col)) adj.Add(new Tuple<int, int, int>(controle.getPeso(lin + 1, col), lin + 1, col));
        if (controle.exist(lin - 1, col)) adj.Add(new Tuple<int, int, int>(controle.getPeso(lin - 1, col), lin - 1, col));
        if (controle.exist(lin, col + 1)) adj.Add(new Tuple<int, int, int>(controle.getPeso(lin, col + 1), lin, col + 1));
        if (controle.exist(lin, col - 1)) adj.Add(new Tuple<int, int, int>(controle.getPeso(lin, col - 1), lin, col - 1));
        return adj;
    }

    public virtual void init(Tuple<int, int> inicial)
    {

    }

    public virtual List<Vector3> getPath(Tuple<int, int> target) {
        return new List<Vector3>();
    }

    public virtual Tuple<int, int> iteration(Tuple<int,int> destino)
    {
        return new Tuple<int, int>(0, 0);
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
