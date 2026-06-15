using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPerder : MonoBehaviour
{
    public void Reiniciar(){
        Time.timeScale = 1;
        SceneManager.LoadScene("level1");
    }

    public void Salir(){
        SceneManager.LoadScene("Menu");
    }
}
