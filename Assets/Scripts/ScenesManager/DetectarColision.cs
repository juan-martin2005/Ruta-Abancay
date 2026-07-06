using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DetectarColision : MonoBehaviour
{
    public float tiempoEspera = 2f;

    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.CompareTag("Enemigo"))
        {
            StartCoroutine(MostrarPanelPerdiste());
        }
    }

    IEnumerator MostrarPanelPerdiste(){
        PlayerPrefs.SetString("UltimoNivel", SceneManager.GetActiveScene().name);
        yield return new WaitForSeconds(tiempoEspera);
        SceneManager.LoadScene("GameOver");
    }

}
