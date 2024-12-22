using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficCongestion : MonoBehaviour
{
    int count = 0;
    [SerializeField] int maxCount = 0;
    [SerializeField] bool isFull = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            count++;
            
            if (count >= maxCount)
                isFull = true;
            else
                isFull = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            count--;

            if (count >= maxCount)
                isFull = true;
            else
                isFull = false;
        }
    }

    public bool GetStatus()
    {
        return isFull;
    }
}
