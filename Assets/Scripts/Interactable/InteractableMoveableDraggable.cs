using UnityEngine;

[RequireComponent(typeof(Actor))]
public class InteractableMoveableDraggable : InteractableMoveable
{
    public override void OnInteractStop()
    {
        base.OnInteractStop();
        
        Interactable interactable = gameObject.AddComponent<Interactable>();

        // pass select events of new Interactable to actor
        Actor ownerActor = GetComponent<Actor>();
        if (ownerActor)
        {
            interactable.OnStartSelect += ownerActor.OnInteractableStartSelect;
            interactable.OnStopSelect += ownerActor.OnInteractableStopSelect;
        }
        
        // this must after we pass the events
        SelectionManager.Instance.SetCurrentSelectable(interactable);

        Destroy(this);
    }
}
