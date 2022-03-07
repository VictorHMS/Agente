using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruta : MonoBehaviour
{
    // Start is called before the first frame update
    public GameControler controle;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        if(controle != null)
        {
            transform.SetPositionAndRotation(new Vector3(controle.posFruta.x, controle.posFruta.y, -1), new Quaternion());
        }
        
    }
}
