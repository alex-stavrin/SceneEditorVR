using UnityEngine;

public class RotateToMainCamera : MonoBehaviour
{
    [SerializeField]
    bool onlyYaw = false;

    Transform mainCamera;

    void Start()
    {
        mainCamera = Camera.main.transform;
    }

    void Update()
    {
        Vector3 direction =   transform.position - mainCamera.position;
        direction.Normalize();
        if (onlyYaw)
        {
            direction.y = 0;
        }
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
