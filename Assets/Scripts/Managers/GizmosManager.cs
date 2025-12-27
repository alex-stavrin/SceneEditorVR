using System.Collections.Generic;
using UnityEngine;

public enum GizmoType
{
    Arrows,
    Rotators,
    Scalers
}

public class GizmosManager : MonoBehaviour
{
    public static GizmosManager Instance { get; private set; }

    [SerializeField]
    GameObject gizmos;

    [SerializeField]
    float gizmosSizeMultiplier = 0.2f;

    InteractableArrow[] interactableArrows;

    InteractableRotator[] interactableRotators;

    InteractableScaler[] interactableScalers;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        SelectionManager.Instance.OnSelectedAdded += OnSelectedAdded;
        SelectionManager.Instance.OnUnSelected += OnUnSelected;
        SelectionManager.Instance.OnReplaced += OnSelectedReplaced;


        PlayerPreferencesManager.Instance.OnGizmoTypeChanged += OnGizmoTypeChanged;

        interactableArrows = gizmos.GetComponentsInChildren<InteractableArrow>();
        interactableRotators = gizmos.GetComponentsInChildren<InteractableRotator>();
        interactableScalers = gizmos.GetComponentsInChildren<InteractableScaler>();

        gizmos.SetActive(false);

        UpdateGizmoTypeVisual(PlayerPreferencesManager.Instance.currentGizmoType);

        foreach(InteractableArrow interactableArrow in interactableArrows)
        {
            interactableArrow.OnStartInteract += OnArrowStartInteract;
            interactableArrow.OnStopInteract += OnArrowStopInteract;
        }

        foreach(InteractableRotator interactableRotator in interactableRotators)
        {
            interactableRotator.OnStartInteract += OnRotatorStartInteract;
            interactableRotator.OnStopInteract += OnRotatorStopInteract;
        }

        foreach(InteractableScaler interactableScaler in interactableScalers)
        {
            interactableScaler.OnStartInteract += OnScalerStartInteract;
            interactableScaler.OnStopInteract += OnScalerStopInteract;
        }
    }

    void Update()
    {
        if(SelectionManager.GetSelectedInteractables().Count > 0)
        {
            Vector3 vectorsSum = new Vector3(0,0,0);
            foreach(Interactable interactable in SelectionManager.GetSelectedInteractables())
            {
                vectorsSum += interactable.transform.position;
            }
            Vector3 gizmoPosition = vectorsSum / SelectionManager.GetSelectedInteractables().Count;
            gizmos.transform.position = gizmoPosition;

            // scale gizmo size relative to player distance from gizmo
            Vector3 playerPosition = PlayerRig.Instance.gameObject.transform.position;
            float distance = Vector3.Distance(playerPosition, gizmos.transform.position);
            gizmos.transform.localScale = new Vector3(distance * gizmosSizeMultiplier,distance * gizmosSizeMultiplier,
                distance * gizmosSizeMultiplier);
        }
    }

    void OnArrowStartInteract(Interactable interactable)
    {
        foreach(InteractableArrow interactableArrow in interactableArrows)
        {
            if(interactable != interactableArrow)
            {
                interactableArrow.StartInactive(null);
            }
        }
    }

    void OnArrowStopInteract()
    {
        foreach(InteractableArrow interactableArrow in interactableArrows)
        {
            interactableArrow.StopInactive(null);
        }       
    }

    void OnRotatorStartInteract(Interactable interactable)
    {
        foreach(InteractableRotator interactableRotator in interactableRotators)
        {
            if(interactable != interactableRotator)
            {
                interactableRotator.StartInactive(null);
            }
        }       
    }

    void OnRotatorStopInteract()
    {
        foreach(InteractableRotator interactableRotator in interactableRotators)
        {
            interactableRotator.StopInactive(null);
        }           
    }

    void OnScalerStartInteract(Interactable interactable)
    {
        foreach(InteractableScaler interactableScaler in interactableScalers)
        {
            if(interactable != interactableScaler)
            {
                interactableScaler.StartInactive(null);
            }
        }         
    }

    void OnScalerStopInteract()
    {
        foreach(InteractableScaler interactableScaler in interactableScalers)
        {
            interactableScaler.StopInactive(null);
        }        
    }

    void OnGizmoTypeChanged(GizmoType gizmoType)
    {
        UpdateGizmoTypeVisual(gizmoType);
    }

    void OnSelectedAdded(Interactable newInteractable, List<Interactable> selected)
    {
        // first selected
        if(selected.Count == 1)
        {
            gizmos.SetActive(true);
        }

        Moveable moveable = newInteractable.transform.root.GetComponent<Moveable>();
        if (moveable)
        {
            foreach (InteractableArrow interactableArrow in interactableArrows)
            {
                interactableArrow.AddMoveable(moveable);
            }
        }

        Rotateable rotateable = newInteractable.transform.root.GetComponent<Rotateable>();
        if (rotateable)
        {
            foreach (InteractableRotator interactableRotator in interactableRotators)
            {
                interactableRotator.AddRotateable(rotateable);
            }
        }
        
        Scaleable scaleable = newInteractable.transform.root.GetComponent<Scaleable>();
        if (scaleable)
        {
            foreach (InteractableScaler interactableScaler in interactableScalers)
            {
                interactableScaler.AddScaleable(scaleable);
            }
        }
    }

    void OnSelectedReplaced(Interactable newInteractable)
    {
        Moveable moveable = newInteractable.transform.root.GetComponent<Moveable>();
        if (moveable)
        {
            foreach (InteractableArrow interactableArrow in interactableArrows)
            {
                interactableArrow.ClearMoveables();
                interactableArrow.AddMoveable(moveable);
            }
        }

        Scaleable scaleable = newInteractable.transform.root.GetComponent<Scaleable>();
        if (scaleable)
        {
            foreach (InteractableScaler interactableScaler in interactableScalers)
            {
                interactableScaler.ClearScaleables();
                interactableScaler.AddScaleable(scaleable);
            }
        }

        Rotateable rotateable = newInteractable.transform.root.GetComponent<Rotateable>();
        if (rotateable)
        {
            foreach (InteractableRotator interactableRotator in interactableRotators)
            {
                interactableRotator.ClearRotateables();
                interactableRotator.AddRotateable(rotateable);
            }
        }       
    }

    void OnUnSelected()
    {
        if (gizmos)
        {
            foreach(InteractableArrow interactableArrow in interactableArrows)
            {
                interactableArrow.StopInteract(null);
                interactableArrow.ClearMoveables();
            }

            foreach(InteractableRotator interactableRotator in interactableRotators)
            {
                interactableRotator.StopInteract(null);
                interactableRotator.ClearRotateables();
            }

            
            foreach(InteractableScaler interactableScaler in interactableScalers)
            {
                interactableScaler.StopInteract(null);  
                interactableScaler.ClearScaleables();       
            }

            gizmos.SetActive(false);
        }
    }

    void UpdateGizmoTypeVisual(GizmoType gizmoType)
    {
        SetArrows(false);
        SetRotators(false);
        SetScalers(false);

        switch (gizmoType)
        {
            case GizmoType.Arrows:
                SetArrows(true);
                break;
            case GizmoType.Rotators:
                SetRotators(true);
                break;
            case GizmoType.Scalers:
                SetScalers(true);
                break;
        }

    }

    void SetArrows(bool bActive)
    {
        foreach (InteractableArrow interactableArrow in interactableArrows)
        {
            if (interactableArrow)
            {
                interactableArrow.gameObject.SetActive(bActive);
            }
        }
    }

    void SetRotators(bool bActive)
    {
        foreach (InteractableRotator interactableRotator in interactableRotators)
        {
            if (interactableRotator)
            {
                interactableRotator.gameObject.SetActive(bActive);
            }
        }
    }

    void SetScalers(bool bActive)
    {
        foreach (InteractableScaler interactableScaler in interactableScalers)
        {
            if (interactableScaler)
            {
                interactableScaler.gameObject.SetActive(bActive);
            }
        }       
    }
}
