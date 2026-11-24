using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Layers_Manager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> Layers;

    private int counter = 0;
    private int max_counter;

    void Start()
    {
        foreach(GameObject x in Layers)
        {
            counter++;
        }
        counter--;
        Debug.Log("El contador es de: " + counter);
        max_counter = counter;
    }

    public void Remove_Layer()
    {
        Layers[counter].SetActive(false);
        Debug.Log("Layer " + counter + " eliminated");
        if (counter > 0) counter--;
    }

    public void Add_Layer()
    {
        Layers[counter].SetActive(true);
        Debug.Log("Layer " + counter + " added");
        if(counter < max_counter) counter++;
    }

}
