using UnityEngine;

public class ControlPanel : MonoBehaviour
{
    [Header("Control Panel Settings")]
    [SerializeField]
    Vector2 offsetFromCamera = new Vector2();

    [Header("World Panel")]

    [SerializeField]
    GameObject worldPanel;

    [Header("Content Panel")]

    [SerializeField]
    GameObject contentPanel;


    [Header("Inspector Panel")]

    [SerializeField]
    GameObject inspectorPanel;


    Transform playerHead;

    public static ControlPanel Instance { get; private set; }

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }


    public void Start()
    {
        playerHead = PlayerRig.Instance.GetPlayerHead();
    }

    public void GoToPlayer()
    {
        Vector3 flatForward = Vector3.ProjectOnPlane(playerHead.forward, Vector3.up).normalized;

        // new position
        transform.position =
            playerHead.position
            + flatForward * offsetFromCamera.x
            + Vector3.up * offsetFromCamera.y;

        // look at player only yaw
        Vector3 direction = playerHead.position - transform.position;
        direction.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(-direction);
        transform.rotation = lookRotation;
    }

    public void OnWorldButtonPressed()
    {
        ActivatePanel(worldPanel);
    }

    public void OnInspectorButtonPressed()
    {
        ActivatePanel(inspectorPanel);
    }

    public void OnContentButtonPressed()
    {
        ActivatePanel(contentPanel);
    }

    void ActivatePanel(GameObject panel)
    {
        panel.SetActive(true);
        panel.transform.rotation = transform.rotation;
    }
}
