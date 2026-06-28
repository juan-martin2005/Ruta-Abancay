using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Jugador : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D col;

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
    }

    void Update()
    {
        movimientoX = (Keyboard.current.dKey.isPressed ? 1 : 0) - (Keyboard.current.aKey.isPressed ? 1 : 0);

        if(Keyboard.current.spaceKey.wasPressedThisFrame && !isSalto && !estaAturdido) intentoSalto = true;

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
        // Si está aturdido (por el bache), se aplica la velocidad reducida
        float velocidadActual = estaAturdido ? velocidadReducida : velocidad;


        //Aplicar el movimiento al Rigidbody
        if (bloqueadoPorPared)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(velocidadActual * movimientoX, rb.linearVelocity.y);
        }

        if (intentoSalto) // Añadido !isSalto para evitar saltos infinitos en el aire
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, salto);
            isSalto = true;
            intentoSalto = false;
        }

        FlipCharacter();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("piso") || collision.gameObject.CompareTag("Suelo") || collision.gameObject.CompareTag("Obstaculo"))
        {
            isSalto = false;
        }

    }

    private bool VerificarBloqueoPared()
    {
        if (movimientoX == 0 || col == null) return false;

        float dirX = Mathf.Sign(movimientoX);
        
        // Calculamos dos puntos desde donde lanzar rayos: uno un poco por encima de los pies y otro cerca de la cabeza
        Vector2 origenAbajo = new Vector2(col.bounds.center.x, col.bounds.min.y + 0.2f);
        Vector2 origenArriba = new Vector2(col.bounds.center.x, col.bounds.max.y - 0.2f);

        // La distancia será la mitad del ancho del jugador más un pequeño margen extra
        float distancia = col.bounds.extents.x + 0.3f;

        // Lanzamos los rayos horizontalmente usando RaycastAll para poder ignorar al propio jugador
        RaycastHit2D[] hitsAbajo = Physics2D.RaycastAll(origenAbajo, new Vector2(dirX, 0), distancia);
        RaycastHit2D[] hitsArriba = Physics2D.RaycastAll(origenArriba, new Vector2(dirX, 0), distancia);

        foreach (var hit in hitsAbajo)
        {
            if (hit.collider != null && hit.collider.gameObject != this.gameObject && hit.collider.CompareTag("Obstaculo"))
                return true;
        }
        foreach (var hit in hitsArriba)
        {
            if (hit.collider != null && hit.collider.gameObject != this.gameObject && hit.collider.CompareTag("Obstaculo"))
                return true;
        }

        return false;
    }

    // Se ejecuta cuando el jugador entra en el área del bache (Trigger)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bache") && !estaAturdido)
        {
            estaAturdido = true;

            // El personaje se recuperará y volverá a su velocidad normal tras 2 segundos
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
