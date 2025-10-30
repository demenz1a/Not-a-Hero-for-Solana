using UnityEngine;

public class HealthBarFollow : MonoBehaviour
{
    [SerializeField] private Transform target; 

    private Vector3 initialScale;

    void Update()
    {
        transform.rotation = Quaternion.identity;
    }
}
