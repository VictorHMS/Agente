using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agente : MonoBehaviour
{
    public GameControler controle;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (controle != null)
        {
            transform.SetPositionAndRotation(new Vector3(controle.posAgente.x, controle.posAgente.y, -2), new Quaternion());
        }
        
    }
}
