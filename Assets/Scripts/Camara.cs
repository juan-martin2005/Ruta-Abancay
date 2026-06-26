using UnityEngine;

public class Camara : MonoBehaviour
{
    [SerializeField] private Transform objetivo;
    [SerializeField] private float suavizado = 5f;
    [SerializeField] private Vector3 desplazamiento;

    private void LateUpdate()
    {
        if (objetivo == null) return;

        Vector3 posicionDeseada = objetivo.position + desplazamiento;

        Vector3 posicionSuavizada = Vector3.Lerp(transform.position, posicionDeseada, suavizado * Time.deltaTime);

        transform.position = posicionSuavizada;
    }
}
