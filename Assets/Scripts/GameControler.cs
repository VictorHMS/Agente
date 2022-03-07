using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControler : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[,] tabuleiro { get; set; }
    public int[,] campo;
    public int[] weight = {1, 5, 3, INF};
    public GameObject field;
    public GameObject tile;
    public Text textinho;
    private const float WIDTH = 1f, HEIGHT = 1f;
    public const int NROWS = 40;
    public const int NCOLS = 60;
    public const int INF = 1000000000;
    public Vector3 posFruta = new Vector3();
    public Vector3 posAgente = new Vector3();
    enum buscasType
    {
        Largura, Profundidade, Custo_Uniforme, Gulosa, AStar
    };
     //cons = Enum.Parse(typeof(buscasType), textinho.toString);

    void randAgente()
    {
        int indI = (int)Random.Range(0.0f, (float)NROWS);
        int indJ = (int)Random.Range(0.0f, (float)NCOLS);

        posAgente = (tabuleiro[indI, indJ].transform.position);
        Debug.Log(indI);
        Debug.Log(indJ);
    }

    void randfruta()
    {
        int indI = (int) Random.Range(0.0f, (float)NROWS);
        int indJ = (int)Random.Range(0.0f, (float)NCOLS);
        posFruta = tabuleiro[indI, indJ].transform.position;
    }

    void generateMap()
    {
        float magnification = Random.Range(6.0f, 11.0f);
        float xOffset = Random.Range(0.0f, 100.0f);
        float yOffset = Random.Range(0.0f, 100.0f);
        for (int i = 0; i < NROWS; i++)
        {
            for (int j = 0; j < NCOLS; j++)
            {
                Vector3 position = new Vector3((j * HEIGHT), (i * WIDTH), 0);
                Color agua = Color.blue;
                Color grama = Color.green;
                Color lama = Color.grey; //marrom
                Color muro = Color.magenta;
                Color[] cores = { grama, agua, lama, muro };
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
    }

    public int getPeso(int i, int j)
    {
        return weight[campo[i, j]];
    }

    public bool exist(int i, int j)
    {
        if (i > NROWS || i < 0) return false;
        if (j > NCOLS || j < 0) return false;
        if (campo[i, j] == 3) return false;
        return true;
    }

    void Start()
    {
        field = GameObject.Find("Field");
        tabuleiro = new GameObject[NROWS, NCOLS];
        campo = new int[NROWS, NCOLS];
        for (int i=0; i<NROWS; i++)
        {
            for(int j=0; j<NCOLS; j++)
            {
                Vector3 position = new Vector3((j * HEIGHT), (i * WIDTH), 0);
                tabuleiro[i,j] = Instantiate(tile,position, new Quaternion());
                /*Color agua = Color.blue;
                Color grama = Color.green;
                Color lama = Color.grey; //marrom
                Color muro = Color.magenta;
                Color[] cores = { grama, agua, lama, muro };
                float xCoord = position.x / 7f;
                float yCoord = position.y/ 7f;
                float val = Mathf.PerlinNoise(xCoord, yCoord);
                Debug.Log(val);
                //Debug.Log(position.x);
                //Debug.Log(position.y);
                val = Mathf.Max(Mathf.Min(val, 0.99f), 0.1f);
                int index = (int) (val*4);
                var aux = tabuleiro[i, j].GetComponent<Renderer>();
                aux.material.SetColor("_Color", cores[index]);*/                
            }
        }
        generateMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
