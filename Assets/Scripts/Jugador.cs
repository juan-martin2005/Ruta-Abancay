using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AudioSource))]
public class Jugador : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D col;
    private AudioSource audioSalto;

    [Header("Audio")]
    [SerializeField] private AudioClip clipSalto;

    [SerializeField] private float velocidad = 5.1f;
    [SerializeField] private float salto = 7.3f;
    [SerializeField] private float velocidadReducida = 0.5f;

    public float movimientoX { get; private set; }
    private bool isSalto = false;
    private bool estaAturdido = false;
    private bool intentoSalto = false;
    private bool bloqueadoPorPared = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        audioSalto = GetComponent<AudioSource>();
    }

    void Update()
    {
        movimientoX = (Keyboard.current.dKey.isPressed ? 1 : 0) - (Keyboard.current.aKey.isPressed ? 1 : 0);

        if (Keyboard.current.spaceKey.wasPressedThisFrame && !isSalto && !estaAturdido)
            intentoSalto = true;

        bloqueadoPorPared = VerificarBloqueoPared();

        if (bloqueadoPorPared)
        {
            anim.SetFloat("movement", 0);
        }
        else
        {
            anim.SetFloat("movement", Mathf.Abs(rb.linearVelocity.x));
        }

        anim.SetBool("isJumping", isSalto);
        anim.SetBool("isStumbled", estaAturdido);
    }

    private void FixedUpdate()
    {
        float velocidadActual = estaAturdido ? velocidadReducida : velocidad;

        if (bloqueadoPorPared)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(velocidadActual * movimientoX, rb.linearVelocity.y);
        }

        if (intentoSalto)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, salto);
            isSalto = true;

            if (audioSalto != null && clipSalto != null)
            {
                audioSalto.PlayOneShot(clipSalto);
            }

            intentoSalto = false;
        }

        FlipCharacter();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("piso") ||
            collision.gameObject.CompareTag("Suelo") ||
            collision.gameObject.CompareTag("Obstaculo"))
        {
            isSalto = false;
        }
    }

    private bool VerificarBloqueoPared()
    {
        if (movimientoX == 0 || col == null) return false;

        float dirX = Mathf.Sign(movimientoX);

        Vector2 origenAbajo = new Vector2(col.bounds.center.x, col.bounds.min.y + 0.2f);
        Vector2 origenArriba = new Vector2(col.bounds.center.x, col.bounds.max.y - 0.2f);

        float distancia = col.bounds.extents.x + 0.3f;

        RaycastHit2D[] hitsAbajo = Physics2D.RaycastAll(origenAbajo, new Vector2(dirX, 0), distancia);
        RaycastHit2D[] hitsArriba = Physics2D.RaycastAll(origenArriba, new Vector2(dirX, 0), distancia);

        foreach (var hit in hitsAbajo)
        {
            if (hit.collider != null &&
                hit.collider.gameObject != gameObject &&
                hit.collider.CompareTag("Obstaculo"))
                return true;
        }

        foreach (var hit in hitsArriba)
        {
            if (hit.collider != null &&
                hit.collider.gameObject != gameObject &&
                hit.collider.CompareTag("Obstaculo"))
                return true;
        }

        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bache") && !estaAturdido)
        {
            estaAturdido = true;

            Invoke("RecuperarVelocidad", 0.3f);
        }
    }

    void RecuperarVelocidad()
    {
        estaAturdido = false;
    }

    void FlipCharacter()
    {
        if (movimientoX < 0 && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-0.9f, 0.9f, 1f);
        }
        else if (movimientoX > 0 && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(0.9f, 0.9f, 1f);
        }
    }
}