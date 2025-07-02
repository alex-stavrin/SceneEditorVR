using UnityEngine;
using UnityEngine.XR;

[RequireComponent(typeof(Rigidbody))]
public class PlayerRig : MonoBehaviour
{

    public static PlayerRig Instance { get; private set; }

    /// PUBLIC ///

    [Header("Player")]

    [SerializeField] 
    Transform playerHead;

    [SerializeField] 
    Transform cameraOffset;

    [SerializeField] 
    CapsuleCollider bodyCollider;

    [SerializeField]
    float bodyHeightMin = 0.5f;

    [SerializeField]
    float bodyHeightMax = 2;

    [Header("Controllers")]

    [SerializeField] 
    Controller leftController;

    [SerializeField] 
    Controller rightController;

    [Header("Air Grabbing")]

    [SerializeField]
    bool canAirGrab = true;
    [SerializeField]
    float grabEndForce = 100f;

    [Header("Teleporting")]
    public bool canTeleport = true;
    

    /// PRIVATE ///

    bool isAirGrabbing = false;
    Vector3 currentGrabMovingPoint;
    InputDeviceRole currentGrabMovingSide;
    Vector3 startingGrabOriginPosition;
    Vector3 translationVector;
    Rigidbody rb;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        rb = GetComponent<Rigidbody>();
        //DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        rb.detectCollisions = false;
    }

    void Update()
    {
        if (isAirGrabbing && canAirGrab)
        {
            if (currentGrabMovingSide == InputDeviceRole.LeftHanded)
            {
                Vector3 leftControllerPosition = leftController.transform.position;
                translationVector = currentGrabMovingPoint - leftControllerPosition;

            }
            else if (currentGrabMovingSide == InputDeviceRole.RightHanded)
            {
                Vector3 rightControllerPosition = rightController.transform.position;
                translationVector = currentGrabMovingPoint - rightControllerPosition;
            }
        }

        //DebugDrawVR.DrawCapsule(bodyCollider.center + bodyCollider.transform.position, Quaternion.identity, bodyCollider.radius,
        //    bodyCollider.height, Color.magenta);

        //    DebugDrawVR.DrawCapsule(transform.position, Quaternion.identity, bodyCollider.radius,
        //bodyCollider.height, Color.red);

        //DebugDrawVR.DrawCapsule(rb.position, Quaternion.identity, bodyCollider.radius,
        //    bodyCollider.height, Color.green);
    }

    private void FixedUpdate()
    {
        bodyCollider.height = Mathf.Clamp(playerHead.localPosition.y, bodyHeightMin, bodyHeightMax);
        bodyCollider.center = new Vector3(playerHead.localPosition.x, bodyCollider.height / 2,
            playerHead.localPosition.z);

        if (isAirGrabbing && canAirGrab)
        {
            Vector3 targetPos = startingGrabOriginPosition + translationVector;
            Vector3 desiredVel = (targetPos - rb.position) / Time.fixedDeltaTime;
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, desiredVel, 0.5f);
        }
    }

    public void StartAirGrab(InputDeviceRole controllerSide, Vector3 grabMovingPoint)
    {
        if (!canAirGrab) return;

        if(isAirGrabbing)
        {
            // if we are requested to start air grab from left handed and we are already grabbing that means right hand
            // is doing air grab.
            if (controllerSide == InputDeviceRole.LeftHanded)
            {
                rightController.StopAirGrab();
            }
            // if we are requested to start air grab from right handed and we are already grabbing that means left hand
            // is doing air grab.
            else if (controllerSide == InputDeviceRole.RightHanded)
            {
                leftController.StopAirGrab();
            }
        }

        startingGrabOriginPosition = rb.position;
        currentGrabMovingPoint = grabMovingPoint;
        currentGrabMovingSide = controllerSide;
        isAirGrabbing = true;
    }

    public void StopAirGrab(InputDeviceRole controllerSide, Vector3 controllerVelocity)
    {
        if(controllerSide == currentGrabMovingSide && isAirGrabbing)
        {
            rb.AddForce(-controllerVelocity * grabEndForce, ForceMode.Impulse);
            isAirGrabbing = false;
        }
    }

    public void TeleportToPosition(Vector3 position)
    {
        isAirGrabbing = false;
        Vector3 bodyPosition = bodyCollider.center + bodyCollider.transform.position;
        Vector3 origin = transform.position;
        Vector3 offset = origin - bodyPosition;
        rb.MovePosition(position + new Vector3(offset.x,0, offset.z));
    }

    public void RotateAround(float amount)
    {
        cameraOffset.Rotate(Vector3.up, amount);
    }
}
