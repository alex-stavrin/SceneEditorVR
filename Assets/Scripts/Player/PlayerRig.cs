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

    [Header("Snap Turn")]
    [SerializeField]
    public float snapTurnAmount = 45f;
    [SerializeField]
    public float snapTurnCooldown = 0.5f;

    [Header("Locomotion")]
    [SerializeField]
    public float maxMoveSpeed = 10.0f;

    [SerializeField]
    public float acceleration = 5.0f;

    [Header("Air Grabbing")]

    [SerializeField]
    bool canAirGrab = true;
    [SerializeField]
    float grabEndForce = 100f;

    [Header("Restrictions")]

    [SerializeField]
    public bool menuRestricted = false;    

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

        // if(isAirGrabbing)
        // {
            // // if we are requested to start air grab from left handed and we are already grabbing that means right hand
            // // is doing air grab.
            // if (controllerSide == InputDeviceRole.LeftHanded)
            // {
            //     rightController.StopAirGrab();
            // }
            // // if we are requested to start air grab from right handed and we are already grabbing that means left hand
            // // is doing air grab.
            // else if (controllerSide == InputDeviceRole.RightHanded)
            // {
            //     leftController.StopAirGrab();
            // }
        // }

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

    public void RotateAround(float amount)
    {
        Quaternion newRotation = Quaternion.AngleAxis(amount, Vector3.up);

        rb.MoveRotation(rb.rotation * newRotation);

        Vector3 newPosition = newRotation * (rb.position - playerHead.position) + playerHead.position;

        rb.MovePosition(newPosition);
    }

    public void Move(Vector3 movementVector)
    {
        if (menuRestricted) return;

        Vector3 playerRight = playerHead.right;
        playerRight.y = 0;
        playerRight.Normalize();

        Vector3 playerForward = playerHead.forward;
        playerForward.y = 0;
        playerForward.Normalize();

        Vector3 targetDirection = (playerRight * movementVector.x) + (playerForward * movementVector.z);
        Vector3 targetVelocity = targetDirection * maxMoveSpeed;

        Vector3 currentVelocity = rb.linearVelocity;
        float maxSpeedChange = acceleration * Time.deltaTime;

        float newX = Mathf.MoveTowards(currentVelocity.x, targetVelocity.x, maxSpeedChange);
        float newZ = Mathf.MoveTowards(currentVelocity.z, targetVelocity.z, maxSpeedChange);

        rb.linearVelocity = new Vector3(newX, currentVelocity.y, newZ);
    }

    public Transform GetPlayerHead()
    {
        return playerHead;
    }
}
