using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeScaler : MonoBehaviour
{
    public float currentTimeScale = 1.0f;
    public TextMeshProUGUI simulationSpeed;

    void Update()
    {
        KeyCode key = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("SimulationSpeed", "L"));

        if (Input.GetKeyDown(key))
        {
            if (currentTimeScale >= 16)
            {
                currentTimeScale = 1;
            }
            else
            {
                currentTimeScale = currentTimeScale * 2;
            }

            simulationSpeed.text = "Velocità di simulazione: " + currentTimeScale + "x";
            Time.timeScale = currentTimeScale;                                          // Aumenta la velocità della simulazione
        }
    }

    void OnEnable()
    {
        Time.timeScale = currentTimeScale;
    }
}
