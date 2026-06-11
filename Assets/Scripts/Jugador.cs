using UnityEngine;
using UnityEngine.InputSystem;

public class Jugador : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    private readonly float velocidad = 4.7f;
    private readonly float salto = 7.3f;
    private readonly float velocidadReducida = 0.1f;

    public float movimientoX;
    private bool isSalto = false;
    private bool estaAturdido = false;
    private bool intentoSalto = false;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        movimientoX = (Keyboard.current.dKey.isPressed ? 1 : 0) - (Keyboard.current.aKey.isPressed ? 1 : 0);

        if(Keyboard.current.spaceKey.isPressed && !isSalto) intentoSalto = true;

        anim.SetFloat("movement", Mathf.Abs(rb.linearVelocity.x));
        anim.SetBool("isJumping", isSalto);
        anim.SetBool("isStumbled", estaAturdido);
    }

    private void FixedUpdate()
    {

        // Si está aturdido (por el bache), se aplica la velocidad reducida
        float velocidadActual = estaAturdido ? velocidadReducida : velocidad;


        //Aplicar el movimiento al Rigidbody
        rb.linearVelocity = new Vector2(velocidadActual * movimientoX, rb.linearVelocity.y);

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
        if (collision.gameObject.CompareTag("piso") || collision.gameObject.CompareTag("Suelo"))
        {
            isSalto = false;
        }

    }

    // Se ejecuta cuando el jugador entra en el área del bache (Trigger)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Comparamos con el Tag "Obstaculo" que tienes asignado en el Inspector de tu bache
        if (collision.CompareTag("Obstaculo") && !estaAturdido)
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
