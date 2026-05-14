using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("HUD")]
    public HUD hud;

    [Header("Jugador")]
    public MovimientoPersonaje playerMovement;
    public Animator playerAnimator;
    public int PuntosTotales { get { return puntosTotales; } }
    private int puntosTotales;
    
     public void SumarPuntos(int puntosASumar)
    {
        puntosTotales += puntosASumar;
        Debug.Log(puntosTotales);
    }

    [Header("UI")]
    public GameObject deathOverlay;

    [Header("Configuración")]
    public int vidasMaximas = 5;
    public float tiempoMuerte = 1f;

    private int vidas;
    private bool estaMuerto = false;

    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        Time.timeScale = 1f;
    }

    private void Start()
    {
        vidas = vidasMaximas;

        // Ocultar overlay al iniciar
        if (deathOverlay != null)
        {
            deathOverlay.SetActive(false);
        }
        else
        {
            Debug.LogError("No se asignó Death Overlay.");
        }
    }

    // =========================
    // PERDER VIDA
    // =========================
    public void PerderVida()
    {
        // Evitar múltiples muertes
        if (estaMuerto)
            return;

        vidas--;

        // Actualizar HUD
        if (hud != null)
        {
            hud.DesactivarVida(vidas);
        }

        // Comprobar muerte
        if (vidas <= 0)
        {
            StartCoroutine(IsDead());
        }
    }

    // =========================
    // MUERTE
    // =========================
    IEnumerator IsDead()
    {
        estaMuerto = true;

        // Bloquear jugador + animación muerte
        if (playerMovement != null)
        {
            playerMovement.Morir();
        }
        else
        {
            // Fallback si no asignaste playerMovement
            if (playerAnimator != null)
            {
                playerAnimator.SetTrigger("IsDead");
            }
        }

        // Esperar animación
        yield return new WaitForSeconds(tiempoMuerte);

        // Mostrar overlay
        if (deathOverlay != null)
        {
            deathOverlay.SetActive(true);
        }

        // Pausar juego
        Time.timeScale = 0f;
    }

    // =========================
    // REINTENTAR
    // =========================
    public void Reintentar()
    {
        Time.timeScale = 1f;
        estaMuerto = false;

        Scene escenaActual = SceneManager.GetActiveScene();
        SceneManager.LoadScene(escenaActual.buildIndex);
    }

    // =========================
    // VOLVER AL MENÚ
    // =========================
    public void VolverAlInicio()
    {
        Time.timeScale = 1f;
        estaMuerto = false;

        SceneManager.LoadScene(0);
    }

    // =========================
    // RECUPERAR VIDA
    // =========================
    public bool RecuperarVida()
    {
        if (vidas >= vidasMaximas)
        {
            return false;
        }

        // Actualizar HUD
        if (hud != null)
        {
            hud.ActivarVida(vidas);
        }

        vidas++;

        return true;
    }
}