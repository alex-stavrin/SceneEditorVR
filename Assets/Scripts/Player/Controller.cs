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

    [SerializeField] LayerMask uiLayer;

    [Header("UI Interactions")]
    [SerializeField] public int pointerId;



    // Raycast global values
    [HideInInspector] public bool bRaycastHit;
    [HideInInspector] public RaycastHit rayHitResult;

    [Header("Moveable Handling")]
    [SerializeField] float moveableMoveSpeed = 5f;

    /// PRIVATE ///

    // interacting
    Interactable currentInteractable;
    bool isInteracting = false;

    // input
    Vector2 thumbstickInputValue;

    // the start of the rays of the controllers
    Vector3 lineStart;

    // snap turn
    float snapTurnTimer;

    // select
    Interactable currentHoverable;

    // interaction
    bool isMovingMoveable = false;

    // ui
    public bool pointingAtUI = false;

    // multi select
    bool isTryingToMultiSelect = false;

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

        southButtonAction.action.started += SouthButtonPressed;
        southButtonAction.action.canceled += SouthButtonReleased;

        snapTurnTimer = 0;
    }

    public void Update()
    {
        // set ray to start
        lineStart = interactPoint.transform.position +
            interactPoint.transform.forward * interactPoint.radius * interactPoint.transform.localScale.x;
        rayLineRenderer.SetPosition(0, lineStart);

        // read thumbstick input
        thumbstickInputValue = thumbstickAction.action.ReadValue<Vector2>();
        
        DoRaycast();
        HoverTest();
        SnapTurnUpdate();
        MoveableHandling();
    }

    void FixedUpdate()
    {
        LocomotionUpdate();
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
        if(controllerSide != InputDeviceRole.RightHanded) return;

        if (pointingAtUI) return;

        if (snapTurnTimer > 0)
        {
            snapTurnTimer -= Time.deltaTime;
        }

        if (!isInteracting)
        {
            if (Mathf.Abs(thumbstickInputValue.x) > 0.2 && snapTurnTimer <= 0 && !isMovingMoveable)
            {
                int direction = thumbstickInputValue.x > 0 ? 1 : -1;

                player.RotateAround(PlayerRig.Instance.snapTurnAmount * direction);
                snapTurnTimer = PlayerRig.Instance.snapTurnCooldown;
            }
        }
    }

    void LocomotionUpdate()
    {
        if (isInteracting) return;

        if (controllerSide != InputDeviceRole.LeftHanded) return;

        if (pointingAtUI) return;

        if (thumbstickInputValue.magnitude < 0.1f) return;

        float thumbstickForward = thumbstickInputValue.y;
        float thumbstickRight = thumbstickInputValue.x;

        Vector3 movementDirection = new Vector3(thumbstickRight, 0.0f, thumbstickForward);
        movementDirection.Normalize();

        PlayerRig.Instance.Move(movementDirection);
    }

    void MoveableHandling()
    {
        isMovingMoveable = false;
        if(currentInteractable)
        {
            InteractableMoveable interactableMoveable = currentInteractable as InteractableMoveable;
            if (interactableMoveable)
            {
                if(Mathf.Abs(thumbstickInputValue.y) > 0.2)
                {
                    isMovingMoveable = true;
                    interactableMoveable.AddDistanceOffset(thumbstickInputValue.y * Time.deltaTime * moveableMoveSpeed);
                }

                if (Mathf.Abs(thumbstickInputValue.x) > 0.2 && snapTurnTimer <= 0)
                {
                    int direction = thumbstickInputValue.x > 0 ? 1 : -1;

                    interactableMoveable.rotateable.RotateAroundY(direction * PlayerPreferencesManager.Instance.snappingRotationAmount);
                    snapTurnTimer = PlayerRig.Instance.snapTurnCooldown;
                }
            }
        }
    }
    void GripStarted(InputAction.CallbackContext context)
    {
        if(controllerSide == InputDeviceRole.LeftHanded)
        {
            PlayerRig.SetGripLeft(true);
            
        }
        else
        {
            PlayerRig.SetGripRight(true);
        }
    }

    void GripEnded(InputAction.CallbackContext context)
    {
        if(controllerSide == InputDeviceRole.LeftHanded)
        {
            PlayerRig.SetGripLeft(false);
            
        }
        else
        {
            PlayerRig.SetGripRight(false);
        } 
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
                    HapticsManager.PlayHapticActorSelected(controllerSide);
                    SoundsManager.PlaySelect(currentHoverable.transform.position);
                    if(isTryingToMultiSelect)
                    {
                        ActionsManager.AddSelectableAction(currentHoverable, this);
                    }
                    else
                    {
                        ActionsManager.ReplaceSelectablesWithOneAction(currentHoverable, this);
                    }
                }
            }
        }
        else
        {
            ActionsManager.UnselectCurrentsAction();
        }
    }
    public void StartInteract(Interactable hoverable)
    {
        hoverable.StartInteract(this);
        isInteracting = true;
        currentInteractable = hoverable;
        currentHoverable = null;
    }
    void TriggerReleased(InputAction.CallbackContext context)
    {
        if (currentInteractable)
        {
            currentInteractable.StopInteract(this);
            isInteracting = false;
            currentInteractable = null;
        }
    }
    void DoRaycast()
    {
        bRaycastHit = Physics.Raycast(interactPoint.transform.position, interactPoint.transform.forward, out rayHitResult, rayRange, uiLayer);
        if(bRaycastHit) // we hit ui
        {
            SetRayGradientLastColor(ColorManager.Instance.rayUIColor);
            rayLineRenderer.SetPosition(1, rayHitResult.point);
            return;
        }

        if(!bRaycastHit)
        {            
            bRaycastHit = Physics.Raycast(interactPoint.transform.position, interactPoint.transform.forward, out rayHitResult, rayRange, gizmoLayer);
            if(bRaycastHit) // we hit gizmo
            {
                SetRayGradientLastColor(ColorManager.Instance.rayGizmoHit);
                rayLineRenderer.SetPosition(1, rayHitResult.point);
                return;
            }
            
            if(!bRaycastHit)
            {
                bRaycastHit = Physics.Raycast(interactPoint.transform.position, interactPoint.transform.forward, out rayHitResult, rayRange);            
            }
        }

        if(bRaycastHit) // we hit collider (probably interactable)
        {
            SetRayGradientLastColor(ColorManager.Instance.rayGizmoHit);
            rayLineRenderer.SetPosition(1, rayHitResult.point);
        }
        else // we didn't hit anything
        {
            SetRayGradientLastColor(ColorManager.Instance.rayNoHitColor);
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
        if(PlayerRig.Instance.menuRestricted == true) return;

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
        if(PlayerRig.Instance.menuRestricted == true) return;

        if (controllerSide == InputDeviceRole.RightHanded)
        {
            RadialManagerGizmos.Instance.DismissRadial();
        }
        else if (controllerSide == InputDeviceRole.LeftHanded)
        {
            RadialManagerActions.Instance.DismissRadial();
        }
    }

    void SouthButtonPressed(InputAction.CallbackContext context)
    {
        if(PlayerRig.Instance.menuRestricted == true) return;
        
        // order here is important
        SetRayGradientLastColor(ColorManager.GetRayMultiSelectColor());
        isTryingToMultiSelect = true;
    }

    void SouthButtonReleased(InputAction.CallbackContext context)
    {
        if(PlayerRig.Instance.menuRestricted == true) return;

        isTryingToMultiSelect = false;
    }

    void SetRayGradientLastColor(Color newColor)
    {
        // multi select color will have higher priority over everthing
        if(isTryingToMultiSelect) return;

        if (rayLineRenderer)
        {
            Gradient gradient = rayLineRenderer.colorGradient;
            GradientColorKey[] colorKeys = gradient.colorKeys;
            colorKeys[0].color = newColor;
            colorKeys[1].color = newColor;
            gradient.colorKeys = colorKeys;
            rayLineRenderer.colorGradient = gradient;
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