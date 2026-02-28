using UnityEngine;

public enum Axis
{
    X,
    Y,
    Z,
    All, // this means we will get the vector (1,1,1)
    Zero, // this mean we will get the vector (0,0,0)
}

public class InteractableGizmo : Interactable
{
    Material arrowMaterial;

    Color startingColor;

    [SerializeField]
    protected Axis targetAxis;

    protected Vector3 GetLocalVectorBasedOnAxis(Transform interactableTransform)
    {
        if(targetAxis == Axis.X)
        {
            return interactableTransform.right;
        }
        else if(targetAxis == Axis.Y)
        {
            return interactableTransform.up;
        }
        else if(targetAxis == Axis.Z)
        {   
            return interactableTransform.forward;
        }
        else if(targetAxis == Axis.All)
        {
            return new Vector3(1,1,1);
        }
        else
        {
            return Vector3.zero;
        }
    }

    protected Vector3 GetWorldVectorBasedOnAxis()
    {
        if(targetAxis == Axis.X)
        {
            return new Vector3(1,0,0);
        }
        else if(targetAxis == Axis.Y)
        {
            return new Vector3(0,1,0);
        }
        else if(targetAxis == Axis.Z)
        {   
            return new Vector3(0,0,1);
        }
        else if(targetAxis == Axis.All)
        {
            return new Vector3(1,1,1);
        }
        else
        {
            return Vector3.zero;
        }
    }


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

        HapticsManager.PlayHapticGizmoHover(controllerInteractor.GetSide());

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
