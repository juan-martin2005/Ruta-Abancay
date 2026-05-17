using System;
using UnityEngine;

public class Fondo : MonoBehaviour
{
    private Material material;
    [SerializeField] private Jugador jugador;
    private readonly float velodidad = 0.18f;
    void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    void Update()
    {
        Vector2 vec = material.mainTextureOffset;
        if (Math.Abs(jugador.movimiento) > 0.1f)
        {
            vec.x += jugador.movimiento * velodidad * Time.deltaTime;
            material.mainTextureOffset = vec;
        }

    }
}
