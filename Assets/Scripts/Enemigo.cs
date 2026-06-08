using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public Transform jugador;
    public float detectarRadio = 11f;
    public float velocidadMaxima = 4.2f; // <-- Un poquito menor que el jugador (4.7) para darle ventaja inicial

    [Tooltip("Qué tan rápido acelera. Valores bajos = más fluido y lento al arrancar.")]
    public float aceleracion = 2.5f;

    [Header("Tiempos de Reacción")]
    public float tiempoParaArrancar = 1.5f;
    private float cronometroReaccion = 0f;
    private bool detectadoPreviamente = false;

    private Rigidbody2D rb;
    private Vector2 movimiento;
    private Animator animator;
    private float velocidadActual = 0f; // <-- Guarda la velocidad que va escalando

    private bool estaAhorcando = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (estaAhorcando)
        {
            movimiento = Vector2.zero;
            return;
        }

        float distanciaJugador = Vector2.Distance(transform.position, jugador.position);

        if (distanciaJugador < detectarRadio)
        {
            Vector2 direccion = (jugador.position - transform.position).normalized;

            if (direccion.x < 0) transform.localScale = new Vector2(1f, 1f);
            if (direccion.x > 0) transform.localScale = new Vector2(-1f, 1f);

            // SISTEMA DE ESPERA / REACCIÓN
            if (!detectadoPreviamente)
            {
                cronometroReaccion += Time.deltaTime;
                movimiento = Vector2.zero;
                velocidadActual = 0f; // No acumula velocidad mientras espera

                if (animator != null) animator.SetBool("estaCaminando", false);

                if (cronometroReaccion >= tiempoParaArrancar)
                {
                    detectadoPreviamente = true;
                }
            }
            else
            {
                // Pasó el tiempo de espera: preparamos la dirección
                movimiento = new Vector2(direccion.x, 0);
                if (animator != null) animator.SetBool("estaCaminando", true);
            }
        }
        else
        {
            // Si el jugador escapa del rango, el enemigo frena gradualmente
            movimiento = Vector2.zero;
            detectadoPreviamente = false;
            cronometroReaccion = 0f;
            if (animator != null) animator.SetBool("estaCaminando", false);
        }
    }

    void FixedUpdate()
    {
        if (estaAhorcando)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (detectadoPreviamente && movimiento != Vector2.zero)
        {
            // ACELERACIÓN FLUIDA: Va de 'velocidadActual' a 'velocidadMaxima' paulatinamente
            velocidadActual = Mathf.MoveTowards(velocidadActual, velocidadMaxima, aceleracion * Time.fixedDeltaTime);
        }
        else
        {
            // DESACELERACIÓN FLUIDA: Si se detiene o pierde al jugador, no se clava en seco de golpe
            velocidadActual = Mathf.MoveTowards(velocidadActual, 0f, (aceleracion * 2f) * Time.fixedDeltaTime);
        }

        // Aplicamos la velocidad suavizada a las físicas
        rb.linearVelocity = new Vector2(movimiento.x * velocidadActual, rb.linearVelocity.y);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectarRadio);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if ((coll.gameObject.name == "Jugador" || coll.gameObject.CompareTag("Player")) && !estaAhorcando)
        {
            estaAhorcando = true;
            movimiento = Vector2.zero;
            rb.linearVelocity = Vector2.zero;

            if (animator != null)
            {
                animator.SetBool("estaCaminando", false);
                animator.SetTrigger("Ahorcar");
            }

            SpriteRenderer jugadorSprite = coll.gameObject.GetComponent<SpriteRenderer>();
            if (jugadorSprite != null) jugadorSprite.enabled = false;

            Jugador scriptJugador = coll.gameObject.GetComponent<Jugador>();
            if (scriptJugador != null) scriptJugador.enabled = false;

            Rigidbody2D jugadorRb = coll.gameObject.GetComponent<Rigidbody2D>();
            if (jugadorRb != null) jugadorRb.linearVelocity = Vector2.zero;
        }
    }
}