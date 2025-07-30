using UnityEngine;

public class ControlPanel : MonoBehaviour
{
    [Header("Control Panel Settings")]
    [SerializeField]
    Vector2 offsetFromCamera = new Vector2();

    [Header("World Panel")]

    [SerializeField]
    GameObject worldPanel;

    [SerializeField]
    Vector3 worldPanelOffsetFromControlPanel;

    [Header("Content Panel")]

    [SerializeField]
    GameObject contentPanel;

    [SerializeField]
    Vector3 contentPanelOffsetFromControlPanel;


    [Header("Inspector Panel")]

    [SerializeField]
    GameObject inspectorPanel;

    [SerializeField]
    Vector3 inspectorPanelOffsetFromControlPanel;


    Transform playerHead;

    public static ControlPanel Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    void Start()
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
        ActivatePanel(worldPanel, worldPanelOffsetFromControlPanel);
    }

    public void OnInspectorButtonPressed()
    {
        ActivatePanel(inspectorPanel, inspectorPanelOffsetFromControlPanel);
    }

    public void OnContentButtonPressed()
    {
        ActivatePanel(contentPanel, contentPanelOffsetFromControlPanel);
    }

    void ActivatePanel(GameObject panel, Vector3 offset)
    {
        panel.SetActive(true);
        panel.transform.rotation = transform.rotation;
        panel.transform.position = transform.position + transform.forward * offset.z
            + transform.right * offset.x +
            transform.up * offset.y;
    }
}
