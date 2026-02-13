using UnityEngine;

[RequireComponent(typeof(Moveable))]
public class InteractableMoveable : InteractableMove
{
    Moveable moveable;

    public bool isFirst = true;

    public override void Start()
    {
        base.Start();

        moveable = GetComponent<Moveable>();
    }

    public override void UpdatePosition()
    {
        base.UpdatePosition();

        moveable.MoveTo(newPosition);
    }

    public override void OnInteractStop(Controller controllerInteractor)
    {
        base.OnInteractStop(controllerInteractor);

        if(isFirst)
        {
            SelectionManager.ReplaceSelectableWithOne(this, controllerInteractor);   
            isFirst = false;
            canBeInteractedThroughState = false;
        }
    }
}