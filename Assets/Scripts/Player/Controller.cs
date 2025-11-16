using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

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

    [SerializeField] LayerMask gizmoLayer;

    [Header("UI Interactions")]
    [SerializeField] public int pointerId;

    [Header("Snap Turn")]
    [SerializeField] float snapTurnAmount = 45f;
    [SerializeField] float snapTurnCooldown = 0.5f;

    // Raycast global values
    [HideInInspector] public bool bRaycastHit;
    [HideInInspector] public RaycastHit rayHitResult;

    [Header("Moveable Handling")]
    [SerializeField] float moveableMoveSpeed = 5f;

    /// PRIVATE /// 

    Vector3 velocity;
    Vector3 lastPosition;

    Interactable currentInteractable;
    bool isInteracting = false;

    Vector2 thumbstickInputValue;

    // the start of the rays of the controllers
    Vector3 lineStart;

    // snap turn
    float snapTurnTimer;

    // select
    Interactable currentHoverable;

    bool isMovingMoveable = false;

    public bool pointingAtUI = false;


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

        snapTurnTimer = 0;
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

        SnapTurnUpdate();

        HoverTest();

        MoveableHandling();
    }

    void OnDisable()
    {
        gripAction.action.started -= GripStarted;
        gripAction.action.canceled -= GripEnded;

        triggerAction.action.started -= TriggerPressed;
        triggerAction.action.canceled -= TriggerReleased;

        northButtonAction.action.started -= NorthButtonPressed;
        northButtonAction.action.canceled -= NorthButtonReleased;

        settingsAction.action.started -= SettingsButtonPressed;
    }

    void SnapTurnUpdate()
    {
        if (snapTurnTimer > 0)
        {
            snapTurnTimer -= Time.deltaTime;
        }

        if (!isInteracting)
        {
            if (Mathf.Abs(thumbstickInputValue.x) > 0.2 && snapTurnTimer <= 0 && !isMovingMoveable)
            {
                int direction = thumbstickInputValue.x > 0 ? 1 : -1;

                player.RotateAround(snapTurnAmount * direction);
                snapTurnTimer = snapTurnCooldown;
            }
        }
    }
    
    void MoveableHandling()
    {
        isMovingMoveable = false;
        if(currentInteractable)
        {
            InteractableMoveable interactableMoveable = currentInteractable as InteractableMoveable;
            if (interactableMoveable)
            {
                if(Mathf.Abs(thumbstickInputValue.y) > 0.1)
                {
                    isMovingMoveable = true;
                    interactableMoveable.AddDistanceOffset(thumbstickInputValue.y * Time.deltaTime * moveableMoveSpeed);
                }
            }
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
                StartInteract(currentHoverable);
            }
            else
            {
                if (currentHoverable.GetState() == InteractableState.IE_SELECTED)
                {
                    StartInteract(currentHoverable);
                }
                else
                {
                    SelectionManager.Instance?.SetCurrentSelectable(currentHoverable, this);
                }
            }
        }
        else
        {
            if (SelectionManager.Instance == null) return;
            if (SelectionManager.Instance.GetCurrentSelectable() == null) return;
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
            currentInteractable.StopInteract(this);

            currentInteractable = null;
        }
    }

    void DoRaycast()
    {

        bRaycastHit = Physics.Raycast(interactPoint.transform.position, interactPoint.transform.forward, out rayHitResult, rayRange, gizmoLayer);

        if(!bRaycastHit)
        {
            bRaycastHit = Physics.Raycast(interactPoint.transform.position, interactPoint.transform.forward, out rayHitResult, rayRange);            
        }

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
        pointingAtUI = false;
        bool foundHover = false;
        if(!isInteracting)
        {
            if (bRaycastHit)
            {
                Interactable interactable = rayHitResult.collider.transform.GetComponent<Interactable>();

                if(rayHitResult.collider.gameObject.layer == LayerMask.NameToLayer("UI"))
                {
                    pointingAtUI = true;
                }
                else
                {
                    if (interactable)
                    {
                        if (currentHoverable)
                        {
                            if (interactable != currentHoverable)
                            {
                                currentHoverable.StopHover(this);
                            }
                        }

                        currentHoverable = interactable;
                        interactable.StartHover(this);
                        foundHover = true;
                    }
                    // if the component is on the same object it may be in the root
                    else
                    {
                        interactable = rayHitResult.collider.transform.root.GetComponent<Interactable>();
                        if (interactable && rayHitResult.collider.gameObject.layer != LayerMask.NameToLayer("UI"))
                        {
                            if (currentHoverable)
                            {
                                if (interactable != currentHoverable)
                                {
                                    currentHoverable.StopHover(this);
                                }
                            }

                            currentHoverable = interactable;
                            interactable.StartHover(this);
                            foundHover = true;
                        }
                    } 
                }
            }
        }

        if(!foundHover)
        {
            if(currentHoverable)
            {
                currentHoverable.StopHover(this);
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

    public InputDeviceRole GetSide()
    {
        return controllerSide;
    }
}