using UnityEngine;

public class GeneradorObstaculo : MonoBehaviour
{
    public GameObject obstaculo1;
    public GameObject obstaculo2;
    public GameObject obstaculo3;
    public GameObject obstaculo4; // El bache
    public Transform jugador;

    public float distanciaEntreSpawns = 25f;
    public float distanciaAdelante = 20f;
    
    // Distancias para los obstáculos
    public float distanciaEntreObstaculos = 2.33f; // Diferencia entre 16.1 y 13.77
    public float distanciaBache = 15.0f; // Aparición un poco más lejana del bache

    // Alturas individuales para cada obstáculo
    public float alturaObs1 = -2.80f;
    public float alturaObs2 = -1.60f;
    public float alturaObs3 = -1.60f;
    public float alturaObs4 = -3.32f; // Altura del bache (ajusta según necesites)

    // Escalas de los obstáculos
    public Vector3 escalaNormal = new Vector3(0.6f, 0.6f, 0.6f);
    public Vector3 escalaBache = new Vector3(0.19f, 0.26f, 0.6f);

    // Límite final del nivel
    public float limiteFinNivel = 175.27f;
    public float distanciaSeguridadFin = 30f; // Distancia a la que dejarán de aparecer obstáculos

    private float proximaX;
    private bool esPatronA = true;

    void Start()
    {
        if (jugador != null)
        {
            proximaX = jugador.position.x + distanciaEntreSpawns;
        }
    }

    void Update()
    {
        if (jugador == null) return;

        // Detener la generación si el jugador está cerca del punto final
        if (jugador.position.x >= (limiteFinNivel - distanciaSeguridadFin)) return;

        if (jugador.position.x >= proximaX)
        {
            GenerarPattern();
            proximaX += distanciaEntreSpawns;
        }
    }

    void GenerarPattern()
    {
        float spawnX = jugador.position.x + distanciaAdelante;

        if (esPatronA)
        {
            // Pattern A: Obstaculo 1 + Obstaculo 2 + Obstaculo 4 (Bache)
            InstanciarObstaculo(obstaculo1, spawnX, alturaObs1, escalaNormal, "Obstaculo");
            InstanciarObstaculo(obstaculo2, spawnX + distanciaEntreObstaculos, alturaObs2, escalaNormal, "Obstaculo");
            InstanciarObstaculo(obstaculo4, spawnX + distanciaBache, alturaObs4, escalaBache, "Bache");
        }
        else
        {
            // Pattern B: Obstaculo 1 + Obstaculo 3 + Obstaculo 4 (Bache)
            InstanciarObstaculo(obstaculo1, spawnX, alturaObs1, escalaNormal, "Obstaculo");
            InstanciarObstaculo(obstaculo3, spawnX + distanciaEntreObstaculos, alturaObs3, escalaNormal, "Obstaculo");
            InstanciarObstaculo(obstaculo4, spawnX + distanciaBache, alturaObs4, escalaBache, "Bache");
        }

        esPatronA = !esPatronA; // Alterna el patrón
    }

    void InstanciarObstaculo(GameObject prefab, float x, float y, Vector3 escala, string etiqueta = "Obstaculo")
    {
        if (prefab == null) return;

        GameObject obj = Instantiate(prefab, new Vector3(x, y, 0), Quaternion.identity);
        obj.transform.localScale = escala;
        obj.tag = etiqueta; // Asignamos el tag correspondiente
    }
}
