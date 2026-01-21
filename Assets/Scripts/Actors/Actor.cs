using UnityEngine;

// This class will represent any object that can be placed in our world
[RequireComponent(typeof(Interactable))]
[RequireComponent(typeof(Outline))]
public class Actor : MonoBehaviour
{
    [SerializeField]
    string actorName;

    [SerializeField]
    string resourcesPath;

    private Interactable interactable;

    private Outline outline;

    void Awake()
    {  
        outline = GetComponent<Outline>();
        outline.enabled = false;

        Interactable localInteractable = GetComponent<Interactable>();
        SetInteractable(localInteractable);
    }

    public void SetInteractable(Interactable newInteractable)
    {
        interactable = newInteractable;
        
        interactable.OnStartSelect += OnInteractableStartSelect;
        interactable.OnStopSelect += OnInteractableStopSelect;

        interactable.OnStartHover += OnInteractableStartHover;
        interactable.OnStopHover += OnInteractableStopHover;
    }

    public void OnInteractableStartHover()
    {
        outline.enabled = true;
        outline.OutlineColor = Color.white;
    }

    public void OnInteractableStopHover()
    {
        outline.enabled = false;
    }

    public void OnInteractableStartSelect()
    {
        outline.enabled = true;
        outline.OutlineColor = ColorManager.GetArrowsHoverColor();
    }

    public void OnInteractableStopSelect()
    {
        outline.enabled = false;
    }

    public string GetActorName()
    {
        return actorName;
    }

    public string GetResourcesPath()
    {
        return resourcesPath;
    }

    public InteractableState GetInteractableState()
    {
        return interactable.GetState();
    }
}