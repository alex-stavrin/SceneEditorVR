using UnityEngine;

public class ControlPanel : MonoBehaviour
{
    [SerializeField]
    Vector2 offsetFromCamera = new Vector2();

    Transform playerHead;
    void Start()
    {
        playerHead = PlayerRig.Instance.GetPlayerHead();
    }

    void Update()
    {
        Vector3 flatForward = Vector3.ProjectOnPlane(playerHead.forward, Vector3.up).normalized;

        // new position
        transform.position =
            playerHead.position
            + flatForward * offsetFromCamera.x
            + Vector3.up * offsetFromCamera.y;
    }
}
