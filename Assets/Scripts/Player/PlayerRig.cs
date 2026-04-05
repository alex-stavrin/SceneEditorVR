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

    [SerializeField]
    public float verticalSpeed = 5.0f;

    [Header("Restrictions")]

    [SerializeField]
    public bool menuRestricted = false;    

    /// PRIVATE ///

    Rigidbody rb;

    /// PUBLIC ///
    
    private bool gripLeft = false;
    private bool gripRight = false;


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

    private void FixedUpdate()
    {
        bodyCollider.height = Mathf.Clamp(playerHead.localPosition.y, bodyHeightMin, bodyHeightMax);
        bodyCollider.center = new Vector3(playerHead.localPosition.x, bodyCollider.height / 2,
            playerHead.localPosition.z);

        HandleVerticalGrip();
    }

    private void HandleVerticalGrip()
    {
        if(menuRestricted) return;
        if(gripLeft && gripRight) return;

        float verticalForce = verticalSpeed;
        if(gripLeft)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, verticalForce, rb.linearVelocity.z);
        }
        else if (gripRight)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, -verticalForce, rb.linearVelocity.z);
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

    public static void SetGripLeft(bool value)
    {
        Instance.gripLeft = value;
    }

    public static void SetGripRight(bool value)
    {
        Instance.gripRight = value;
    }
}
