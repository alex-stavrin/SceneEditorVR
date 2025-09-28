using System;
using UnityEngine;

public class InteractableMoveable : Interactable
{
    public void MoveTo(Vector3 newPosition)
    {
        SetPosition(newPosition);
    }

    public void MoveBy(Vector3 offset)
    {
        SetPosition(transform.position + offset);
    }

    // global
    private void SetPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }
}