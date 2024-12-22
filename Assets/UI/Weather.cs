using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Weather : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    
    void Awake()
    {
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);

        dropdown.value = PlayerPrefs.GetInt("Weather", 0);
    }

    void OnDropdownValueChanged(int value)
    {
        PlayerPrefs.SetInt("Weather", value);
    }
}
