using UnityEngine;

public class RadialManager : MonoBehaviour
{
    public static RadialManager Instance { get; private set; }

    [SerializeField]
    GameObject radial;

    private Controller currentController;

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
        radial.SetActive(false);
    }

    void Update()
    {
        if (currentController)
        {
            Vector3 radialUp = radial.transform.up;
            Vector3 radialPlane = radial.transform.forward;
            Vector3 controllerDirection = currentController.transform.position - radial.transform.position;
            Vector3 projectedControllerDirection = Vector3.ProjectOnPlane(controllerDirection, radialPlane);
            float radialAngle = Vector3.SignedAngle(radialUp, projectedControllerDirection, radialPlane);

            VirtualRealityConsole.PrintMessage(radialAngle.ToString(), PrintTypeVRC.Clear);
        }
    }

    public void CallRadial(Controller controllerCalling)
    {
        currentController = controllerCalling;
        radial.SetActive(true);
        radial.transform.position = controllerCalling.transform.position;
        Vector3 direction = radial.transform.position - PlayerRig.Instance.GetPlayerHead().position;
        direction.Normalize();
        direction.y = 0;
        radial.transform.rotation = Quaternion.LookRotation(-direction);
    }

    public void DismissRadial()
    {
        if (currentController)
        {            
            currentController = null;
            radial.SetActive(false);
        }
    }
}
