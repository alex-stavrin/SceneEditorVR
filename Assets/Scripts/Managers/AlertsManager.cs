using UnityEngine;

public class AlertsManager : MonoBehaviour
{
    private static AlertsManager _instance;
    public static AlertsManager Instance { get { return _instance; } }

    [SerializeField]
    Alert alert;

    [SerializeField]
    Vector2 offsetFromCamera = new Vector2();

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public static void SpawnAlert(string alertText)
    {
        if (!Instance.alert) return;

        Alert alert = Instance.alert;

        alert.gameObject.SetActive(true);

        Transform playerHead = PlayerRig.Instance.GetPlayerHead();

        Vector3 flatForward = Vector3.ProjectOnPlane(playerHead.forward, Vector3.up).normalized;

        // new position
        alert.transform.position =
            playerHead.position
            + flatForward * Instance.offsetFromCamera.x
            + Vector3.up * Instance.offsetFromCamera.y;

        // look at player only yaw
        Vector3 direction = playerHead.position - alert.transform.position;
        direction.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(-direction);
        alert.transform.rotation = lookRotation;

        alert.SetText(alertText);
    }
}