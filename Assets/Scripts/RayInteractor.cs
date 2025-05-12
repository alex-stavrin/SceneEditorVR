using UnityEngine;
using UnityEngine.InputSystem;

public struct GrabbableInfo
{
    public bool exists;
    public RayGrabbable grabbableComponent;
    public float grabbableDistance;
}

[RequireComponent(typeof(LineRenderer))]
public class RayInteractor : MonoBehaviour
{
    LineRenderer lineRenderer;

    [SerializeField] float rayRange;
    [SerializeField] Transform rayStart;
    [SerializeField] InputActionProperty grabAction;
    [SerializeField] InputActionProperty thumbstickAction;
    [SerializeField] float grabbedMoveSpeed;
    [SerializeField] float grabbedRotationSpeed;

    GrabbableInfo potentialGrabbable;
    GrabbableInfo currentGrabbable;

    void Start()
    {
        
        lineRenderer = GetComponent<LineRenderer>();

        // reset any changes made in the editor
        lineRenderer.positionCount = 2;

        grabAction.action.started += GrabStarted;
        grabAction.action.canceled += GrabEnded;
    }

    void Update()
    {
        lineRenderer.SetPosition(0, rayStart.position);

        if(!IsGrabbing())
        {
            RaycastHit hit;
            if(Physics.Raycast(rayStart.position, rayStart.forward, out hit, rayRange))
            {
                lineRenderer.SetPosition(1, hit.point);

                RayGrabbable grabbable = hit.transform.gameObject.GetComponent<RayGrabbable>();
                if(grabbable)
                {
                    potentialGrabbable.exists = true;
                    potentialGrabbable.grabbableComponent = grabbable;
                    potentialGrabbable.grabbableDistance = hit.distance;

                    // add also the distance from the hit to the gameobject so that we grab it by its origin
                    //float DistanceBetweenHitAndHitable = Vector3.Distance(grabbable.gameObject.transform.position, hit.point);
                    //potentialGrabbable.grabbableDistance += DistanceBetweenHitAndHitable;
                }
                else
                {
                    potentialGrabbable.exists = false;
                }
            }
            else
            {
                potentialGrabbable.exists = false;
                Vector3 rayEnd = rayStart.position + rayStart.forward * rayRange;
                lineRenderer.SetPosition(1, rayEnd);
            }
        }
        else
        {

            Vector2 thumbstickInputValue = thumbstickAction.action.ReadValue<Vector2>();

            if(Mathf.Abs(thumbstickInputValue.y) > 0.5)
            {
                float newGrabDistance = currentGrabbable.grabbableDistance + thumbstickInputValue.y * Time.deltaTime * grabbedMoveSpeed;
                currentGrabbable.grabbableDistance = Mathf.Clamp(newGrabDistance, 0, rayRange);
            }

            if(Mathf.Abs(thumbstickInputValue.x) > 0.5)
            {
                float angleDelta = thumbstickInputValue.x * Time.deltaTime * grabbedRotationSpeed;
                currentGrabbable.grabbableComponent.gameObject.transform.Rotate(-Vector3.up, angleDelta);
            }

            Vector3 GrabbablePosition = rayStart.position + (rayStart.forward * currentGrabbable.grabbableDistance);
            currentGrabbable.grabbableComponent.UpdatePosition(GrabbablePosition);
            lineRenderer.SetPosition(1, GrabbablePosition);
        }

    }

    void GrabStarted(InputAction.CallbackContext context)
    {
        if (potentialGrabbable.exists)
        {
            if(potentialGrabbable.grabbableComponent.currentInteractor != null)
            {
                potentialGrabbable.grabbableComponent.currentInteractor.StopGrab();
            }

            currentGrabbable = potentialGrabbable;
            currentGrabbable.exists = true;
            currentGrabbable.grabbableComponent.currentInteractor = this;

            potentialGrabbable.exists = false;
        }
    }

    void GrabEnded(InputAction.CallbackContext context)
    {
        StopGrab();
    }

    public void StopGrab()
    {
        if (currentGrabbable.exists)
        {
            currentGrabbable.exists = false;
            currentGrabbable.grabbableComponent.currentInteractor = null;
            potentialGrabbable.exists = false;
        }
    }

    public bool IsGrabbing()
    {
        return currentGrabbable.exists;
    }
}