using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingManager : MonoBehaviour
{
    [SerializeField] PostProcessVolume p;

    void Start()
    {
        if (p.profile.TryGetSettings(out AmbientOcclusion ambientOcclusion))
        {
            switch (PlayerPrefs.GetInt("AmbientOcclusion", 1))
            {
                case 0:
                    ambientOcclusion.enabled.value = false;
                    break;
                case 1:
                    ambientOcclusion.enabled.value = true;
                    break;
            }
        }

        if (p.profile.TryGetSettings(out Bloom bloom))
        {
            switch (PlayerPrefs.GetInt("Bloom", 1))
            {
                case 0:
                    bloom.enabled.value = false;
                    break;
                case 1:
                    bloom.enabled.value = true;
                    break;
            }
        }
    }
}
