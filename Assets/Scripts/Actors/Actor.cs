using UnityEngine;

// This class will represent any object that can be placed in our world
[RequireComponent(typeof(Interactable))]
public class Actor : MonoBehaviour
{

    [SerializeField]
    string actorName;

    private Interactable interactable;

    void Start()
    {
        interactable = GetComponent<Interactable>();

        interactable.OnStartSelect += OnInteractableStartSelect;
        interactable.OnStopSelect += OnInteractableStopSelect;
    }

    public void OnInteractableStartSelect()
    {
        
    }

    public void OnInteractableStopSelect()
    {
        
    }

    public string GetActorName()
    {
        return actorName;
    }
}