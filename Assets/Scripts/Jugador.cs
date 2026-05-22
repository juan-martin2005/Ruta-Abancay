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
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {

        anim.SetFloat("movement", rb.linearVelocity.x);
        anim.SetBool("isJumping",isSalto);
    }

    private void FixedUpdate()
    {
        float velocidad_actual = velocidad;
        movimiento = (Keyboard.current.dKey.isPressed ? 1 : 0) - (Keyboard.current.aKey.isPressed ? 1 : 0);

        rb.linearVelocity = new Vector2(velocidad_actual * movimiento, rb.linearVelocity.y);

        bool saltar = Keyboard.current.spaceKey.isPressed;

        if (saltar)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, salto);
            isSalto = true;
        }
        FlipCharacter();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("piso"))
        {
            isSalto = false;
        }
        
    }

    void FlipCharacter()
    {
        if (movimiento < 0) transform.localScale = new Vector2(-1f, 1f);
        if (movimiento > 0) transform.localScale = new Vector2(1f, 1f);
    }
}
