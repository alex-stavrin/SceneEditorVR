using UnityEngine;

public class InteractableMoveable : Interactable
{
    [SerializeField]
    public bool allowDirect = true;
    
    public override void OnHoverStart()
    {
        base.OnHoverStart();
    }
}