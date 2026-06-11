using UnityEngine;

public class LimiteController : MonoBehaviour
{
    private string limite_izq = "Limite_Izq";
    private string limite_der = "Limite_Der";


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(limite_izq) || collision.gameObject.CompareTag(limite_der))
        {

            Debug.Log("El jugador está chocando contra el límite: " + collision.gameObject.name);
        }
    }
}
