using UnityEditor;
using UnityEngine;
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

        //    DebugDrawVR.DrawCapsule(bodyCollider.center + bodyCollider.transform.position, Quaternion.identity, bodyCollider.radius,
        //        bodyCollider.height, Color.magenta);

        //    DebugDrawVR.DrawCapsule(bodyCollider.center, Quaternion.identity, bodyCollider.radius,
        //bodyCollider.height, Color.green);

        //    DebugDrawVR.DrawCapsule(transform.position, Quaternion.identity, bodyCollider.radius,
        //bodyCollider.height, Color.red);
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
        rb.rotation = rb.rotation * Quaternion.AngleAxis(amount, Vector3.up);  
    }
}
