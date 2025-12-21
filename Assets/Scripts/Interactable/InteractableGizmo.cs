using UnityEngine;

public class InteractableGizmo : Interactable
{
    Material arrowMaterial;

    Color startingColor;


    public override void Start()
    {
        base.Start();

        MeshRenderer renderer = GetComponent<MeshRenderer>();
        if (renderer)
        {
            arrowMaterial = renderer.material;
            startingColor = arrowMaterial.GetColor("_Color");
        }
    }

    public override void OnInactiveStart(Controller controllerInteractor)
    {
        base.OnInactiveStart(controllerInteractor);

        arrowMaterial.SetColor("_Color", ColorManager.GetInactiveColor());
    }

    public override void OnInactiveStop(Controller controllerInteractor)
    {
        base.OnInactiveStop(controllerInteractor);

        arrowMaterial.SetColor("_Color", startingColor);
    }

    public override void OnInteractStart(Controller controllerInteractor)
    {
        base.OnInteractStart(controllerInteractor);



        if (arrowMaterial)
        {
            arrowMaterial.SetColor("_Color", ColorManager.GetArrowsHoverColor());
        }
    }

    public override void OnInteractStop(Controller controllerInteractor)
    {
        base.OnInteractStop(controllerInteractor);

        if (arrowMaterial)
        {
            arrowMaterial.SetColor("_Color", startingColor);
        }       
    }

    public override void OnHoverStart(Controller controllerInteractor)
    {
        base.OnHoverStart(controllerInteractor);

        HapticsManager.SendHaptic(controllerInteractor.GetSide(), HapticsManager.GetHoverAmplitude(), HapticsManager.GetHoverDuration());

        if (arrowMaterial)
        {
            arrowMaterial.SetColor("_Color", ColorManager.GetArrowsHoverColor());
        }
    }

    public override void OnHoverStop(Controller controllerInteractor)
    {
        base.OnHoverStop(controllerInteractor);

        if (arrowMaterial)
        {
            arrowMaterial.SetColor("_Color", startingColor);
        }
    }
}
