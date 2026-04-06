using UnityEngine;

[RequireComponent(typeof(Moveable))]
[RequireComponent(typeof(Rotateable))]
public class InteractableMoveable : InteractableMove
{
    public Moveable moveable;

    public Rotateable rotateable;

    public bool isFirst = true;

    public override void Start()
    {
        base.Start();

        rotateable = GetComponent<Rotateable>();
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