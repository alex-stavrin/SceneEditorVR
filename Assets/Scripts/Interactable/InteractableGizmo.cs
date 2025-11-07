using UnityEngine;

public class InteractableGizmo : Interactable
{
    Material arrowMaterial;

    Color startingColor;

    Interactable currentInteractable;

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
    
    public void SetInteractable(Interactable _interactable)
    {
        currentInteractable = _interactable;
    }

    public override void OnInteractStart()
    {
        base.OnInteractStart();

        if (arrowMaterial)
        {
            arrowMaterial.SetColor("_Color", ColorManager.Instance.arrowsHoverColor);
        }
    }

    public override void OnInteractStop()
    {
        base.OnInteractStop();

        if (arrowMaterial)
        {
            arrowMaterial.SetColor("_Color", startingColor);
        }       
    }

    public override void OnHoverStart()
    {
        base.OnHoverStart();

        if (arrowMaterial)
        {
            arrowMaterial.SetColor("_Color", ColorManager.Instance.arrowsHoverColor);
        }
    }

    public override void OnHoverStop()
    {
        base.OnHoverStop();

        if (arrowMaterial)
        {
            arrowMaterial.SetColor("_Color", startingColor);
        }
    }
}
