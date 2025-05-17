using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class PlayerRig : MonoBehaviour
{
    [SerializeField] Transform playerHead;
    [SerializeField] CapsuleCollider bodyCollider;

    [SerializeField] Controller leftController;
    [SerializeField] Controller rightController;
    [SerializeField] Rigidbody rb;
    [SerializeField] float grabEndForce = 100f;

    public float bodyHeightMin = 0.5f;
    public float bodyHeightMax = 2;

    bool isAirGrabbing = false;
    Vector3 currentGrabMovingPoint;
    InputDeviceRole currentGrabMovingSide;
    Vector3 startingGrabOriginPosition;

    Vector3 translationVector;

    void Start()
    {
        rb.detectCollisions = false;
    }

    void Update()
    {
        if (isAirGrabbing)
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

        if (isAirGrabbing)
        {
            Vector3 targetPos = startingGrabOriginPosition + translationVector;
            Vector3 desiredVel = (targetPos - rb.position) / Time.fixedDeltaTime;
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, desiredVel, 0.5f);
        }
    }

    public void StartAirGrab(InputDeviceRole controllerSide, Vector3 grabMovingPoint)
    {
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
        if(controllerSide == currentGrabMovingSide)
        {
            rb.AddForce(-controllerVelocity * grabEndForce, ForceMode.Impulse);
            isAirGrabbing = false;
        }
    }
}
