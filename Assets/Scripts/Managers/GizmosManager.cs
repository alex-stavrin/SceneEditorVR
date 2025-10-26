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

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
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
