using UnityEngine;

public class InteractableMove : Interactable
{

    private float currentInteractDistance;
    private Vector3 interactionOffset;
    protected Vector3 newPosition;
    
    public override void Start()
    {
        
    }

    void Update()
    {
        if (state == InteractableState.IE_INTERACTING)
        {
            newPosition = interactionOffset +
                interactor.GetInteractPoint().position + interactor.GetInteractPoint().forward * currentInteractDistance;

            UpdatePosition();
        }
    }

    public virtual void UpdatePosition()
    {
        transform.position = newPosition;
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