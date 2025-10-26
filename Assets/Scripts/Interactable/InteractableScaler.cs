using UnityEngine;

public class InteractableScaler : InteractableGizmo
{
    private Scaleable scaleable;

    [SerializeField]
    Vector3 direction;

    Vector3 interactableScaleableStartingScale;

    Vector3 interactorStartingPosition;


    public void SetScaleable(Scaleable _scaleable)
    {
        scaleable = _scaleable;
    }

    public override void Update()
    {
        base.Update();

        if (state == InteractableState.IE_INTERACTING)
        {
            if (scaleable)
            {

                Vector3 interactorOffset = interactor.transform.position - interactorStartingPosition;

                Vector3 projectedOffset = Vector3.Dot(interactorOffset, direction) * direction;

                scaleable.ScaleTo(interactableScaleableStartingScale + projectedOffset * PlayerPreferencesManager.Instance.axisMultiplier);
            }
        }
    }

    public override void OnInteractStart()
    {
        base.OnInteractStart();

        interactableScaleableStartingScale = scaleable.transform.localScale;
        interactorStartingPosition = interactor.transform.position;
    }
}
