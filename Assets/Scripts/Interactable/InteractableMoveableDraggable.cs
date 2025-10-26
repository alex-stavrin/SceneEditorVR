using UnityEngine;

public class InteractableMoveableDraggable : InteractableMoveable
{
    public override void OnInteractStop()
    {
        base.OnInteractStop();

        Interactable interactable = gameObject.AddComponent<Interactable>();

        SelectionManager.Instance.SetCurrentSelectable(interactable);

        Destroy(this);
    }
}
