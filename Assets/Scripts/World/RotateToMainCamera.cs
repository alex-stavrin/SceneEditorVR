using UnityEngine;

public class RotateToMainCamera : MonoBehaviour
{
    Transform mainCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction =   transform.position - mainCamera.position;
        direction.Normalize();
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
