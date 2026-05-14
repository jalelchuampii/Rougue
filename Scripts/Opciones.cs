using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Opciones : MonoBehaviour
{
    public static Opciones instancia;

    [Header("Brillo")]
    public Image panelBrillo;

    private float brilloActual = 0f;

    void Awake()
    {
        instancia = this;
    }

    void Start()
    {
        float volumen = PlayerPrefs.GetFloat("Volumen", 1f);
        AudioListener.volume = volumen;

        brilloActual = PlayerPrefs.GetFloat("Brillo", 0f);

        AplicarBrillo(brilloActual);
    }

    // ---------------- SONIDO ----------------

    public void ActivarSonido()
    {
        AudioListener.volume = 1f;

        PlayerPrefs.SetFloat("Volumen", 1f);
        PlayerPrefs.Save();
    }

    public void DesactivarSonido()
    {
        AudioListener.volume = 0f;

        PlayerPrefs.SetFloat("Volumen", 0f);
        PlayerPrefs.Save();
    }

    // ---------------- BRILLO ----------------

    public void Brillo20()
    {
        CambiarBrillo(0.8f);
    }

    public void Brillo50()
    {
        CambiarBrillo(0.5f);
    }

    public void Brillo75()
    {
        CambiarBrillo(0.25f);
    }

    public void Brillo100()
    {
        CambiarBrillo(0f);
    }

    void CambiarBrillo(float alpha)
    {
        brilloActual = alpha;

        AplicarBrillo(alpha);

        PlayerPrefs.SetFloat("Brillo", alpha);
        PlayerPrefs.Save();
    }

    void AplicarBrillo(float alpha)
    {
        if (panelBrillo != null)
        {
            Color color = panelBrillo.color;
            color.a = alpha;
            panelBrillo.color = color;
        }
    }
}