using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParameterInitializer : MonoBehaviour
{
    public DayNightCycle dayNightCycle;
    public Light sun;
    public Material newSkybox;
    public GameObject rain;

    void Awake()
    {
        // Inizializza l'ora del giorno
        dayNightCycle._timeOfDay = PlayerPrefs.GetFloat("Time", 0);

        // Inizializza il meteo
        switch (PlayerPrefs.GetInt("Weather", 0))
        {
            case 1:
                dayNightCycle.sunVariation = -0.2f;
                RenderSettings.skybox = newSkybox;
                rain.SetActive(false);
                sun.renderMode = LightRenderMode.ForceVertex;
                break;

            case 2:
                dayNightCycle.sunVariation = -0.2f;
                RenderSettings.skybox = newSkybox;
                rain.SetActive(true);
                sun.renderMode = LightRenderMode.ForceVertex;
                break;
        }

        // Inizializza le ombre
        switch (PlayerPrefs.GetInt("Shadows", 1))
        {
            case 0:
                sun.shadows = LightShadows.None;
                break;

            case 1:
                sun.shadows = LightShadows.Soft;
                break;
        }

    }
}
