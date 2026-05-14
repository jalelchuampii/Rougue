using UnityEngine;

public class EnemigoVida : MonoBehaviour
{
    public int vida = 3;

    private Animator animator;
    private bool muerto = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void RecibirDaño(int daño)
    {
        // Evita recibir daño después de morir
        if (muerto)
            return;

        vida -= daño;

        // Animación de golpe
        if (animator != null)
        {
            animator.SetTrigger("Hit");
        }

        // Comprobar muerte
        if (vida <= 0)
        {
            Morir();
        }
    }

    void Morir()
    {
        muerto = true;

        // Animación de muerte
        if (animator != null)
        {
            animator.SetTrigger("Dead");
        }

        // Desactivar collider para evitar más golpes
        Collider2D collider = GetComponent<Collider2D>();

        if (collider != null)
        {
            collider.enabled = false;
        }

        // Destruir enemigo después de la animación
        Destroy(gameObject, 0.5f);
    }
}