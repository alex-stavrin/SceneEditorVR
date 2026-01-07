using UnityEngine;

// This class will represent any object that can be placed in our world
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

    public void OnInteractableStartSelect()
    {
        actorCollider.enabled = false;
    }

    public void OnInteractableStopSelect()
    {
        actorCollider.enabled = true;
    }
}
