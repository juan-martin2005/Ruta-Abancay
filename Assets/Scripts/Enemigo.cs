using UnityEngine;

public class Enemigo : MonoBehaviour
{

    public Transform jugador;
    public float detectarRadio = 8.60f;
    public float velocidad = 16.0f;

    private Rigidbody2D rb;
    private Vector2 movimiento;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        
    }

    void Update()
    {
        float distanciaJugador = Vector2.Distance(transform.position, jugador.position);
        if (distanciaJugador < detectarRadio)
        {
            Vector2 direccion = (jugador.position - transform.position).normalized;
            movimiento = new Vector2(direccion.x, 0);
        }
        else
        {
            movimiento = Vector2.zero;
        }

        rb.MovePosition(rb.position + movimiento * velocidad * Time.deltaTime);
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectarRadio);
    }
}
