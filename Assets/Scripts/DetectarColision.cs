using System.Collections;
using UnityEngine;

public class DetectarColision : MonoBehaviour
{
    public GameObject menuPerder;
    public float tiempoEspera = 2f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemigo"))
        {
            StartCoroutine(MostrarPanelPerdiste());
        }
    }

    IEnumerator MostrarPanelPerdiste()
    {
        yield return new WaitForSeconds(tiempoEspera);

        if (menuPerder != null) menuPerder.SetActive(true);
    }

}
