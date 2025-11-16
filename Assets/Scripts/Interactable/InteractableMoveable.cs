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

    public override void OnInteractStart(Controller controllerInteractor)
    {
        base.OnInteractStart(controllerInteractor);

        currentInteractDistance = Vector3.Distance(transform.position, interactor.GetInteractPoint().position);
        interactionOffset = transform.position - interactor.rayHitResult.point;
    }

    public void AddDistanceOffset(float offset)
    {
        currentInteractDistance = Mathf.Clamp(currentInteractDistance + offset, 0, Mathf.Infinity);
    }
}