using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CarCounter : MonoBehaviour
{
    [SerializeField] int counter = 0;
    [SerializeField] int maxCounter = 100;
    [SerializeField] TextMeshProUGUI textBox;
    [SerializeField] GameObject carGenerators;

    void Start()
    {
        maxCounter = (int) PlayerPrefs.GetFloat("maxCarNumber", 100);
    }

    public void Increase()
    {
        counter++;
        textBox.text = "Numero di auto: " + counter;

        CheckCounter();
    }

    public void Decrease()
    {
        counter--;
        textBox.text = "Numero di auto: " + counter;

        CheckCounter();
    }

    void CheckCounter()
    {
        if (counter >= maxCounter)
            carGenerators.SetActive(false);
        else
            carGenerators.SetActive(true);
    }
}
