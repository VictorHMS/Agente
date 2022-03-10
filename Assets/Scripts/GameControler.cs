using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class GameControler : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[,] tabuleiro { get; set; }
    public int[,] campo;
    public int[] weight = { 1, 5, 3, INF };
    public GameObject field;
    public GameObject canvas;
    public GameObject tile;
    public Text textinho;
    private const float WIDTH = 1f, HEIGHT = 1f;
    public const int NROWS = 40;
    public const int NCOLS = 60;
    public const int INF = 1000000000;
    public float tbusca = 0.01f;
    public float speed = 0.5f;
    public Vector3 posFruta = new Vector3();
    public Vector3 posAgente = new Vector3();
    public Tuple<int, int> indAgente, indFruta;
    private Color agua = Color.blue;
    private Color grama = Color.green;
    private Color lama = Color.grey; //marrom
    private Color muro = Color.magenta;
    private Color[] cores;
    private int energia;
    private int score;
    private int[] custos;
    private List<Vector3> path;

    public Text pontinhos;
    public Text gastos;

    private int state = 0;
    
    Dfs dfs;
    Bfs bfs;
    CustoUniforme custoUniforme;
    Astrela astrela;
    PathFinder busca;
    Tuple<int, int> node;
    buscasType buscaType = buscasType.Largura;
    float tempo = 0;
    enum buscasType
    {
        Largura, Profundidade, CustoUniforme, Gulosa, AStar
    };

    public int getNRows()
    {
        return 40;
    }

    public int getNCols()
    {
        return 60;
    }

    void randAgente()
    {
        int indI;
        int indJ;
        do
        {
            indI = (int)UnityEngine.Random.Range(0.0f, (float)NROWS);
            indJ = (int)UnityEngine.Random.Range(0.0f, (float)NCOLS);
        } while (getPeso(indI, indJ) == INF);

        posAgente = (tabuleiro[indI, indJ].transform.position);
        indAgente = new Tuple<int, int>(indI,indJ);
        //Debug.Log(indI);
        //Debug.Log(indJ);
    }

    void randfruta()
    {
        int indI;
        int indJ;
        do
        {
            indI = (int)UnityEngine.Random.Range(0.0f, (float)NROWS);
            indJ = (int)UnityEngine.Random.Range(0.0f, (float)NCOLS);
        } while (getPeso(indI, indJ) == INF);

        posFruta = tabuleiro[indI, indJ].transform.position;
        indFruta = new Tuple<int, int>(indI, indJ);
    }

    void generateMap()
    {
        float magnification = UnityEngine.Random.Range(6.0f, 11.0f);
        float xOffset = UnityEngine.Random.Range(0.0f, 100.0f);
        float yOffset = UnityEngine.Random.Range(0.0f, 100.0f);
        for (int i = 0; i < NROWS; i++)
        {
            for (int j = 0; j < NCOLS; j++)
            {
                Vector3 position = new Vector3((j * HEIGHT), (i * WIDTH), 0);
                
                float xCoord = (position.x + xOffset) / magnification;
                float yCoord = (position.y + yOffset) / magnification;
                float val = Mathf.PerlinNoise(xCoord, yCoord);
                //Debug.Log(val);
                //Debug.Log(position.x);
                //Debug.Log(position.y);
                val = Mathf.Max(Mathf.Min(val, 0.99f), 0.1f);
                int index = (int)(val * 4);
                campo[i, j] = index;
                var aux = tabuleiro[i, j].GetComponent<Renderer>();
                aux.material.SetColor("_Color", cores[index]);
            }
        }
        randAgente();
        randfruta();
        node = new Tuple<int, int>(indAgente.Item1, indAgente.Item2);
        tempo = 0;
    }

    public int getPeso(int i, int j)
    {
        return weight[campo[i, j]];
    }

    public bool exist(int i, int j)
    {
        if (i >= NROWS || i < 0) return false;
        if (j >= NCOLS || j < 0) return false;
        if (campo[i, j] == 3) return false;
        return true;
    }

    public void GotoPoint(Vector3 target) {
        var diff = (target - posAgente).normalized;
        posAgente = posAgente + diff*speed;
    }

    public void updateFruta() {
        var distance = (posAgente - posFruta).magnitude;
        if (distance < 0.7) {
            randfruta();
        }
    }

    void Start()
    {
        dfs = gameObject.AddComponent<Dfs>();
        bfs = gameObject.AddComponent<Bfs>();
        custoUniforme = gameObject.AddComponent<CustoUniforme>();
        astrela = gameObject.AddComponent<Astrela>();
        busca = gameObject.AddComponent<PathFinder>();

        

        field = GameObject.Find("Field");
        canvas = GameObject.Find("Canvas");


        pontinhos.text = "Frutas: 0";
        gastos.text = "Custo: 0";
        score = 0;

        tabuleiro = new GameObject[NROWS, NCOLS];
        campo = new int[NROWS, NCOLS];
        cores = new Color[] { grama, agua, lama, muro };
        custos = new int[] {1, 10, 5, 1000000000};
        for (int i=0; i<NROWS; i++)
        {
            for(int j=0; j<NCOLS; j++)
            {
                Vector3 position = new Vector3((j * HEIGHT), (i * WIDTH), 0);
                tabuleiro[i,j] = Instantiate(tile,position, new Quaternion());            
            }
        }
        generateMap();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == 0)
        {
            //generate World
            pontinhos.text = "Frutas: " + score.ToString();
            generateMap();
            state = 2;
        }
        else if (state == 2)
        {
            //init pathFinder
            buscaType = (buscasType)Enum.Parse(typeof(buscasType), textinho.text);
            energia = 0;
            gastos.text = "Custos: " + energia.ToString();
            switch (buscaType)
            {
                case buscasType.Largura:
                    {
                        busca = bfs;
                        busca.init(indAgente);
                        //bfs.init(indAgente);
                        break;
                    }
                case buscasType.Profundidade:
                    {
                        busca = dfs;
                        busca.init(indAgente);
                        //dfs.init(indAgente);
                        break;
                    }
                case buscasType.CustoUniforme:
                    {
                        busca = custoUniforme;
                        busca.init(indAgente);
                        break;
                    }
                case buscasType.Gulosa:
                    {
                        //TODO: algotitmo
                        break;
                    }
                case buscasType.AStar:
                    {
                        busca = astrela;
                        busca.init(indAgente);
                        break;
                    }
            }
     
            state = 3;
        }
        else if (state == 3)
        {
            //find path
            float nt = Time.deltaTime;
            tempo += nt;
            if (tempo >= tbusca)
            {
                if (exist(node.Item1, node.Item2)) esmaecer(node);
                if (busca.terminei)
                {
                    state = 4;
                    path = busca.getPath(indFruta);
                }
                else
                {
                    node = busca.iteration(indFruta);
                    if (exist(node.Item1, node.Item2)) pintar(node);
                }
                tempo = 0;
            }
            /*
            switch (buscaType)
            {
                case buscasType.Largura:
                    {

                        float nt = Time.deltaTime;
                        tempo += nt;
                        if (tempo >= tbusca)
                        {
                            if (exist(node.Item1, node.Item2)) esmaecer(node);
                            if (busca.terminei)
                            {
                                state = 4;
                                path = busca.getPath(indFruta);
                            }
                            else
                            {
                                node = busca.iteration(indFruta);
                                if (exist(node.Item1, node.Item2)) pintar(node);
                            }
                            tempo = 0;
                        }
                        break;

                    }
                case buscasType.Profundidade:
                    {
                        float nt = Time.deltaTime;
                        tempo += nt;
                        if (tempo >= tbusca)
                        {
                            if (exist(node.Item1, node.Item2)) esmaecer(node);
                            if (busca.terminei)
                            {
                                state = 4;
                               path = busca.getPath(indFruta);    
                            }
                            else
                            {
                                node = busca.iteration(indFruta);
                                if (exist(node.Item1, node.Item2)) pintar(node);
                            }
                            tempo = 0;
                        }
                        break;
                    }
                case buscasType.CustoUniforme:
                    {
                        //TODO: algotitmo
                        break;
                    }
                case buscasType.Gulosa:
                    {
                        //TODO: algotitmo
                        break;
                    }
                case buscasType.AStar:
                    {
                        //TODO: algotitmo
                        break;
                    }
            }*/
        }
        else if (state == 4) {
            // draw path
            foreach (var elem in path) {
                energia += custos[campo[(int)elem.x,(int)elem.y]];
                var aux = tabuleiro[(int)elem.x, (int)elem.y].GetComponent<Renderer>();
                aux.material.SetColor("_Color", Color.black);
            }
            gastos.text = "Custos: " + energia.ToString();
            state = 5;
        }
        else if (state == 5)
        {
            //execute path
            if (path.Count > 0)
            {
                Vector3 targePos = tabuleiro[(int)path[0].x, (int)path[0].y].transform.position;
                var distance = (posAgente - targePos).magnitude;
                if (distance < 0.4)
                {
                    path.RemoveAt(0);
                }

                GotoPoint(targePos);

                if ((posAgente - posFruta).magnitude < 0.4)
                {
                    score++;
                    state = 0;
                }
            }
            else {
                GotoPoint(posFruta);
            }
        }
    }

    void pintar(Tuple<int,int> posi)
    {
        var aux = tabuleiro[posi.Item1,posi.Item2].GetComponent<Renderer>();
        aux.material.SetColor("_Color", Color.black);
    }

    void esmaecer(Tuple<int, int> posi)
    {
        var aux = tabuleiro[posi.Item1, posi.Item2].GetComponent<Renderer>();
        aux.material.SetColor("_Color", cores[campo[posi.Item1,posi.Item2]]);
        float r = aux.material.GetColor("_Color").r;
        float g = aux.material.GetColor("_Color").g;
        float b = aux.material.GetColor("_Color").b;
        aux.material.SetColor("_Color", new Color(r,g,b,0.8f));
    }

}
