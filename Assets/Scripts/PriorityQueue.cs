using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PriorityQueue<T>
{

	List<Tuple<T, double>> elements = new List<Tuple<T, double>>();
	private int tam { get; set; } = 0;

	
	public int Count
	{
		get { return elements.Count; }
	}

	public int size()
    {
		return tam;
    }

	public bool empty()
    {
		return tam == 0;
    }

	public void Clear()
    {
		elements.Clear();
		tam = 0;
    }

	
	public void Enqueue(T item, double priorityValue)
	{
		tam++;
		int ind = tam - 1;
		elements.Add(Tuple.Create(item, priorityValue));
		heapfyLT(ind);

	}

	private void heapfyLT(int ind)
    {
		int pai = (ind - 1) / 2;
		if (pai < 0) return;
		if(elements[pai].Item2 > elements[ind].Item2)
        {
			Tuple<T, double> aux = new Tuple<T, double>(elements[pai].Item1, elements[pai].Item2);
			elements[pai] = elements[ind];
			elements[ind] = aux;
			heapfyLT(pai);
        }
    }


	public T Dequeue()
	{

		Tuple<T, double> topo = new Tuple<T, double>(elements[0].Item1, elements[0].Item2);
		elements[0] = new Tuple<T, double>(elements[tam - 1].Item1, elements[tam - 1].Item2);

		heapfyTL(0);

		
		elements.RemoveAt(tam-1);
		tam--;
		return topo.Item1;
	}

	private void heapfyTL(int ind)
    {
		int l = (ind * 2) + 1;
		int r = (ind * 2) + 2;
		int menor = ind;

		if(l < tam && elements[l].Item2 < elements[ind].Item2)
        {
			menor = l;
        }
        else
        {
			menor = ind;
        }
		if(r < tam && elements[r].Item2 < elements[menor].Item2)
        {
			menor = r;
        }
		if(menor != ind)
        {
			Tuple<T, double> aux = new Tuple<T, double>(elements[menor].Item1, elements[menor].Item2);
			elements[menor] = elements[ind];
			elements[ind] = aux;
			heapfyTL(menor);
		}

	}

	
	public T Peek()
	{
		T bestItem = elements[0].Item1;
		return bestItem;
	}
}
