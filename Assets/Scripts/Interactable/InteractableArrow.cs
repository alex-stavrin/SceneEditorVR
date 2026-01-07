using System.Collections.Generic;
using UnityEngine;

// Helper class to store info about selected object that are Moveable
public class MoveableInfo
{
    public Moveable moveable;
    public Vector3 startingPosition;
}

public class InteractableArrow : InteractableGizmo
{

    private Vector3 interactorStartingPosition;

    private List<MoveableInfo> moveableInfos = new List<MoveableInfo>();

    public void  Update()
    {
        if (state == InteractableState.IE_INTERACTING)
        {
            Vector3 interactorOffset = interactor.transform.position - interactorStartingPosition;
            Vector3 direction = GetWorldVectorBasedOnAxis();
            if(moveableInfos.Count == 1)
            {
                direction = GetLocalVectorBasedOnAxis(moveableInfos[0].moveable.transform);
            }
            Vector3 projectedOffset = Vector3.Dot(interactorOffset, direction) * direction;
            foreach(MoveableInfo moveableInfo in moveableInfos)
            {
                Moveable currentMoveable = moveableInfo.moveable;
                if(currentMoveable)
                {                    
                    currentMoveable.MoveTo(moveableInfo.startingPosition + projectedOffset * PlayerPreferencesManager.Instance.axisMultiplier);
                }
            }
        }
    }

    public override void OnInteractStart(Controller controllerInteractor)
    {
        base.OnInteractStart(controllerInteractor);

        foreach(MoveableInfo mi in moveableInfos)
        {
            mi.startingPosition = mi.moveable.transform.position;
        }
        
        interactorStartingPosition = interactor.transform.position;
    }

    public override void OnInteractStop(Controller controllerInteractor)
    {
        base.OnInteractStop(controllerInteractor);

        List<GameObject> movedObjects = new List<GameObject>();
        List<Vector3> oldPositions = new List<Vector3>();
        List<Vector3> newPositions = new List<Vector3>();
        foreach(MoveableInfo mi in moveableInfos)
        {
            movedObjects.Add(mi.moveable.gameObject);
            oldPositions.Add(mi.startingPosition);
            newPositions.Add(mi.moveable.transform.position);
        }

        MoveAction moveAction = new MoveAction(movedObjects, oldPositions, newPositions);
        ActionsManager.AddAction(moveAction);
    }

    // === Helper classes for List<MoveableInfo> ===

    public void AddMoveable(Moveable newMoveable)
    {
        MoveableInfo newMoveableI = new MoveableInfo();
        newMoveableI.moveable = newMoveable;
        newMoveableI.startingPosition = newMoveable.transform.position;
        moveableInfos.Add(newMoveableI);
    }

    public void ClearMoveables()
    {
        moveableInfos.Clear();
    }
}