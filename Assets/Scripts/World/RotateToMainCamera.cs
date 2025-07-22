using UnityEngine;

public class RotateToMainCamera : MonoBehaviour
{
    Transform mainCamera;

    void Start()
    {
        mainCamera = Camera.main.transform;
    }

    void Update()
    {
        Vector3 direction =   transform.position - mainCamera.position;
        direction.Normalize();
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
