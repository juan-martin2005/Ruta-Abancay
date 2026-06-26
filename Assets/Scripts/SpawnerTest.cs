using UnityEngine;

public class SpawnerTest : MonoBehaviour
{
    [Header("Lista de Obstáculos")]
    [SerializeField] private GameObject[] obstaculosPrefabs;

    [Header("Configuración")]
    [SerializeField] private float puntoInicioX = 15f;
    [SerializeField] private float puntoFinalX = 180f;
    [SerializeField] private float alturaSueloY = -3.5f;

    [Header("Frecuencia y Distancia")]
    [SerializeField] private float distanciaMinimaEntreObstaculos = 15f;
    [SerializeField] private float distanciaMaximaEntreObstaculos = 20f;

    void Start()
    {
        GenerarObstaculosLineales();
    }

    void GenerarObstaculosLineales()
    {
        if (obstaculosPrefabs == null || obstaculosPrefabs.Length == 0)
        {
            Debug.LogError("Sin obstaculos");
            return;
        }

        float posicionActualX = puntoInicioX;

        while (posicionActualX < puntoFinalX)
        {
            float distanciaAleatoria = Random.Range(distanciaMinimaEntreObstaculos,distanciaMaximaEntreObstaculos);
            posicionActualX += distanciaAleatoria;

            if (posicionActualX >= puntoFinalX) break;

            Vector3 posicionSpawn = new Vector3(posicionActualX, alturaSueloY, 0f);

            int indiceAleatorio = Random.Range(0, obstaculosPrefabs.Length);
            GameObject obstaculoElegido = obstaculosPrefabs[indiceAleatorio];

            Instantiate(obstaculoElegido, posicionSpawn, Quaternion.identity);
        }
    }
}
