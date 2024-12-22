using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CarNumber : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI display;

    void Awake()
    {
        slider.onValueChanged.AddListener(OnSliderValueChanged);

        slider.value = PlayerPrefs.GetFloat("maxCarNumber", 100) / 500;
        display.text = PlayerPrefs.GetFloat("maxCarNumber", 100).ToString();
    }

    void OnSliderValueChanged(float value)
    {
        value = value * 500;

        int a = (int)value;

        display.text = a.ToString();

        PlayerPrefs.SetFloat("maxCarNumber", a);
        PlayerPrefs.Save();
    }
}
