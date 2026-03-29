using System.Collections.Generic;
using UnityEngine;

public class RotateableInfo
{
    public Rotateable rotateable;
    public Quaternion startingRotation;
    public float startingAngle;
    public Vector3 startingPosition;
}

public class InteractableRotator : InteractableGizmo
{
    private List<RotateableInfo> rotateableInfos = new List<RotateableInfo>();

    Vector3 interactionStartingPosition;
    Vector3 rotateableReferenceVector;
    Vector3 interactionCenter;

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
            Vector3 interactorCurrentPosition = interactor.gameObject.transform.position;
            Vector3 controllerDirection = (interactorCurrentPosition - interactionStartingPosition).normalized;

            Vector3 rotatingVector = GetWorldVectorBasedOnAxis();
            Vector3 projectedControllerDirection = Vector3.ProjectOnPlane(controllerDirection, rotatingVector);

            float angle = Vector3.SignedAngle(rotateableReferenceVector, projectedControllerDirection, rotatingVector);
            float visRadius = RotationVisualizerManager.Instance.visualizationRadius;
                
            if(SelectionManager.GetSelectedGameobjects().Count == 1)
            {
                
                foreach(RotateableInfo rotateableInfo in rotateableInfos)
                {                
                    if (rotatingVector == new Vector3(0, 1, 0)) // y axis
                    {
                        float circleX = Mathf.Sin(angle * Mathf.Deg2Rad);
                        float circleZ = Mathf.Cos(angle * Mathf.Deg2Rad);


                        Vector3 targetVisualizerLocation =  interactionStartingPosition +
                            new Vector3(visRadius * circleX, 0, visRadius * circleZ);

                        RotationVisualizerManager.Instance.SetTargetVisualizerLocation(targetVisualizerLocation);
                        RotationVisualizerManager.UpdateLineEndWorldToLocal(targetVisualizerLocation);

                        rotateableInfo.rotateable.SetRotationAroundAxisAndPoint(interactionCenter,
                            new Vector3(0,1,0), rotateableInfo.startingRotation, rotateableInfo.startingPosition,
                            angle - rotateableInfo.startingAngle);                
                    }
                    else if (rotatingVector == new Vector3(1, 0, 0)) // x axis
                    {
                        float circleY = Mathf.Cos(angle * Mathf.Deg2Rad);
                        float circleZ = Mathf.Sin(angle * Mathf.Deg2Rad);

                        Vector3 targetVisualizerLocation = interactionStartingPosition +
                            new Vector3(0, visRadius * circleY, visRadius * circleZ);

                        RotationVisualizerManager.Instance.SetTargetVisualizerLocation(targetVisualizerLocation);
                        RotationVisualizerManager.UpdateLineEndWorldToLocal(targetVisualizerLocation);

                        rotateableInfo.rotateable.SetRotationAroundAxisAndPoint(interactionCenter,
                            new Vector3(1,0,0), rotateableInfo.startingRotation, rotateableInfo.startingPosition,
                            angle - rotateableInfo.startingAngle);
                    }
                    else if (rotatingVector == new Vector3(0,0,1)) // z axis
                    {
                        float circleY = Mathf.Cos(angle * Mathf.Deg2Rad);
                        float circleX = -Mathf.Sin(angle * Mathf.Deg2Rad);

                        Vector3 targetVisualizerLocation = interactionStartingPosition +
                            new Vector3(visRadius * circleX, circleY * visRadius, 0);

                        RotationVisualizerManager.Instance.SetTargetVisualizerLocation(targetVisualizerLocation);
                        RotationVisualizerManager.UpdateLineEndWorldToLocal(targetVisualizerLocation);

                        rotateableInfo.rotateable.SetRotationAroundAxisAndPoint(interactionCenter,
                            new Vector3(0,0,1), rotateableInfo.startingRotation, rotateableInfo.startingPosition,
                            angle - rotateableInfo.startingAngle);
                    }
                }
            }
            else
            {
                foreach(RotateableInfo rotateableInfo in rotateableInfos)
                {                
                    if (rotatingVector == new Vector3(0, 1, 0)) // y axis
                    {
                        float circleX = Mathf.Sin(angle * Mathf.Deg2Rad);
                        float circleZ = Mathf.Cos(angle * Mathf.Deg2Rad);

                        Vector3 targetVisualizerLocation = interactionStartingPosition +
                            new Vector3(visRadius * circleX, 0, visRadius * circleZ);

                        RotationVisualizerManager.Instance.SetTargetVisualizerLocation(targetVisualizerLocation);
                        RotationVisualizerManager.UpdateLineEndWorldToLocal(targetVisualizerLocation);

                        rotateableInfo.rotateable.SetRotationAroundAxisAndPoint(interactionCenter,
                            new Vector3(0,1,0), rotateableInfo.startingRotation, rotateableInfo.startingPosition,
                            angle);                
                    }
                    else if (rotatingVector == new Vector3(1, 0, 0)) // x axis
                    {
                        float circleY = Mathf.Cos(angle * Mathf.Deg2Rad);
                        float circleZ = Mathf.Sin(angle * Mathf.Deg2Rad);

                        Vector3 targetVisualizerLocation = interactionStartingPosition +
                            new Vector3(0, visRadius * circleY, visRadius * circleZ);

                        RotationVisualizerManager.Instance.SetTargetVisualizerLocation(targetVisualizerLocation);
                        RotationVisualizerManager.UpdateLineEndWorldToLocal(targetVisualizerLocation);

                        rotateableInfo.rotateable.SetRotationAroundAxisAndPoint(interactionCenter,
                            new Vector3(1,0,0), rotateableInfo.startingRotation, rotateableInfo.startingPosition,
                            angle);    
                    }
                    else if (rotatingVector == new Vector3(0,0,1)) // z axis
                    {
                        float circleY = Mathf.Cos(angle * Mathf.Deg2Rad);
                        float circleX = -Mathf.Sin(angle * Mathf.Deg2Rad);

                        Vector3 targetVisualizerLocation = interactionStartingPosition +
                            new Vector3(visRadius * circleX, circleY * visRadius, 0);

                        RotationVisualizerManager.Instance.SetTargetVisualizerLocation(targetVisualizerLocation);
                        RotationVisualizerManager.UpdateLineEndWorldToLocal(targetVisualizerLocation);

                        rotateableInfo.rotateable.SetRotationAroundAxisAndPoint(interactionCenter,
                            new Vector3(0,0,1), rotateableInfo.startingRotation, rotateableInfo.startingPosition,
                            angle);    
                    }
                }             
            }

        }
    }

    public override void OnInteractStart(Controller controllerInteractor)
    {
        base.OnInteractStart(controllerInteractor);

        Vector3 rotatingVector = GetWorldVectorBasedOnAxis();
        interactionCenter = GizmosManager.GetCenter();
        foreach(RotateableInfo rotateableInfo in rotateableInfos)
        {
            rotateableInfo.startingRotation = rotateableInfo.rotateable.transform.rotation;
            rotateableInfo.startingPosition = rotateableInfo.rotateable.transform.position;

            if (rotatingVector == new Vector3(0, 1, 0)) // y axis
            {
                // store the rotateable starting angle
                rotateableInfo.startingAngle = rotateableInfo.startingRotation.eulerAngles.y;
                rotateableReferenceVector = new Vector3(0, 0, 1);

                float circleX = Mathf.Sin(rotateableInfo.startingAngle * Mathf.Deg2Rad);
                float circleZ = Mathf.Cos(rotateableInfo.startingAngle * Mathf.Deg2Rad);

                if(SelectionManager.GetSelectedGameobjects().Count > 1)
                {
                    circleX = 0;
                    circleZ = 1;
                }

                Vector3 deltaVector = new Vector3(circleX, 0, circleZ).normalized * RotationVisualizerManager.Instance.visualizationRadius;

                interactionStartingPosition = interactor.gameObject.transform.position - deltaVector;

                RotationVisualizerManager.UpdateLineStartWorldToLocal(interactionStartingPosition);
                RotationVisualizerManager.Instance.InitVisualizers(interactionStartingPosition, rotatingVector);
            }
            else if (rotatingVector == new Vector3(1, 0, 0)) // x axis
            {
                rotateableInfo.startingAngle = rotateableInfo.startingRotation.eulerAngles.x;
                rotateableReferenceVector = new Vector3(0, 1, 0);

                float circleY = Mathf.Cos(rotateableInfo.startingAngle * Mathf.Deg2Rad);
                float circleZ = Mathf.Sin(rotateableInfo.startingAngle * Mathf.Deg2Rad);

                if(SelectionManager.GetSelectedGameobjects().Count > 1)
                {
                    circleY = 1;
                    circleZ = 0;
                }

                Vector3 deltaVector = new Vector3(0, circleY, circleZ).normalized * RotationVisualizerManager.Instance.visualizationRadius;

                interactionStartingPosition = interactor.gameObject.transform.position - deltaVector;

                RotationVisualizerManager.UpdateLineStartWorldToLocal(interactionStartingPosition); 
                RotationVisualizerManager.Instance.InitVisualizers(interactionStartingPosition, rotatingVector);
            }
            else if (rotatingVector == new Vector3(0,0,1)) // z axis
            {
                rotateableInfo.startingAngle = rotateableInfo.startingRotation.eulerAngles.z;
                rotateableReferenceVector = new Vector3(0, 1, 0);

                float circleY = Mathf.Cos(rotateableInfo.startingAngle * Mathf.Deg2Rad);
                float circleX = -Mathf.Sin(rotateableInfo.startingAngle * Mathf.Deg2Rad);

                if(SelectionManager.GetSelectedGameobjects().Count > 1)
                {
                    circleY = 1;
                    circleX = 0;
                }

                Vector3 deltaVector = new Vector3(circleX, circleY, 0).normalized * RotationVisualizerManager.Instance.visualizationRadius;

                interactionStartingPosition = interactor.gameObject.transform.position - deltaVector;

                RotationVisualizerManager.UpdateLineStartWorldToLocal(interactionStartingPosition);
                RotationVisualizerManager.Instance.InitVisualizers(interactionStartingPosition, rotatingVector);
            }
        }
    }

    // On interact stop add an action to the ActionManager that stores the rotation we did
    // so that we can do and undo it
    public override void OnInteractStop(Controller controllerInteractor)
    {
        base.OnInteractStop(controllerInteractor);

        RotationVisualizerManager.Instance.DismissVisualizers();

        List<GameObject> gameObjects = new List<GameObject>();
        List<Quaternion> oldRotations = new List<Quaternion>();
        List<Quaternion> newRotations = new List<Quaternion>();
        List<Vector3> oldPositions = new List<Vector3>();
        List<Vector3> newPositions = new List<Vector3>();

        foreach(RotateableInfo rotateableInfo in rotateableInfos)
        {
            gameObjects.Add(rotateableInfo.rotateable.gameObject);

            oldRotations.Add(rotateableInfo.startingRotation);
            newRotations.Add(rotateableInfo.rotateable.transform.rotation);

            oldPositions.Add(rotateableInfo.startingPosition);
            newPositions.Add(rotateableInfo.rotateable.transform.position);
        }

        RotateAction rotateAction = new RotateAction(gameObjects, oldRotations, newRotations, oldPositions, newPositions);
        ActionsManager.AddAction(rotateAction);
    }
}
