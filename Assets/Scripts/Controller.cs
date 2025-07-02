using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Controller : MonoBehaviour
{

    [Header("World")]
    [SerializeField] PlayerRig player;
    [SerializeField] Camera playerCamera;
    [SerializeField] SphereCollider grabPoint;

    [Header("Input")]
    [SerializeField] InputDeviceRole controllerSide;
    [SerializeField] InputActionProperty grabAction;
    [SerializeField] InputActionProperty thumbstickAction;
    [SerializeField] InputActionProperty northButtonAction;
    [SerializeField] InputActionProperty southButtonAction;
    [SerializeField] InputActionProperty triggerAction;

    [Header("Ray Interaction")]
    [SerializeField] float startingGrabMoveSpeed;
    [SerializeField] float maxGrabMoveSpeed;
    [SerializeField] float grabMoveAcceleration;
    [SerializeField] float startingGrabRotationSpeed;
    [SerializeField] float maxGrabRotationSpeed;
    [SerializeField] float grabRotationAcceleration;
    [SerializeField] float rayRange;
    [SerializeField] float minDistanceToRotate;
    [SerializeField] LineRenderer rayLineRenderer;

    [Header("Teleport")]
    [SerializeField] LineRenderer teleportLineRenderer;
    [SerializeField] int maxTeleportLineResolution = 30;
    [SerializeField] float teleportLineRange = 10f;
    [SerializeField] float teleportGravityMultiplier = 0.5f;
    [SerializeField] float navMeshAssist = 1f;
    [SerializeField] Transform rectile;
    [SerializeField] bool drawDebugPoints = false;

    [Header("UI Interactions")]
    [SerializeField] public int pointerId;

    [Header("Snap Turn")]
    [SerializeField] float snapTurnAmount = 45f;
    [SerializeField] float snapTurnCooldown = 0.5f;

    Vector3 velocity;
    Vector3 lastPosition;
    bool isAirGrabbing = false;

    Grabbable currentGrabbable;
    bool isGrabbing = false;
    float grabDistance = 0;
    Vector3 grabOffset;

    bool isTryingToTeleport = false;

    MeshRenderer grabPointMesh;

    Vector2 thumbstickInputValue;

    // Raycast global values
    [HideInInspector] public bool bRaycastHit;
    [HideInInspector] public RaycastHit rayHitResult;

    // the start of the rays of the controllers
    Vector3 lineStart;

    // grab move movement (rotation and position)
    float currentGrabMoveSpeed;
    float currentGrabRotationSpeed;

    // snap turn
    float snapTurnTimer;

    // select
    Selectable currentSelectable;
    bool hasSelected = false;

    void Start()
    {
        // reset any changes made in the editor
        rayLineRenderer.positionCount = 2;

        grabAction.action.started += GrabStarted;
        grabAction.action.canceled += GrabEnded;

        triggerAction.action.started += TriggerPressed;
        triggerAction.action.canceled += TriggerReleased;

        northButtonAction.action.started += NorthButtonPressed;

        grabPointMesh = grabPoint.gameObject.GetComponent<MeshRenderer>();

        currentGrabMoveSpeed = startingGrabMoveSpeed;
        currentGrabRotationSpeed = startingGrabRotationSpeed;

        snapTurnTimer = 0;

        
    }

    void Update()
    {
        // calculate velocity
        Vector3 currentPosition = transform.position;
        Vector3 delta = currentPosition - lastPosition;
        velocity = delta / Time.deltaTime;
        lastPosition = currentPosition;

        // set ray to start
        lineStart = grabPoint.transform.position +
            grabPoint.transform.forward * grabPoint.radius * grabPoint.transform.localScale.x;
        rayLineRenderer.SetPosition(0, lineStart);

        // read thumbstick input
        thumbstickInputValue = thumbstickAction.action.ReadValue<Vector2>();

        if(currentGrabbable)
        {
            grabPointMesh.enabled = !isTryingToTeleport && grabDistance <= minDistanceToRotate
                && currentGrabbable.allowDirectGrab;
        }
        else
        {
            grabPointMesh.enabled = !isTryingToTeleport && grabDistance <= minDistanceToRotate;
        }

        DoRaycast();

        GrabUpdate();
        TeleportUpdate();
        SnapTurnUpdate();
    }

    void GrabUpdate()
    {
        if (isGrabbing)
        {
            if (currentGrabbable)
            {
                if (Mathf.Abs(thumbstickInputValue.y) > 0.5)
                {
                    if(thumbstickInputValue.y < -0.5f || currentGrabbable.limitOffset < currentGrabbable.stopGrabMoveThreshold || !(currentGrabbable.limitGrabMove))
                    {
                        if (currentGrabMoveSpeed < maxGrabMoveSpeed)
                        {
                            currentGrabMoveSpeed += grabMoveAcceleration * Time.deltaTime;
                        }

                        float newGrabDistance = grabDistance + thumbstickInputValue.y * Time.deltaTime * currentGrabMoveSpeed;
                        grabDistance = Mathf.Clamp(newGrabDistance, 0, rayRange);
                    }
                }
                else
                {
                    currentGrabMoveSpeed = startingGrabMoveSpeed;
                }

                if (Mathf.Abs(thumbstickInputValue.x) > 0.5)
                {
                    if (currentGrabRotationSpeed < maxGrabRotationSpeed)
                    {
                        currentGrabRotationSpeed += grabRotationAcceleration * Time.deltaTime;
                    }

                    float angleDelta = thumbstickInputValue.x * Time.deltaTime * currentGrabRotationSpeed;
                    currentGrabbable.transform.RotateAround(currentGrabbable.GetLineEndPoint(), -Vector3.up, angleDelta);

                }
                else
                {
                    currentGrabRotationSpeed = startingGrabRotationSpeed;

                }

                currentGrabbable.UpdateGrab(grabDistance, grabOffset, grabDistance < minDistanceToRotate);
            }
        }
    }

    void SnapTurnUpdate()
    {
        if(snapTurnTimer > 0)
        {
            snapTurnTimer -= Time.deltaTime;
        }

        if(!isGrabbing && !isTryingToTeleport)
        {
            if(Mathf.Abs( thumbstickInputValue.x) > 0.2 && snapTurnTimer <= 0)
            {
                int direction = thumbstickInputValue.x > 0 ? 1 : -1;

                player.RotateAround(snapTurnAmount * direction);
                snapTurnTimer = snapTurnCooldown;
            }
        }
    }
    void TeleportUpdate()
    {
        if (PlayerRig.Instance.canTeleport == false) return;

        Vector3 lineStart = grabPoint.transform.position +
            grabPoint.transform.forward * grabPoint.radius * grabPoint.transform.localScale.x;

        if (!isTryingToTeleport && thumbstickInputValue.y > 0.4f && !isGrabbing)
        {
            isTryingToTeleport = true;
        }

        if (isTryingToTeleport && thumbstickInputValue.y <= 0.4f)
        {
            Teleport();
            isTryingToTeleport = false;
        }

        if (isTryingToTeleport && !isGrabbing)
        {
            rayLineRenderer.gameObject.SetActive(false);
            teleportLineRenderer.gameObject.SetActive(true);

            List<Vector3> points = new List<Vector3>();
            Vector3 launchVelocity = grabPoint.transform.forward * teleportLineRange;
            points.Add(lineStart);
            bool foundPoint = false;
            for (int i = 1; i < maxTeleportLineResolution; i++)
            {
                Vector3 newPosition = points[i - 1] + launchVelocity / maxTeleportLineResolution;
                launchVelocity += Physics.gravity * teleportGravityMultiplier;
                points.Add(newPosition);

                // because we will have like 1000 points lets look for the navmesh on ones that are divisible 2
                // to save some perfomance
                if (!foundPoint)
                {
                    NavMeshHit navHit;
                    bool navRes = NavMesh.SamplePosition(points[i], out navHit, navMeshAssist, NavMesh.AllAreas);
                    if (navRes)
                    {
                        foundPoint = true;
                        rectile.position = navHit.position;
                        rectile.rotation = Quaternion.identity;
                        break;
                    }
                }
            }

            rectile.gameObject.SetActive(foundPoint);
            teleportLineRenderer.positionCount = points.Count;
            teleportLineRenderer.SetPositions(points.ToArray());
            grabPointMesh.enabled = false;
        }
        else
        {
            teleportLineRenderer.gameObject.SetActive(false);
            rayLineRenderer.gameObject.SetActive(true);
            rectile.gameObject.SetActive(false);
        }
    }

    void Teleport()
    {
        if(rectile.gameObject.activeSelf)
        {
            player.TeleportToPosition(rectile.position);
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
            List<Grabbable> grabbables = new List<Grabbable>();
            foreach (Collider hit in hits)
            {
                Grabbable grabbable = hit.transform.root.GetComponent<Grabbable>();
                if(grabbable)
                {
                    grabbables.Add(grabbable);
                }
            }

            if(grabbables.Count > 0)
            {
                Grabbable minGrabbableDirect = grabbables[0];
                float minDistance = Vector3.Distance(minGrabbableDirect.transform.position, grabPoint.transform.position);
                foreach (Grabbable grabbable in grabbables)
                {
                    float distanceFromGrabPoint = Vector3.Distance(grabbable.transform.position, grabPoint.transform.position);
                    if(distanceFromGrabPoint < minDistance)
                    {
                        minDistance = distanceFromGrabPoint;
                        minGrabbableDirect = grabbable;
                    }
                }

                if(minGrabbableDirect)
                {
                    minGrabbableDirect.StopGrab();
                }

                StartGrab(minGrabbableDirect, 0, minGrabbableDirect.transform.position - grabPoint.transform.position);
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
            if (bRaycastHit)
            {
                Grabbable grabbable = rayHitResult.collider.transform.root.GetComponent<Grabbable>();
                Canvas canvas = rayHitResult.collider.transform.GetComponent<Canvas>();
                if (grabbable && !canvas)
                {
                    StartGrab(grabbable, rayHitResult.distance, grabbable.transform.position - rayHitResult.point);
                }
            }
        }
    }
    
    void TriggerReleased(InputAction.CallbackContext context)
    {
        StopGrab();
    }

    void DoRaycast()
    {
        bRaycastHit = Physics.Raycast(grabPoint.transform.position, grabPoint.transform.forward, out rayHitResult, rayRange);
        if(isGrabbing)
        {
            rayLineRenderer.SetPosition(1, currentGrabbable.GetLineEndPoint());
        }
        else
        {
            if(bRaycastHit)
            {
                rayLineRenderer.SetPosition(1, rayHitResult.point);
            }
            else
            {
                rayLineRenderer.SetPosition(1, GetMaxRayPosition());
            }
        }
    }

    public void StartGrab(Grabbable grabbable, float startGrabbingOffset, Vector3 startGrabOffset)
    {
        grabOffset = startGrabOffset;
        currentGrabbable = grabbable;
        isGrabbing = true;
        currentGrabbable.StartGrab(this);
        grabDistance = startGrabbingOffset;
    }

    public void StopGrab()
    {
        if (isGrabbing)
        {
            isGrabbing = false;
            currentGrabbable.StopGrab();
            currentGrabbable = null;
            grabDistance = 0;
        }
    }

    public void StopAirGrab()
    {
        isAirGrabbing = false;
    }

    void NorthButtonPressed(InputAction.CallbackContext context)
    {
        bool foundSelectable = false;
        if(bRaycastHit)
        {
            Selectable selectable = rayHitResult.collider.transform.root.gameObject.GetComponent<Selectable>();
            if(selectable && selectable != currentGrabbable)
            {
                foundSelectable = true;
                StartSelect(selectable);
            }
        }

        if(!foundSelectable)
        {
            StopSelect();
        }
    }

    void StartSelect(Selectable selectable)
    {
        if (currentSelectable)
        {
            currentSelectable.StopSelect();
        }

        selectable.StartSelect();
        currentSelectable = selectable;
        hasSelected = true;
    }

    void StopSelect()
    {
        if (currentSelectable)
        {
            currentSelectable.StopSelect();
        }

        currentSelectable = null;
        hasSelected = false;
    }

    public Transform GetGrabPoint()
    {
        return grabPoint.transform;
    }

    public Vector3 GetMaxRayPosition()
    {
        return lineStart + grabPoint.transform.forward * rayRange;
    }

    public Vector3 GetRayStart()
    {
        return lineStart;
    }

    public bool TriggerPressedThisFrame()
    {
        return triggerAction.action.WasPressedThisFrame();
    }

    public bool TriggerReleasedThisFrame()
    {
        return triggerAction.action.WasReleasedThisFrame();
    }

    public float GetRayRange()
    {
        return rayRange;
    }
}