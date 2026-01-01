using System.Collections.Generic;
using UnityEngine;

public class ScaleableInfo
{
    public Scaleable scaleable;
    public Vector3 startingScale;
}

public class InteractableScaler : InteractableGizmo
{
    private List<ScaleableInfo> scaleableInfos = new List<ScaleableInfo>();

    Vector3 interactorStartingPosition;

    public void AddScaleable(Scaleable newScaleable)
    {
        ScaleableInfo newScaleableInfo = new ScaleableInfo();
        newScaleableInfo.scaleable = newScaleable;
        scaleableInfos.Add(newScaleableInfo);
    }

    public void ClearScaleables()
    {
        scaleableInfos.Clear();
    }

    public void Update()
    {
        if (state == InteractableState.IE_INTERACTING)
        {
            Vector3 interactorOffset = interactor.transform.position - interactorStartingPosition;
            
            Vector3 dragDirection = GetWorldVectorBasedOnAxis();
            if(SelectionManager.GetSelectedGameobjects().Count == 1)
            {
               dragDirection = GetLocalVectorBasedOnAxis(SelectionManager.GetSelectedGameobjects()[0].transform);
            }

            float dragMagnitude = Vector3.Dot(interactorOffset, dragDirection) * PlayerPreferencesManager.Instance.axisMultiplier;
            Vector3 localScaleAdjustment = Vector3.zero;

            switch (targetAxis)
            {
                case Axis.X:
                    localScaleAdjustment.x = dragMagnitude;
                    break;
                case Axis.Y:
                    localScaleAdjustment.y = dragMagnitude;
                    break;
                case Axis.Z:
                    localScaleAdjustment.z = dragMagnitude;
                    break;
                case Axis.All:
                    localScaleAdjustment = Vector3.one * dragMagnitude;
                    break;
            }

            foreach(ScaleableInfo scaleableInfo in scaleableInfos)
            {                
                scaleableInfo.scaleable.ScaleTo(scaleableInfo.startingScale + localScaleAdjustment);
            }
        }
    }

    public override void OnInteractStart(Controller controllerInteractor)
    {
        base.OnInteractStart(controllerInteractor);

        foreach(ScaleableInfo scaleableInfo in scaleableInfos)
        {
            scaleableInfo.startingScale = scaleableInfo.scaleable.transform.localScale;
        }

        interactorStartingPosition = interactor.transform.position;
    }

    public override void OnInteractStop(Controller controllerInteractor)
    {
        base.OnInteractStop(controllerInteractor);

        List<GameObject> gameObjects = new List<GameObject>();
        List<Vector3> newScales = new List<Vector3>();
        List<Vector3> oldScales = new List<Vector3>();
        foreach(ScaleableInfo scaleableInfo in scaleableInfos)
        {
            gameObjects.Add(scaleableInfo.scaleable.gameObject);
            newScales.Add(scaleableInfo.scaleable.transform.localScale);
            oldScales.Add(scaleableInfo.startingScale);
        }

        ScaleAction scaleAction = new ScaleAction(gameObjects, oldScales, newScales);
        ActionsManager.AddAction(scaleAction);
    }
}
