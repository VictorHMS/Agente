using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class DropDown : MonoBehaviour
{

    enum buscasType {
        Largura, Profundidade, CustoUniforme, Gulosa, AStar
    };

    public Text TextBox;
    // Start is called before the first frame update
    void Start()
    {
        var dropd = transform.GetComponent<Dropdown>();
        dropd.options.Clear();

        foreach (var busca in Enum.GetNames(typeof(buscasType)))
        {
            dropd.options.Add(new Dropdown.OptionData() { text = busca });
        }

        DropDownItemSelected(dropd);
        
        TextBox.text = dropd.options[0].text;
        dropd.onValueChanged.AddListener(delegate { DropDownItemSelected(dropd); });
        dropd.value = 1;
        dropd.value = 4;
    }

    void DropDownItemSelected(Dropdown dropd)
    {
        int index = dropd.value;
        TextBox.text = dropd.options[index].text;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
