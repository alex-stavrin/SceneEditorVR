using UnityEngine;

[RequireComponent(typeof(Actor))]
public class InteractableMoveableDraggable : InteractableMoveable
{
    public override void OnInteractStop(Controller controllerInteractor)
    {
        base.OnInteractStop(controllerInteractor);
        
        Interactable interactable = gameObject.AddComponent<Interactable>();

        // pass select events of new Interactable to actor
        Actor ownerActor = GetComponent<Actor>();
        if (ownerActor)
        {
            interactable.OnStartSelect += ownerActor.OnInteractableStartSelect;
            interactable.OnStopSelect += ownerActor.OnInteractableStopSelect;
        }
        
        // this must after we pass the events
        SelectionManager.ReplaceSelectablesWithOne(interactable, controllerInteractor);

        Destroy(this);
    }
}
