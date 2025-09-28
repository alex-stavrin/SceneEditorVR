using UnityEngine;

public class InteractableArrow : InteractableGizmo
{
    InteractableMoveable interactableMoveable;

    [SerializeField]
    Vector3 direction;

    Vector3 interactableMoveableStartingPosition;

    Vector3 interactorStartingPosition;

    public override void Start()
    {
        base.Start();

        interactableMoveable = transform.root.GetComponent<InteractableMoveable>();
    }

    void Update()
    {
        if (state == InteractableState.IE_INTERACTING)
        {
            if (interactableMoveable)
            {

                Vector3 interactorOffset = interactor.transform.position - interactorStartingPosition;

                Vector3 projectedOffset = Vector3.Dot(interactorOffset, direction) * direction;

                interactableMoveable.MoveTo(interactableMoveableStartingPosition + projectedOffset * PlayerPreferencesManager.Instance.axisMultiplier);
            }
        }
    }

    public override void OnInteractStart()
    {
        base.OnInteractStart();

        interactableMoveableStartingPosition = interactableMoveable.transform.position;

        interactorStartingPosition = interactor.transform.position;
    }
}