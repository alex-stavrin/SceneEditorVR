using Unity.Mathematics;
using UnityEngine;

public class InteractableRotator : InteractableGizmo
{
    private Rotateable rotateable;

    [SerializeField]
    Vector3 rotatingVector;

    Vector3 interactionStartingPosition;

    Vector3 rotateableReferenceVector;

    float startingAngle = 0;
    Quaternion startingRotation;

    public void SetRotateable(Rotateable _rotateable)
    {
        rotateable = _rotateable;
    }

    void Update()
    {
        if(state == InteractableState.IE_INTERACTING)
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

                rotateable.gameObject.transform.rotation = Quaternion.AngleAxis(angle - startingAngle, new Vector3(0,1,0)) * startingRotation;
            }
            else if (rotatingVector == new Vector3(1, 0, 0)) // x axis
            {
                float circleY = Mathf.Cos(angle * Mathf.Deg2Rad);
                float circleZ = Mathf.Sin(angle * Mathf.Deg2Rad);

                RotationVisualizerManager.Instance.SetTargetVisualizerLocation(interactionStartingPosition +
                    new Vector3(0, visRadius * circleY, visRadius * circleZ));

                rotateable.gameObject.transform.rotation = Quaternion.AngleAxis(angle - startingAngle, new Vector3(1,0,0)) * startingRotation;
            }
            else if (rotatingVector == new Vector3(0,0,1)) // z axis
            {
                float circleY = Mathf.Cos(angle * Mathf.Deg2Rad);
                float circleX = -Mathf.Sin(angle * Mathf.Deg2Rad);

                RotationVisualizerManager.Instance.SetTargetVisualizerLocation(interactionStartingPosition +
                    new Vector3(visRadius * circleX, circleY * visRadius, 0));

                rotateable.gameObject.transform.rotation = Quaternion.AngleAxis(angle - startingAngle, new Vector3(0,0,1)) * startingRotation;
            }
        }
    }

    public override void OnInteractStart(Controller controllerInteractor)
    {
        base.OnInteractStart(controllerInteractor);

        if (rotateable)
        {
            startingRotation = rotateable.transform.rotation;

            if (rotatingVector == new Vector3(0, 1, 0)) // y axis
            {
                startingAngle = startingRotation.eulerAngles.y;
                rotateableReferenceVector = new Vector3(0, 0, 1);

                float circleX = Mathf.Sin(startingAngle * Mathf.Deg2Rad);
                float circleZ = Mathf.Cos(startingAngle * Mathf.Deg2Rad);

                Vector3 deltaVector = new Vector3(circleX, 0, circleZ).normalized * RotationVisualizerManager.Instance.visualizationRadius;

                interactionStartingPosition = interactor.gameObject.transform.position - deltaVector;

                RotationVisualizerManager.Instance.InitVisualizers(interactionStartingPosition);
            }
            else if (rotatingVector == new Vector3(1, 0, 0)) // x axis
            {
                startingAngle = startingRotation.eulerAngles.x;
                rotateableReferenceVector = new Vector3(0, 1, 0);

                float circleY = Mathf.Cos(startingAngle * Mathf.Deg2Rad);
                float circleZ = Mathf.Sin(startingAngle * Mathf.Deg2Rad);

                Vector3 deltaVector = new Vector3(0, circleY, circleZ).normalized * RotationVisualizerManager.Instance.visualizationRadius;

                interactionStartingPosition = interactor.gameObject.transform.position - deltaVector;

                RotationVisualizerManager.Instance.InitVisualizers(interactionStartingPosition);
            }
            else if (rotatingVector == new Vector3(0,0,1)) // z axis
            {
                startingAngle = startingRotation.eulerAngles.z;
                rotateableReferenceVector = new Vector3(0, 1, 0);

                float circleY = Mathf.Cos(startingAngle * Mathf.Deg2Rad);
                float circleX = -Mathf.Sin(startingAngle * Mathf.Deg2Rad);

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
