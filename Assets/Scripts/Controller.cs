
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class Controller : MonoBehaviour
{

    [Header("World")]
    [SerializeField] PlayerRig player;
    [SerializeField] SphereCollider grabPoint;

    [Header("Input")]
    [SerializeField] InputDeviceRole controllerSide;
    [SerializeField] InputActionProperty grabAction;
    [SerializeField] InputActionProperty thumbstickAction;
    [SerializeField] InputActionProperty northButtonAction;
    [SerializeField] InputActionProperty southButtonAction;
    [SerializeField] InputActionProperty triggerAction;

    [Header("Ray Interaction")]
    [SerializeField] float grabbedMoveSpeed;
    [SerializeField] float grabbedRotationSpeed;
    [SerializeField] float rayRange;
    [SerializeField] float minDistanceToRotate;

    LineRenderer lineRenderer;

    Vector3 velocity;
    Vector3 lastPosition;
    bool isAirGrabbing = false;

    Grabbable currentGrabbable;
    bool isGrabbing = false;
    float grabbingOffset = 0;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        // reset any changes made in the editor
        lineRenderer.positionCount = 2;

        grabAction.action.started += GrabStarted;
        grabAction.action.canceled += GrabEnded;

        triggerAction.action.started += TriggerPressed;
        triggerAction.action.canceled += TriggerReleased;
    }

    void Update()
    {

        // calculate velocity
        Vector3 currentPosition = transform.position;
        Vector3 delta = currentPosition - lastPosition;
        velocity = delta / Time.deltaTime;
        lastPosition = currentPosition;

        // set ray to start
        Vector3 lineStart = grabPoint.transform.position +
            grabPoint.transform.forward * grabPoint.radius * grabPoint.transform.localScale.x;
        lineRenderer.SetPosition(0, lineStart);

        if(isGrabbing)
        {
            if(currentGrabbable)
            {
                Vector2 thumbstickInputValue = thumbstickAction.action.ReadValue<Vector2>();

                if (Mathf.Abs(thumbstickInputValue.y) > 0.5)
                {
                    float newGrabDistance = grabbingOffset + thumbstickInputValue.y * Time.deltaTime * grabbedMoveSpeed;
                    grabbingOffset = Mathf.Clamp(newGrabDistance, 0, rayRange);
                }

                if (Mathf.Abs(thumbstickInputValue.x) > 0.5)
                {
                    float angleDelta = thumbstickInputValue.x * Time.deltaTime * grabbedRotationSpeed;
                    currentGrabbable.gameObject.transform.Rotate(-Vector3.up, angleDelta);
                }

                currentGrabbable.UpdateGrab(grabbingOffset, grabbingOffset < minDistanceToRotate);

                lineRenderer.SetPosition(1, currentGrabbable.transform.position);
            }
        }
        else
        {
            RaycastHit hit;
            if (Physics.Raycast(grabPoint.transform.position, grabPoint.transform.forward, out hit, rayRange))
            {
                lineRenderer.SetPosition(1, hit.point);
            }
            else
            {
                Vector3 rayEnd = grabPoint.transform.position + grabPoint.transform.forward * rayRange;
                lineRenderer.SetPosition(1, rayEnd);
            }

        }
    }

    void GrabStarted(InputAction.CallbackContext context)
    {
        player.StartAirGrab(controllerSide, transform.position);
    }

    void GrabEnded(InputAction.CallbackContext context)
    {
        player.StopAirGrab(controllerSide, velocity);
    }

    void TriggerPressed(InputAction.CallbackContext context)
    {
        bool shouldRayCast = false;

        Collider[] hits = Physics.OverlapSphere(grabPoint.transform.position, grabPoint.radius * grabPoint.transform.localScale.x);
        if (hits.Length > 0)
        {
            List<Grabbable> grabbableDirects = new List<Grabbable>();
            foreach (Collider hit in hits)
            {
                Grabbable grabbable = hit.GetComponent<Grabbable>();
                if(grabbable)
                {
                    grabbableDirects.Add(grabbable);
                }
            }

            if(grabbableDirects.Count > 0)
            {
                Grabbable minGrabbableDirect = grabbableDirects[0];
                float minDistance = Vector3.Distance(minGrabbableDirect.transform.position, grabPoint.transform.position);
                foreach (Grabbable grabbableDirect in grabbableDirects)
                {
                    float distanceFromGrabPoint = Vector3.Distance(grabbableDirect.transform.position, grabPoint.transform.position);
                    if(distanceFromGrabPoint < minDistance)
                    {
                        minDistance = distanceFromGrabPoint;
                        minGrabbableDirect = grabbableDirect;
                    }
                }

                if(minGrabbableDirect)
                {
                    minGrabbableDirect.StopGrab();
                }

                currentGrabbable = minGrabbableDirect;
                isGrabbing = true;
                currentGrabbable.StartGrab();
                grabbingOffset = 0;
                currentGrabbable.grabbingController = this;
                shouldRayCast = false;
            }
            else
            {
                shouldRayCast = true;
            }
        }
        else
        {
            shouldRayCast = true;
        }

        if(shouldRayCast)
        {
            // ray stuff
            Vector3 lineStart = grabPoint.transform.position +
                grabPoint.transform.forward * grabPoint.radius * grabPoint.transform.localScale.x;
            RaycastHit hitResult;
            bool hit = Physics.Raycast(lineStart, grabPoint.transform.forward, out hitResult, rayRange);
            if (hit)
            {
                Grabbable grabbable = hitResult.collider.gameObject.GetComponent<Grabbable>();
                if (grabbable)
                {
                    currentGrabbable = grabbable;
                    isGrabbing = true;
                    currentGrabbable.StartGrab();
                    grabbingOffset = hitResult.distance;
                    currentGrabbable.grabbingController = this;
                }
            }
        }
    }
    
    void TriggerReleased(InputAction.CallbackContext context)
    {
        StopGrab();
    }

    public void StopGrab()
    {
        if (isGrabbing)
        {
            isGrabbing = false;
            currentGrabbable = null;
        }
    }

    public void StopAirGrab()
    {
        isAirGrabbing = false;
    }

    public Transform GetGrabPoint()
    {
        return grabPoint.transform;
    }
}