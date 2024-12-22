using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeOfDay : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI timeDisplay;

    void Start()
    {
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    void Awake()                                                                            
    {
        slider.onValueChanged.AddListener(OnSliderValueChanged);

        //Inizializza lo slider e il display ai parametri salvati dall'utente se disponibili, altrimenti lo inizializza a quelli di default ovvero 0
        slider.value = PlayerPrefs.GetFloat("Time", 0);

        float hour = Mathf.FloorToInt(PlayerPrefs.GetFloat("Time", 0) * 24);
        float minute = Mathf.FloorToInt(((PlayerPrefs.GetFloat("Time", 0) * 24) - hour) * 60);

        string hourString = "" + hour;
        string minuteString = "" + minute;

        if (hour < 10)
            hourString = "0" + hourString;

        if (minute < 10)
            minuteString = "0" + minuteString;

        timeDisplay.text = hourString + ":" + minuteString;
    }

    void OnSliderValueChanged(float value)
    {
        float hour = Mathf.FloorToInt(value * 24);
        float minute = Mathf.FloorToInt(((value * 24) - hour) * 60);

        string hourString = "" + hour;
        string minuteString = "" + minute;

        if (hour < 10)
            hourString = "0" + hourString;

        if (minute < 10)
            minuteString = "0" + minuteString;

        timeDisplay.text = hourString + ":" + minuteString;

        PlayerPrefs.SetFloat("Time", value);
        PlayerPrefs.Save(); // Salva i dati sul disco (opzionale)
    }
}
