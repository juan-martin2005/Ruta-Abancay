using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public Transform jugador;
    private Rigidbody2D rb;
    private Animator animator;

    public float detectarRadio = 16f;
    public float velocidadMaxima = 4.5f; 
    public float aceleracion = 2.5f;

    [Header("Tiempos de Reacción")]
    public float tiempoParaArrancar = 0.08f;
    private float cronometroReaccion = 0f;
    private bool detectadoPreviamente = false;

    private Vector2 movimientoX;
    private float velocidadActual = 0.6f; // <-- Guarda la velocidad que va escalando
    private bool estaAhorcando = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (estaAhorcando) return;

        float distanciaJugador = Mathf.Abs(jugador.position.x - transform.position.x);

        if (distanciaJugador < detectarRadio)
        {
            float direccion = jugador.position.x > transform.position.x ? 1f : -1f;

            if (direccion < 0) transform.localScale = new Vector2(1f, 1f);
            if (direccion > 0) transform.localScale = new Vector2(-1f, 1f);

            // SISTEMA DE ESPERA / REACCIÓN
            if (!detectadoPreviamente)
            {
                cronometroReaccion += Time.deltaTime;
                movimientoX = Vector2.zero;
                velocidadActual = 0f;

                if (cronometroReaccion >= tiempoParaArrancar)
                {
                    detectadoPreviamente = true;
                }
            }
            else
            {
                // Pasó el tiempo de espera: preparamos la dirección
                movimientoX = new Vector2(direccion, 0f);
            }
        }
        else
        {
            // Si el jugador escapa del rango, el enemigo frena gradualmente
            movimientoX = Vector2.zero;
            detectadoPreviamente = false;
            cronometroReaccion = 0f;
        }
    }

    void FixedUpdate()
    {
        if (estaAhorcando)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        if (detectadoPreviamente && movimientoX != Vector2.zero)
        {
            velocidadActual = Mathf.MoveTowards(velocidadActual, velocidadMaxima, aceleracion * Time.fixedDeltaTime);
        }
        else
        {
            velocidadActual = Mathf.MoveTowards(velocidadActual, 0f, (aceleracion * 2f) * Time.fixedDeltaTime);
        }

        rb.linearVelocity = new Vector2(movimientoX.x * velocidadActual, rb.linearVelocity.y);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectarRadio);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstaculo"))
        {
            Destroy(other.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Obstaculo"))
        {
            Destroy(coll.gameObject);
        }

        if (coll.gameObject.CompareTag("Jugador") && !estaAhorcando)
        {
            estaAhorcando = true;
            movimientoX = Vector2.zero;

            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

            if (animator != null)
            {
                animator.SetTrigger("Ahorcar");
            }

            Jugador scriptJugador = coll.gameObject.GetComponent<Jugador>();

            if(scriptJugador != null)
            {
                scriptJugador.enabled = false;
                SpriteRenderer jugadorSprite = coll.gameObject.GetComponent<SpriteRenderer>();
                if (jugadorSprite != null) jugadorSprite.enabled = false;

                Rigidbody2D jugadorRb = coll.gameObject.GetComponent<Rigidbody2D>();
                if (jugadorRb != null) jugadorRb.linearVelocity = Vector2.zero;
            }
        }
    }
}