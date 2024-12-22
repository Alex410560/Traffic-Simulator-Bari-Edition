using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Keybind : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI upButton;
    [SerializeField] TextMeshProUGUI downButton;
    [SerializeField] TextMeshProUGUI speedButton;

    bool upBinding = false;
    bool downBinding = false;
    bool speedBinding = false;

    [SerializeField] Slider sensitivitySlider;
    [SerializeField] TextMeshProUGUI sensitivityDisplay;

    [SerializeField] Slider horizontalSpeedSlider;
    [SerializeField] TextMeshProUGUI horizontalSpeedDisplay;

    void Start()
    {
        upButton.text = PlayerPrefs.GetString("Up", "LeftShift");
        downButton.text = PlayerPrefs.GetString("Down", "LeftControl");
        speedButton.text = PlayerPrefs.GetString("SimulationSpeed", "L");

        sensitivitySlider.onValueChanged.AddListener(ChangeMouseSensitivity);
        sensitivitySlider.value = PlayerPrefs.GetFloat("MouseSensitivity", 0.5f);
        sensitivityDisplay.text = PlayerPrefs.GetFloat("MouseSensitivity", 0.5f).ToString();

        horizontalSpeedSlider.onValueChanged.AddListener(ChangeHorizontalSpeed);
        horizontalSpeedSlider.value = PlayerPrefs.GetFloat("HorizontalSpeed", 50f) / 100;
        horizontalSpeedDisplay.text = PlayerPrefs.GetFloat("HorizontalSpeed", 50f).ToString();
    }

    void Update()
    {
        if (upBinding)
        {
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(keyCode))
                {
                    upButton.text = keyCode.ToString();
                    upBinding = false;
                    PlayerPrefs.SetString("Up", keyCode.ToString());
                    PlayerPrefs.Save();
                }
            }
        }

        if (downBinding)
        {
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(keyCode))
                {
                    downButton.text = keyCode.ToString();
                    downBinding = false;
                    PlayerPrefs.SetString("Down", keyCode.ToString());
                    PlayerPrefs.Save();
                }
            }
        }

        if (speedBinding)
        {
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(keyCode))
                {
                    speedButton.text = keyCode.ToString();
                    speedBinding = false;
                    PlayerPrefs.SetString("SimulationSpeed", keyCode.ToString());
                    PlayerPrefs.Save();
                }
            }
        }
    }

    public void ChangeUpKey()
    {
        upButton.text = "Premi un tasto";

        upBinding = true;
    }

    public void ChangeDownKey()
    {
        downButton.text = "Premi un tasto";

        downBinding = true;
    }

    public void ChangeSpeedKey()
    {
        speedButton.text = "Premi un tasto";

        speedBinding = true;
    }

    void ChangeMouseSensitivity(float value)
    {
        float x = Mathf.Round(value * 100f) / 100f;

        if (x == 0f)
        {
            x = 0.01f;
        }

        sensitivityDisplay.text = x.ToString();

        PlayerPrefs.SetFloat("MouseSensitivity", x);
        PlayerPrefs.Save();
    }

    void ChangeHorizontalSpeed(float value)
    {
        float x = Mathf.Round(value * 100f);

        if (x == 0f)
        {
            x = 1f;
        }

        horizontalSpeedDisplay.text = x.ToString();

        PlayerPrefs.SetFloat("HorizontalSpeed", x);
        PlayerPrefs.Save();
    }
}
