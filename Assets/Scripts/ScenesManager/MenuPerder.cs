using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPerder : MonoBehaviour
{
    public void Reiniciar(){
        Time.timeScale = 1;
        string ultimoNivel = PlayerPrefs.GetString("UltimoNivel", "level1");
        SceneManager.LoadScene(ultimoNivel);
    }

    public void Salir(){
        SceneManager.LoadScene("Menu");
    }
}
