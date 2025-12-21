using System.Collections.Generic;
using UnityEngine;

public class RotateableInfo
{
    public Rotateable rotateable;
    public Vector3 startingScale;

    public Quaternion startingRotation;

    public float startingAngle;
}

public class InteractableRotator : InteractableGizmo
{
    private List<RotateableInfo> rotateableInfos = new List<RotateableInfo>();
    [SerializeField]
    Vector3 rotatingVector;
    Vector3 interactionStartingPosition;
    Vector3 rotateableReferenceVector;

    public void AddRotateable(Rotateable newRotateable)
    {
        RotateableInfo rotateableInfo = new RotateableInfo();
        rotateableInfo.rotateable = newRotateable;
        rotateableInfos.Add(rotateableInfo);
    }

    public void ClearRotateables()
    {
        rotateableInfos.Clear();
    }

    void Update()
    {
        if(state == InteractableState.IE_INTERACTING)
        {
            foreach(RotateableInfo rotateableInfo in rotateableInfos)
            {                
                Vector3 interactorCurrentPosition = interactor.gameObject.transform.position;

                Vector3 controllerDirection = (interactorCurrentPosition - interactionStartingPosition).normalized;
                Vector3 projectedControllerDirection = Vector3.ProjectOnPlane(controllerDirection, rotatingVector);

                float angle = Vector3.SignedAngle(rotateableReferenceVector, projectedControllerDirection, rotatingVector);

                float visRadius = RotationVisualizerManager.Instance.visualizationRadius;

                if (rotatingVector == new Vector3(0, 1, 0)) // y axis
                {
                    float circleX = Mathf.Sin(angle * Mathf.Deg2Rad);
                    float circleZ = Mathf.Cos(angle * Mathf.Deg2Rad);

                    RotationVisualizerManager.Instance.SetTargetVisualizerLocation(interactionStartingPosition +
                        new Vector3(visRadius * circleX, 0, visRadius * circleZ));

                    rotateableInfo.rotateable.SetRotationAroundAxis(new Vector3(0,1,0), rotateableInfo.startingRotation,
                     angle - rotateableInfo.startingAngle);
                }
                else if (rotatingVector == new Vector3(1, 0, 0)) // x axis
                {
                    float circleY = Mathf.Cos(angle * Mathf.Deg2Rad);
                    float circleZ = Mathf.Sin(angle * Mathf.Deg2Rad);

                    RotationVisualizerManager.Instance.SetTargetVisualizerLocation(interactionStartingPosition +
                        new Vector3(0, visRadius * circleY, visRadius * circleZ));

                    rotateableInfo.rotateable.SetRotationAroundAxis(new Vector3(1,0,0), rotateableInfo.startingRotation,
                     angle - rotateableInfo.startingAngle);
                }
                else if (rotatingVector == new Vector3(0,0,1)) // z axis
                {
                    float circleY = Mathf.Cos(angle * Mathf.Deg2Rad);
                    float circleX = -Mathf.Sin(angle * Mathf.Deg2Rad);

                    RotationVisualizerManager.Instance.SetTargetVisualizerLocation(interactionStartingPosition +
                        new Vector3(visRadius * circleX, circleY * visRadius, 0));

                    rotateableInfo.rotateable.SetRotationAroundAxis(new Vector3(0,0,1), rotateableInfo.startingRotation,
                     angle - rotateableInfo.startingAngle);
                }
            }
        }
    }

    public override void OnInteractStart(Controller controllerInteractor)
    {
        base.OnInteractStart(controllerInteractor);

        foreach(RotateableInfo rotateableInfo in rotateableInfos)
        {
            rotateableInfo.startingRotation = rotateableInfo.rotateable.transform.rotation;

            if (rotatingVector == new Vector3(0, 1, 0)) // y axis
            {
                rotateableInfo.startingAngle = rotateableInfo.startingRotation.eulerAngles.y;
                rotateableReferenceVector = new Vector3(0, 0, 1);

                float circleX = Mathf.Sin(rotateableInfo.startingAngle * Mathf.Deg2Rad);
                float circleZ = Mathf.Cos(rotateableInfo.startingAngle * Mathf.Deg2Rad);

                Vector3 deltaVector = new Vector3(circleX, 0, circleZ).normalized * RotationVisualizerManager.Instance.visualizationRadius;

                interactionStartingPosition = interactor.gameObject.transform.position - deltaVector;

                RotationVisualizerManager.Instance.InitVisualizers(interactionStartingPosition);
            }
            else if (rotatingVector == new Vector3(1, 0, 0)) // x axis
            {
                rotateableInfo.startingAngle = rotateableInfo.startingRotation.eulerAngles.x;
                rotateableReferenceVector = new Vector3(0, 1, 0);

                float circleY = Mathf.Cos(rotateableInfo.startingAngle * Mathf.Deg2Rad);
                float circleZ = Mathf.Sin(rotateableInfo.startingAngle * Mathf.Deg2Rad);

                Vector3 deltaVector = new Vector3(0, circleY, circleZ).normalized * RotationVisualizerManager.Instance.visualizationRadius;

                interactionStartingPosition = interactor.gameObject.transform.position - deltaVector;

                RotationVisualizerManager.Instance.InitVisualizers(interactionStartingPosition);
            }
            else if (rotatingVector == new Vector3(0,0,1)) // z axis
            {
                rotateableInfo.startingAngle = rotateableInfo.startingRotation.eulerAngles.z;
                rotateableReferenceVector = new Vector3(0, 1, 0);

                float circleY = Mathf.Cos(rotateableInfo.startingAngle * Mathf.Deg2Rad);
                float circleX = -Mathf.Sin(rotateableInfo.startingAngle * Mathf.Deg2Rad);

                Vector3 deltaVector = new Vector3(circleX, circleY, 0).normalized * RotationVisualizerManager.Instance.visualizationRadius;

                interactionStartingPosition = interactor.gameObject.transform.position - deltaVector;

                RotationVisualizerManager.Instance.InitVisualizers(interactionStartingPosition);
            }
        }
    }

    public override void OnInteractStop(Controller controllerInteractor)
    {
        base.OnInteractStop(controllerInteractor);

        RotationVisualizerManager.Instance.DismissVisualizers();
    }
}
