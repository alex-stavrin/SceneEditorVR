using Unity.Mathematics;
using UnityEngine;

public class InteractableRotator : InteractableGizmo
{
    private Rotateable rotateable;

    [SerializeField]
    Vector3 rotatingVector;

    Vector3 interactorStartingPosition;

    public void SetRotateable(Rotateable _rotateable)
    {
        rotateable = _rotateable;
    }

    void Update()
    {
        if(state == InteractableState.IE_INTERACTING)
        {            
            Vector3 interactorCurrentPosition = interactor.gameObject.transform.position;

            if(rotatingVector == new Vector3(0,1,0)) // Rotate around Y
            {
                Vector3 referenceVector = rotateable.gameObject.transform.right;

                Vector3 controllerDirection = (interactorCurrentPosition - interactorStartingPosition).normalized;
                Vector3 projectedControllerDirection = Vector3.ProjectOnPlane(controllerDirection, rotatingVector);

                float angle = Vector3.SignedAngle(projectedControllerDirection, referenceVector, rotatingVector);

                float circleX = Mathf.Cos(angle * Mathf.Deg2Rad);
                float circleZ = Mathf.Sin(angle * Mathf.Deg2Rad);

                RotationVisualizerManager.Instance.SetTargetVisualizerLocation(interactorStartingPosition +
                    new Vector3(0.25f * circleX, 0, 0.25f * circleZ));

                // Vector3 rotationEuler = rotateable.gameObject.transform.rotation.eulerAngles;
                // rotateable.gameObject.transform.rotation = Quaternion.Euler(new Vector3(rotationEuler.x, angle, rotationEuler.z));

                VirtualRealityConsole.PrintMessage(angle.ToString(), PrintTypeVRC.Clear);      
            }        
        }
    }

    public override void OnInteractStart()
    {
        base.OnInteractStart();

        interactorStartingPosition = interactor.gameObject.transform.position;

        RotationVisualizerManager.Instance.InitVisualizers(interactorStartingPosition);
    }

    public override void OnInteractStop()
    {
        base.OnInteractStop();

        RotationVisualizerManager.Instance.DismissVisualizers();
    }
}
