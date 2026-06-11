using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPerder : MonoBehaviour
{

    public void Reiniciar()
    {
        Time.timeScale = 1;

        string nombreEscena = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(nombreEscena);
    }
    public void Salir()
    {
        SceneManager.LoadScene("Menu");
    }
}
