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

        interactableArrows = gizmos.GetComponentsInChildren<InteractableArrow>();
        interactableRotators = gizmos.GetComponentsInChildren<InteractableRotator>();
        interactableScalers = gizmos.GetComponentsInChildren<InteractableScaler>();
    }

    void OnSelected(Interactable interactable)
    {
        if (interactable)
        {
            gizmos.SetActive(true);
            gizmos.transform.position = interactable.transform.position;
            gizmos.transform.SetParent(interactable.transform);
            Moveable moveable = interactable.gameObject.GetComponent<Moveable>();
            if (moveable)
            {

            }
        }
    }

    void OnUnSelected()
    {
        if (gizmos)
        {

        }
    }

    void UpdateGizmos()
    {
        SetArrows(false);
        SetRotators(false);
        SetScalers(false);

        switch (PlayerPreferencesManager.Instance.currentGizmoType)
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
                interactableArrow.gameObject.SetActive(true);
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
