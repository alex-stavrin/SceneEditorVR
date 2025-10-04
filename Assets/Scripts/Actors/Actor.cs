using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class Actor : MonoBehaviour
{

    Interactable interactable;

    Collider actorCollider;

    void Start()
    {
        interactable = GetComponent<Interactable>();
        actorCollider = GetComponent<Collider>();

        interactable.OnStartSelect += OnInteractableStartSelect;
        interactable.OnStopSelect += OnInteractableStopSelect;
    }

    void OnInteractableStartSelect()
    {
        actorCollider.enabled = false;
    }

    void OnInteractableStopSelect()
    {
        actorCollider.enabled = true;
    }
}
