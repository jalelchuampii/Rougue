using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoPersonaje : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 5f;

    [Header("Salto")]
    public float fuerzaSalto = 10f;
    public float saltosMaximos = 2;

    [Header("Combate")]
    public float fuerzaGolpe = 10f;

    [Header("Suelo")]
    public LayerMask capaSuelo;

    [Header("Audio")]
    public AudioClip sonidoSalto;

    // Componentes
    private Rigidbody2D rigidBody;
    private BoxCollider2D boxCollider;
    private Animator animator;

    // Estado
    private bool mirandoDerecha = true;
    private float saltosRestantes;
    private bool puedeMoverse = true;

    // MUERTE
    public bool estaMuerto = false;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();

        saltosRestantes = saltosMaximos;
    }

    void Update()
    {
        // Si está muerto no hacemos nada
        if (estaMuerto)
            return;

        ProcesarMovimiento();
        ProcesarSalto();

        // Animación salto
        animator.SetBool("IsJumping", !EstaEnSuelo());
    }

    bool EstaEnSuelo()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            boxCollider.bounds.center,
            new Vector2(boxCollider.bounds.size.x * 0.9f, boxCollider.bounds.size.y),
            0f,
            Vector2.down,
            0.3f,
            capaSuelo
        );

        return raycastHit.collider != null;
    }

    void ProcesarMovimiento()
    {
        if (!puedeMoverse)
            return;

        float inputMovimiento = Input.GetAxis("Horizontal");

        // Animación correr
        if (inputMovimiento != 0f)
        {
            animator.SetBool("IsRunning", true);
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }

        // Movimiento
        rigidBody.velocity = new Vector2(
            inputMovimiento * velocidad,
            rigidBody.velocity.y
        );

        // Girar personaje
        GestionarOrientacion(inputMovimiento);
    }

    void ProcesarSalto()
    {
        if (EstaEnSuelo())
        {
            saltosRestantes = saltosMaximos;
        }

        if (Input.GetKeyDown(KeyCode.Space) && saltosRestantes > 0)
        {
            saltosRestantes--;

            // Reiniciar velocidad vertical
            rigidBody.velocity = new Vector2(
                rigidBody.velocity.x,
                0f
            );

            // Aplicar salto
            rigidBody.AddForce(
                Vector2.up * fuerzaSalto,
                ForceMode2D.Impulse
            );

            // Animación salto
            animator.SetBool("IsJumping", true);
        }
    }

    void GestionarOrientacion(float inputMovimiento)
    {
        if (
            (mirandoDerecha && inputMovimiento < 0) ||
            (!mirandoDerecha && inputMovimiento > 0)
        )
        {
            mirandoDerecha = !mirandoDerecha;

            transform.localScale = new Vector2(
                -transform.localScale.x,
                transform.localScale.y
            );
        }
    }

    public void AplicarGolpe()
    {
        // Si está muerto no recibe golpes
        if (estaMuerto)
            return;

        puedeMoverse = false;

        Vector2 direccionGolpe;

        if (rigidBody.velocity.x > 0)
        {
            direccionGolpe = new Vector2(-1, 1);
        }
        else
        {
            direccionGolpe = new Vector2(1, 1);
        }

        // Reiniciar velocidad antes del golpe
        rigidBody.velocity = Vector2.zero;

        // Aplicar fuerza
        rigidBody.AddForce(
            direccionGolpe * fuerzaGolpe,
            ForceMode2D.Impulse
        );

        StartCoroutine(EsperarYActivarMovimiento());
    }

    IEnumerator EsperarYActivarMovimiento()
    {
        // Espera mínima
        yield return new WaitForSeconds(0.1f);

        // Esperar a tocar suelo
        while (!EstaEnSuelo())
        {
            yield return null;
        }

        // Reactivar movimiento
        puedeMoverse = true;
    }

    // LLAMAR CUANDO EL JUGADOR MUERA
    public void Morir()
    {
        estaMuerto = true;
        puedeMoverse = false;

        // Parar movimiento
        rigidBody.velocity = Vector2.zero;

        // Reset animaciones
        animator.SetBool("IsRunning", false);
        animator.SetBool("IsJumping", false);

        // Trigger muerte
        animator.SetTrigger("IsDead");
    }
}