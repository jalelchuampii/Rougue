using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioGlobal : MonoBehaviour
{
    void Start()
    {
        float volumen = PlayerPrefs.GetFloat("Volumen", 1f);

        AudioListener.volume = volumen;
    }
}
