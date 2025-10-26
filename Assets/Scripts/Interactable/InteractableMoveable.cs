using UnityEngine;

public class InteractableMoveable : Interactable
{

    private float currentInteractDistance;
    private Vector3 interactionOffset;

    void Update()
    {
        if (state == InteractableState.IE_INTERACTING)
        {
            transform.position = interactionOffset +
                interactor.GetInteractPoint().position + interactor.GetInteractPoint().forward * currentInteractDistance;
        }
    }

    public override void OnInteractStart()
    {
        base.OnInteractStart();

        currentInteractDistance = Vector3.Distance(transform.position, interactor.GetInteractPoint().position);
        interactionOffset = transform.position - interactor.rayHitResult.point;
    }
}