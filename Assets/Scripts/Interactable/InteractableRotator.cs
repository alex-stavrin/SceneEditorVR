using UnityEngine;

public class InteractableRotator : InteractableGizmo
{
    private Rotateable rotateable;


    public void SetRotateable(Rotateable _rotateable)
    {
        rotateable = _rotateable;
    }
}
