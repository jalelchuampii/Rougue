using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtaquePersonaje : MonoBehaviour
{
    public Transform puntoAtaque;
    public float radioAtaque = 0.5f;
    public LayerMask capaEnemigos;

    public int dañoAtaque = 1;

    public Animator animator;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Atacar();
        }
    }

    void Atacar()
    {
        // Animación
        animator.SetTrigger("IsAttacking");

        // Detectar enemigos
        Collider2D[] enemigos = Physics2D.OverlapCircleAll(
            puntoAtaque.position,
            radioAtaque,
            capaEnemigos
        );

        // Hacer daño
        foreach (Collider2D enemigo in enemigos)
        {
            enemigo.GetComponent<EnemigoVida>()?.RecibirDaño(dañoAtaque);
        }
    }

    // Mostrar círculo en escena
    private void OnDrawGizmosSelected()
    {
        if (puntoAtaque == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(puntoAtaque.position, radioAtaque);
    }
}