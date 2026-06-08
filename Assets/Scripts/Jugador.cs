using UnityEngine;
using UnityEngine.InputSystem;

public class Jugador : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private readonly float velocidad = 4.7f;
    private readonly float salto = 7.3f;
    public float movimiento;
    private bool isSalto = false;
    private bool estaAturdido = false;
    private readonly float velocidadReducida = 0.1f;
    private Animator jugadorAnim;
    private readonly float velocidadCaminar = 3.0f; // Velocidad normal
    private readonly float velocidadCorrer = 5.5f;  // Velocidad al correr
    private bool estaCorriendo = false;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {

        anim.SetFloat("movement", rb.linearVelocity.x);
        anim.SetBool("isJumping", isSalto);
        anim.SetBool("isStumbled", estaAturdido);
    }

    private void FixedUpdate()
    {
        // 1. Detectar si el jugador está presionando Shift Izquierdo para correr
        estaCorriendo = Keyboard.current.leftShiftKey.isPressed;

        // 2. Elegir la velocidad base dependiendo de si camina o corre
        float velocidadBase = estaCorriendo ? velocidadCorrer : velocidadCaminar;

        // 3. Si está aturdido (por el bache), se aplica la velocidad reducida
        float velocidad_actual = estaAturdido ? velocidadReducida : velocidadBase;

        // 4. Leer el movimiento de las teclas A y D
        movimiento = (Keyboard.current.dKey.isPressed ? 1 : 0) - (Keyboard.current.aKey.isPressed ? 1 : 0);

        // 5. Aplicar el movimiento al Rigidbody
        rb.linearVelocity = new Vector2(velocidad_actual * movimiento, rb.linearVelocity.y);

        // Código del salto...
        bool saltar = Keyboard.current.spaceKey.isPressed;
        if (saltar && !isSalto) // Añadido !isSalto para evitar saltos infinitos en el aire
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, salto);
            isSalto = true;
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
            Invoke("RecuperarVelocidad", 0.9f);
        }
    }

    void RecuperarVelocidad()
    {
        estaAturdido = false;
    }


    public void estaAhorcando(float tiempo)
    {
        // 1. Detener el movimiento del jugador aquí (ej. velocidad = 0)

        // 2. Activar la animación del JUGADOR sufriendo
        if (jugadorAnim != null)
        {
            // Supongamos que creaste este parámetro en el Animator del Jugador
            jugadorAnim.SetBool("recibiendoAhorque", true);
            Invoke("Recuperarse", tiempo);
        }
    }



    void FlipCharacter()
    {
        if (movimiento < 0) transform.localScale = new Vector2(-1f, 1f);
        if (movimiento > 0) transform.localScale = new Vector2(1f, 1f);
    }



}
