using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public Transform jugador;
    private Rigidbody2D rb;
    private Animator animator;
    private AudioSource audioSource;

    public float detectarRadio = 16f;
    public float velocidadMaxima = 4.5f;
    public float aceleracion = 2.5f;

    [Header("Audio")]
    public AudioClip sonidoDeteccion;

    [Header("Tiempos de Reacción")]
    public float tiempoParaArrancar = 0.08f;
    private float cronometroReaccion = 0f;
    private bool detectadoPreviamente = false;

    private Vector2 movimientoX;
    private float velocidadActual = 0.6f;
    private bool estaAhorcando = false;

    // Evita que el sonido se repita mientras el jugador siga dentro del rango
    private bool sonidoReproducido = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (estaAhorcando) return;

        float distanciaJugador = Mathf.Abs(jugador.position.x - transform.position.x);

        if (distanciaJugador < detectarRadio)
        {
            // Reproduce el sonido UNA SOLA VEZ cuando detecta al jugador
            if (!sonidoReproducido)
            {
                audioSource.PlayOneShot(sonidoDeteccion);
                sonidoReproducido = true;
            }

            float direccion = jugador.position.x > transform.position.x ? 1f : -1f;

            if (direccion < 0) transform.localScale = new Vector2(1f, 1f);
            if (direccion > 0) transform.localScale = new Vector2(-1f, 1f);

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
                movimientoX = new Vector2(direccion, 0f);
            }
        }
        else
        {
            movimientoX = Vector2.zero;
            detectadoPreviamente = false;
            cronometroReaccion = 0f;

            // Permite que vuelva a sonar cuando el jugador salga y vuelva a entrar
            sonidoReproducido = false;
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

            if (scriptJugador != null)
            {
                scriptJugador.enabled = false;

                SpriteRenderer jugadorSprite = coll.gameObject.GetComponent<SpriteRenderer>();
                if (jugadorSprite != null)
                    jugadorSprite.enabled = false;

                Rigidbody2D jugadorRb = coll.gameObject.GetComponent<Rigidbody2D>();
                if (jugadorRb != null)
                    jugadorRb.linearVelocity = Vector2.zero;
            }
        }
    }
}