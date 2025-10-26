using UnityEngine;

public class InteractableArrow : InteractableGizmo
{
    Moveable moveable = null;

    [SerializeField]
    Vector3 direction;

    Vector3 interactableMoveableStartingPosition;

    Vector3 interactorStartingPosition;

    public void SetMoveable(Moveable _moveable)
    {
        moveable = _moveable;
    }

    public override void  Update()
    {
        base.Update();

        if (state == InteractableState.IE_INTERACTING)
        {
            if (moveable)
            {

                Vector3 interactorOffset = interactor.transform.position - interactorStartingPosition;

                Vector3 projectedOffset = Vector3.Dot(interactorOffset, direction) * direction;

                moveable.MoveTo(interactableMoveableStartingPosition + projectedOffset * PlayerPreferencesManager.Instance.axisMultiplier);
            }
        }
    }

    public override void OnInteractStart()
    {
        base.OnInteractStart();

        interactableMoveableStartingPosition = moveable.transform.position;
        interactorStartingPosition = interactor.transform.position;
    }
}