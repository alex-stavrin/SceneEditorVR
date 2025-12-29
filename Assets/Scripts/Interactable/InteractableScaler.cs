using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScaleableInfo
{
    public Scaleable scaleable;
    public Vector3 startingScale;
}

public class InteractableScaler : InteractableGizmo
{
    private List<ScaleableInfo> scaleableInfos = new List<ScaleableInfo>();

    [SerializeField]
    Vector3 direction;

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
            foreach(ScaleableInfo scaleableInfo in scaleableInfos)
            {                
                Vector3 interactorOffset = interactor.transform.position - interactorStartingPosition;

                Vector3 projectedOffset = Vector3.Dot(interactorOffset, direction) * direction;

                scaleableInfo.scaleable.ScaleTo(scaleableInfo.startingScale + projectedOffset * PlayerPreferencesManager.Instance.axisMultiplier);
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
