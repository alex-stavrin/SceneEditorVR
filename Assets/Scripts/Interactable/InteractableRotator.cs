using Unity.Mathematics;
using UnityEngine;

public class InteractableRotator : InteractableGizmo
{
    private Rotateable rotateable;

    [SerializeField]
    Vector3 rotatingVector;

    Vector3 interactorStartingPosition;

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

            Vector3 controllerDirection = (interactorCurrentPosition - interactorStartingPosition).normalized;
            Vector3 projectedControllerDirection = Vector3.ProjectOnPlane(controllerDirection, rotatingVector);

            float angle = Vector3.SignedAngle(rotateableReferenceVector, projectedControllerDirection, rotatingVector);

            if (rotatingVector == new Vector3(0, 1, 0)) // y axis
            {
                float circleX = Mathf.Sin(angle * Mathf.Deg2Rad);
                float circleZ = Mathf.Cos(angle * Mathf.Deg2Rad);

                RotationVisualizerManager.Instance.SetTargetVisualizerLocation(interactorStartingPosition +
                    new Vector3(0.2f * circleX, 0, 0.2f * circleZ));

                rotateable.gameObject.transform.rotation = Quaternion.AngleAxis(angle - startingAngle, new Vector3(0,1,0)) * startingRotation;
            }
            else if (rotatingVector == new Vector3(1, 0, 0)) // x axis
            {
                float circleY = Mathf.Cos(angle * Mathf.Deg2Rad);
                float circleZ = Mathf.Sin(angle * Mathf.Deg2Rad);

                RotationVisualizerManager.Instance.SetTargetVisualizerLocation(interactorStartingPosition +
                    new Vector3(0, 0.2f * circleY, 0.2f * circleZ));

                rotateable.gameObject.transform.rotation = Quaternion.AngleAxis(angle - startingAngle, new Vector3(1,0,0)) * startingRotation;
            }
            else if (rotatingVector == new Vector3(0,0,1)) // z axis
            {
                float circleY = Mathf.Cos(angle * Mathf.Deg2Rad);
                float circleX = -Mathf.Sin(angle * Mathf.Deg2Rad);

                RotationVisualizerManager.Instance.SetTargetVisualizerLocation(interactorStartingPosition +
                    new Vector3(0.2f * circleX, circleY * 0.2f, 0));

                rotateable.gameObject.transform.rotation = Quaternion.AngleAxis(angle - startingAngle, new Vector3(0,0,1)) * startingRotation;
            }
        }
    }

    public override void OnInteractStart()
    {
        base.OnInteractStart();

        if (rotateable)
        {
            startingRotation = rotateable.transform.rotation;

            if (rotatingVector == new Vector3(0, 1, 0)) // y axis
            {
                startingAngle = startingRotation.eulerAngles.y;
                rotateableReferenceVector = new Vector3(0, 0, 1);
            }
            else if (rotatingVector == new Vector3(1, 0, 0)) // x axis
            {
                startingAngle = startingRotation.eulerAngles.x;
                rotateableReferenceVector = new Vector3(0, 1, 0);  
            }
            else if (rotatingVector == new Vector3(0,0,1)) // z axis
            {
                startingAngle = startingRotation.eulerAngles.z;
                rotateableReferenceVector = new Vector3(0, 1, 0);  
            }
        }

        interactorStartingPosition = interactor.gameObject.transform.position;

        RotationVisualizerManager.Instance.InitVisualizers(interactorStartingPosition);
    }

    public override void OnInteractStop()
    {
        base.OnInteractStop();

        RotationVisualizerManager.Instance.DismissVisualizers();
    }
}
