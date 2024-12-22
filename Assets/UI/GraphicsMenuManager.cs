using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GraphicsMenuManager : MonoBehaviour
{
    [SerializeField] TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;

    [SerializeField] TMP_Dropdown antialiasingDropdown;

    [SerializeField] TextMeshProUGUI occlusionButton;

    [SerializeField] TextMeshProUGUI bloomButton;

    [SerializeField] TextMeshProUGUI shadowsButton;

    void Start()
    {
        InitializeResolutionDropdown();
        InitializeAntialiasingDropdown();
        InitializeOcclusionButton();
        InitializeBloomButton();
        InitializeShadowsButton();
    }

    void InitializeResolutionDropdown()
    {
        resolutionDropdown.onValueChanged.AddListener(ChangeResolution);

        resolutionDropdown.ClearOptions();

        int currentResolutionIndex = 0;
        resolutions = Screen.resolutions;
        var options = new List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            if (Screen.currentResolution.Equals(resolutions[i]))
            {
                currentResolutionIndex = i;
            }

            float refreshRate = resolutions[i].refreshRateRatio.numerator / (float)resolutions[i].refreshRateRatio.denominator;

            refreshRate = Mathf.Round(refreshRate * 100) / 100;

            string option = resolutions[i].width + "x" + resolutions[i].height + " " + refreshRate + "Hz";
            options.Add(option);
        }

        resolutionDropdown.AddOptions(options);

        resolutionDropdown.value = currentResolutionIndex;
    }

    void ChangeResolution(int value)
    {
        Screen.SetResolution(resolutions[value].width, resolutions[value].height, FullScreenMode.ExclusiveFullScreen, resolutions[value].refreshRateRatio);
        PlayerPrefs.Save();
    }

    void InitializeAntialiasingDropdown()
    {
        antialiasingDropdown.onValueChanged.AddListener(ChangeAntialiasing);

        antialiasingDropdown.value = PlayerPrefs.GetInt("Antialiasing", 3);
    }

    void ChangeAntialiasing(int value)
    {
        PlayerPrefs.SetInt("Antialiasing", value);
        PlayerPrefs.Save();
    }

    void InitializeOcclusionButton()
    {
        switch (PlayerPrefs.GetInt("AmbientOcclusion", 1))
        {
            case 0:
                occlusionButton.text = "No";
                break;
            case 1:
                occlusionButton.text = "Sì";
                break;
        }
    }

    public void ChangeOcclusion()
    {
        switch (PlayerPrefs.GetInt("AmbientOcclusion", 1))
        {
            case 0:
                occlusionButton.text = "Sì";
                PlayerPrefs.SetInt("AmbientOcclusion", 1);
                
                break;
            case 1:
                occlusionButton.text = "No";
                PlayerPrefs.SetInt("AmbientOcclusion", 0);
                break;
        }

        PlayerPrefs.Save();
    }

    void InitializeBloomButton()
    {
        switch (PlayerPrefs.GetInt("Bloom", 1))
        {
            case 0:
                bloomButton.text = "No";
                break;
            case 1:
                bloomButton.text = "Sì";
                break;
        }
    }

    public void ChangeBloom()
    {
        switch (PlayerPrefs.GetInt("Bloom", 1))
        {
            case 0:
                bloomButton.text = "Sì";
                PlayerPrefs.SetInt("Bloom", 1);

                break;
            case 1:
                bloomButton.text = "No";
                PlayerPrefs.SetInt("Bloom", 0);
                break;
        }

        PlayerPrefs.Save();
    }

    void InitializeShadowsButton()
    {
        switch (PlayerPrefs.GetInt("Shadows", 1))
        {
            case 0:
                shadowsButton.text = "No";
                break;
            case 1:
                shadowsButton.text = "Sì";
                break;
        }
    }

    public void ChangeShadows()
    {
        switch (PlayerPrefs.GetInt("Shadows", 1))
        {
            case 0:
                shadowsButton.text = "Sì";
                PlayerPrefs.SetInt("Shadows", 1);

                break;
            case 1:
                shadowsButton.text = "No";
                PlayerPrefs.SetInt("Shadows", 0);
                break;
        }

        PlayerPrefs.Save();
    }
}
