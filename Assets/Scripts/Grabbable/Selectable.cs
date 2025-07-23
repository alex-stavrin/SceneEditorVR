using UnityEngine;

public class Selectable : MonoBehaviour
{
    [SerializeField] GameObject recticle;

    bool isHovered;
    bool isSelected;

    Material rectileMaterial;

    private void Start()
    {
        if(recticle)
        {
            recticle.SetActive(false);
            
            rectileMaterial = recticle.GetComponent<Renderer>().material;
            rectileMaterial.SetColor("_Color", SelectionManager.Instance.GetHoverColor());
        }
    }

    public void StartSelect()
    {
        rectileMaterial.SetColor("_Color", SelectionManager.Instance.GetSelectedColor());
        isSelected = true;
        recticle.SetActive(true);
    }

    public void StopSelect()
    {
        rectileMaterial.SetColor("_Color", SelectionManager.Instance.GetHoverColor());
        isSelected = false;
        recticle.SetActive(false);
    }

    public void StartHover()
    {
        if (isSelected) return;

        recticle.SetActive(true);
    }

    public void StopHover()
    {
        if (isSelected) return;

        recticle.SetActive(false);
    }

    public bool IsHovered()
    {
        return isHovered;
    }

    public void SetSelected(bool selected)
    {
        isSelected = selected;
    }

    public bool GetSelected()
    {
        return isSelected;
    }
}
