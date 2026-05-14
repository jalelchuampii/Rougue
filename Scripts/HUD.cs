using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{
	public GameManager gameManager;
    public TextMeshProUGUI puntos;
	public GameObject[] vidas;

	public void DesactivarVida(int indice) {
		vidas[indice].SetActive(false);
	}

	public void ActivarVida(int indice) {
		vidas[indice].SetActive(true);
	}
	

    // Update is called once per frame
    void Update()
    {
        puntos.text = gameManager.PuntosTotales.ToString();
    }
}