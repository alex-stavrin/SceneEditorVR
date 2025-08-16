using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using System.Collections.Generic;
using UnityEngine.AI;

public class Controller : MonoBehaviour
{
    /// PUBLIC ///

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
    [SerializeField] InputActionProperty settingsAction;
 
    [Header("Ray Interaction")]
    [SerializeField] float startingGrabMoveSpeed;
    [SerializeField] float rayRange;
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

    // Raycast global values
    [HideInInspector] public bool bRaycastHit;
    [HideInInspector] public RaycastHit rayHitResult;

    /// PRIVATE /// 

    Vector3 velocity;
    Vector3 lastPosition;

    Interactable currentGrabbable;
    bool isGrabbing = false;
    float grabDistance = 0;

    bool isTryingToTeleport = false;

    MeshRenderer grabPointMesh;

    Vector2 thumbstickInputValue;

    // the start of the rays of the controllers
    Vector3 lineStart;

    // snap turn
    float snapTurnTimer;

    // select
    Interactable currentHoverable;

    // grab move
    float currentGrabMoveSpeed;

    public void Start()
    {
        // reset any changes made in the editor
        rayLineRenderer.positionCount = 2;

        grabAction.action.started += GrabStarted;
        grabAction.action.canceled += GrabEnded;

        triggerAction.action.started += TriggerPressed;
        triggerAction.action.canceled += TriggerReleased;

        northButtonAction.action.started += NorthButtonPressed;

        settingsAction.action.started += SettingsButtonPressed;

        grabPointMesh = grabPoint.gameObject.GetComponent<MeshRenderer>();

        snapTurnTimer = 0;

        currentGrabMoveSpeed = startingGrabMoveSpeed;

        if (PlayerRig.Instance.canTeleport == false)
        {
            rectile.gameObject.SetActive(false);
            teleportLineRenderer.gameObject.SetActive(false);
        }
    }

    public void Update()
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

        DoRaycast();

        TeleportUpdate();
        SnapTurnUpdate();
        HoverTest();
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

        if (isTryingToTeleport && !isGrabbing && PointingAtUI() == false)
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
        if (currentHoverable)
        {
            // if(currentHoverable.GrabImmediately)
            // {
            //     StartGrab(currentHoverable, rayHitResult.distance, currentHoverable.transform.position - rayHitResult.point);
            // }
            // else
            // {
            //     if (currentHoverable.GetState() == InteractableState.IE_SELECTED)
            //     {
            //         StartGrab(currentHoverable, rayHitResult.distance, currentHoverable.transform.position - rayHitResult.point);
            //     }
            //     else
            //     {
            //         SelectionManager.Instance.SetCurrentSelectable(currentHoverable);
            //     }
            // }
        }
        else
        {
            SelectionManager.Instance.UnselectCurrent();
        }
    }
    
    void TriggerReleased(InputAction.CallbackContext context)
    {
        
    }

    void DoRaycast()
    {
        bRaycastHit = Physics.Raycast(grabPoint.transform.position, grabPoint.transform.forward, out rayHitResult, rayRange);

        if(bRaycastHit)
        {
            rayLineRenderer.SetPosition(1, rayHitResult.point);
        }
        else
        {
            rayLineRenderer.SetPosition(1, GetMaxRayPosition());
        }

    }

    public void StopAirGrab()
    {

    }

    public void SettingsButtonPressed(InputAction.CallbackContext context)
    {
        if(ControlPanel.Instance)
        {
            ControlPanel.Instance.GoToPlayer();
        }
    }
    
    void HoverTest()
    {
        bool foundHover = false;
        if(!isGrabbing)
        {
            if (bRaycastHit)
            {
                Interactable selectable = rayHitResult.collider.transform.root.GetComponent<Interactable>();

                if(selectable && rayHitResult.collider.gameObject.layer != LayerMask.NameToLayer("UI"))
                {
                    if(currentHoverable)
                    {
                        if(selectable != currentHoverable)
                        {
                            currentHoverable.StopHover();
                        }
                    }

                    currentHoverable = selectable;
                    selectable.StartHover();
                    foundHover = true;
                }
            }
        }

        if(!foundHover)
        {
            if(currentHoverable)
            {
                currentHoverable.StopHover();
                currentHoverable = null;
            }
        }
    }

    void NorthButtonPressed(InputAction.CallbackContext context)
    {

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

    public Vector2 GetThumbstickValue()
    {
        return thumbstickInputValue;
    }

    public bool PointingAtUI()
    {
        if(bRaycastHit)
        {
            return rayHitResult.collider.gameObject.layer == LayerMask.NameToLayer("UI");

        }
        else
        {
            return false;
        }
    }
}