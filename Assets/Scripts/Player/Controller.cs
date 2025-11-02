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
    [SerializeField] SphereCollider interactPoint;

    [Header("Input")]
    [SerializeField] InputDeviceRole controllerSide;
    [SerializeField] InputActionProperty gripAction;
    [SerializeField] InputActionProperty thumbstickAction;
    [SerializeField] InputActionProperty northButtonAction;
    [SerializeField] InputActionProperty southButtonAction;
    [SerializeField] InputActionProperty triggerAction;
    [SerializeField] InputActionProperty settingsAction;
 
    [Header("Ray Interaction")]
    [SerializeField] float startingInteractMoveSpeed;
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

    Interactable currentInteractable;
    bool isInteracting = false;

    bool isTryingToTeleport = false;

    MeshRenderer interactPointMesh;

    Vector2 thumbstickInputValue;

    // the start of the rays of the controllers
    Vector3 lineStart;

    // snap turn
    float snapTurnTimer;

    // select
    Interactable currentHoverable;


    public void Start()
    {
        // reset any changes made in the editor
        rayLineRenderer.positionCount = 2;

        gripAction.action.started += GripStarted;
        gripAction.action.canceled += GripEnded;

        triggerAction.action.started += TriggerPressed;
        triggerAction.action.canceled += TriggerReleased;

        northButtonAction.action.started += NorthButtonPressed;
        northButtonAction.action.canceled += NorthButtonReleased;

        settingsAction.action.started += SettingsButtonPressed;

        interactPointMesh = interactPoint.gameObject.GetComponent<MeshRenderer>();

        snapTurnTimer = 0;

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
        lineStart = interactPoint.transform.position +
            interactPoint.transform.forward * interactPoint.radius * interactPoint.transform.localScale.x;
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

        if(!isInteracting && !isTryingToTeleport)
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

        Vector3 lineStart = interactPoint.transform.position +
            interactPoint.transform.forward * interactPoint.radius * interactPoint.transform.localScale.x;

        if (!isTryingToTeleport && thumbstickInputValue.y > 0.4f && !isInteracting)
        {
            isTryingToTeleport = true;
        }

        if (isTryingToTeleport && thumbstickInputValue.y <= 0.4f)
        {
            Teleport();
            isTryingToTeleport = false;
        }

        if (isTryingToTeleport && !isInteracting && PointingAtUI() == false)
        {
            rayLineRenderer.gameObject.SetActive(false);
            teleportLineRenderer.gameObject.SetActive(true);

            List<Vector3> points = new List<Vector3>();
            Vector3 launchVelocity = interactPoint.transform.forward * teleportLineRange;
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
            interactPointMesh.enabled = false;
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

    void GripStarted(InputAction.CallbackContext context)
    {
        player.StartAirGrab(controllerSide, transform.position);
    }

    void GripEnded(InputAction.CallbackContext context)
    {
        player.StopAirGrab(controllerSide, velocity);
    }

    void TriggerPressed(InputAction.CallbackContext context)
    {
        if (currentHoverable)
        {
            if (currentHoverable.interactImmediately)
            {
                VirtualRealityConsole.PrintMessage("Interact imm", PrintTypeVRC.Append);
                StartInteract(currentHoverable);
            }
            else
            {
                VirtualRealityConsole.PrintMessage("Select...", PrintTypeVRC.Append);
                if (currentHoverable.GetState() == InteractableState.IE_SELECTED)
                {
                    StartInteract(currentHoverable);
                }
                else
                {
                    SelectionManager.Instance?.SetCurrentSelectable(currentHoverable);
                }
            }
        }
        else
        {
            VirtualRealityConsole.PrintMessage("Unselect...", PrintTypeVRC.Append);
            SelectionManager.Instance?.UnselectCurrent();
        }
    }

    public void StartInteract(Interactable hoverable)
    {
        hoverable.StartInteract(this);
        currentInteractable = hoverable;
        currentHoverable = null;
    }

    void TriggerReleased(InputAction.CallbackContext context)
    {
        if (currentInteractable)
        {
            currentInteractable.StopInteract();

            currentInteractable = null;
        }
    }

    void DoRaycast()
    {
        bRaycastHit = Physics.Raycast(interactPoint.transform.position, interactPoint.transform.forward, out rayHitResult, rayRange);

        if(bRaycastHit)
        {
            rayLineRenderer.SetPosition(1, rayHitResult.point);
        }
        else
        {
            rayLineRenderer.SetPosition(1, GetMaxRayPosition());
        }

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
        if(!isInteracting)
        {
            if (bRaycastHit)
            {
                Interactable interactable = rayHitResult.collider.transform.GetComponent<Interactable>();

                if (interactable && rayHitResult.collider.gameObject.layer != LayerMask.NameToLayer("UI"))
                {
                    if (currentHoverable)
                    {
                        if (interactable != currentHoverable)
                        {
                            currentHoverable.StopHover();
                        }
                    }

                    currentHoverable = interactable;
                    interactable.StartHover();
                    foundHover = true;
                }

                // if the component is on the same object it may be in the root
                if (!interactable)
                {
                    interactable = rayHitResult.collider.transform.root.GetComponent<Interactable>();
                    if (interactable && rayHitResult.collider.gameObject.layer != LayerMask.NameToLayer("UI"))
                    {
                        if (currentHoverable)
                        {
                            if (interactable != currentHoverable)
                            {
                                currentHoverable.StopHover();
                            }
                        }

                        currentHoverable = interactable;
                        interactable.StartHover();
                        foundHover = true;
                    }
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
        if (controllerSide == InputDeviceRole.RightHanded)
        {
            RadialManagerGizmos.Instance.CallRadial(this);
        }
        else if (controllerSide == InputDeviceRole.LeftHanded)
        {
            RadialManagerActions.Instance.CallRadial(this);
        }
    }

    void NorthButtonReleased(InputAction.CallbackContext context)
    {
        if (controllerSide == InputDeviceRole.RightHanded)
        {
            RadialManagerGizmos.Instance.DismissRadial();
        }
        else if (controllerSide == InputDeviceRole.LeftHanded)
        {
            RadialManagerActions.Instance.DismissRadial();
        }
    }

    public Transform GetInteractPoint()
    {
        return interactPoint.transform;
    }

    public Vector3 GetMaxRayPosition()
    {
        return lineStart + interactPoint.transform.forward * rayRange;
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