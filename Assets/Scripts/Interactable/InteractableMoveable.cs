using UnityEngine;

public class InteractableMoveable : InteractableMove
{
    Moveable moveable;

    public override void Start()
    {
        moveable = GetComponent<Moveable>();
    }

    public override void UpdatePosition()
    {
        moveable.MoveTo(newPosition);
    }
}