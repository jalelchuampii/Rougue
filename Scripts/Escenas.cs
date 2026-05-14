using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Escenas : MonoBehaviour
{
   public void cambiarescena1(){
    SceneManager.LoadScene("Inicio");
   }
   public void cambiarescenaOpciones(){
    SceneManager.LoadScene("Opciones");
   }
   public void camiarescenaInicio(){
    SceneManager.LoadScene("juego");
   }
    public void SalirJuego()
    {
        Debug.Log("Saliendo del juego...");

        Application.Quit();
    }
}
