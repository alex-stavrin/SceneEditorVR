using Unity.VisualScripting;
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

    InteractableArrow[] interactableArrows;

    InteractableRotator[] interactableRotators;

    InteractableScaler[] interactableScalers;

    Interactable currentInteractableSelected = null;

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
        SelectionManager.Instance.OnSelected += OnSelected;
        SelectionManager.Instance.OnUnSelected += OnUnSelected;
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

    void Update()
    {
        if(currentInteractableSelected)
        {
            gizmos.transform.position = currentInteractableSelected.transform.position;
        }
    }

    void OnGizmoTypeChanged(GizmoType gizmoType)
    {
        UpdateGizmoTypeVisual(gizmoType);
    }

    void OnSelected(Interactable interactable)
    {
        if (interactable)
        {
            gizmos.SetActive(true);
            gizmos.transform.position = interactable.transform.position;

            currentInteractableSelected = interactable;

            Moveable moveable = interactable.transform.root.GetComponent<Moveable>();
            if (moveable)
            {
                foreach (InteractableArrow interactableArrow in interactableArrows)
                {
                    interactableArrow.SetMoveable(moveable);
                    interactableArrow.SetInteractable(interactable);
                }
            }

            Rotateable rotateable = interactable.transform.root.GetComponent<Rotateable>();
            if (rotateable)
            {
                foreach (InteractableRotator interactableRotator in interactableRotators)
                {
                    interactableRotator.SetRotateable(rotateable);
                    interactableRotator.SetInteractable(interactable);
                }
            }
            
            Scaleable scaleable = interactable.transform.root.GetComponent<Scaleable>();
            if (scaleable)
            {
                foreach (InteractableScaler interactableScaler in interactableScalers)
                {
                    interactableScaler.SetScaleable(scaleable);
                    interactableScaler.SetInteractable(interactable);
                }
            }
        }
    }

    void OnUnSelected()
    {
        if (gizmos)
        {

            foreach(InteractableArrow interactableArrow in interactableArrows)
            {
                if(interactableArrow.GetState() == InteractableState.IE_INTERACTING)
                {
                    interactableArrow.StopInteract(null);
                }
            }

            foreach(InteractableRotator interactableRotator in interactableRotators)
            {
                if(interactableRotator.GetState() == InteractableState.IE_INTERACTING)
                {
                    interactableRotator.StopInteract(null);
                }               
            }

            
            foreach(InteractableScaler interactableScaler in interactableScalers)
            {
                if(interactableScaler.GetState() == InteractableState.IE_INTERACTING)
                {
                    interactableScaler.StopInteract(null);
                }               
            }

            gizmos.SetActive(false);
            currentInteractableSelected = null;
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
