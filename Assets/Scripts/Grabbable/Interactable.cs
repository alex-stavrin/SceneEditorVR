using UnityEngine;

public enum InteractableState
{
    IE_IDLE,
    IE_HOVERED,
    IE_SELECTED,
    IE_GRABBED
}

public class Interactable : MonoBehaviour
{
    [Header("World")]
    [SerializeField] GameObject recticle;

    [Header("Interaction")]
    [SerializeField] private bool grabImmediately = false;
    public bool GrabImmediately
    {
        get => grabImmediately;
        set => grabImmediately = value;
    }

    InteractableState state;
    Material rectileMaterial;

    Controller currentGrabber = null;
    float grabDistance;
    Vector3 grabOffset;

    private void Start()
    {
        state = InteractableState.IE_IDLE;
        if(recticle)
        {
            recticle.SetActive(false);
            
            rectileMaterial = recticle.GetComponent<Renderer>().material;
            rectileMaterial.SetColor("_Color", SelectionManager.Instance.GetHoverColor());
        }
    }

    private void Update()
    {
        if (state == InteractableState.IE_GRABBED)
        {
            if(currentGrabber)
            {
                Vector3 grabPointPos = currentGrabber.GetGrabPoint().position;
                Vector3 grabPointDirection = currentGrabber.GetGrabPoint().forward;
                //////
            }
        }
    }

    public void StartGrab(float grabDistance, Vector3 grabOffset)
    {
        state = InteractableState.IE_GRABBED;
    }

    public void StopGrab()
    {
        state = InteractableState.IE_SELECTED;
    }

    public void StartSelect()
    {
        rectileMaterial.SetColor("_Color", SelectionManager.Instance.GetSelectedColor());
        recticle.SetActive(true);
        state = InteractableState.IE_SELECTED;
    }

    public void StopSelect()
    {
        rectileMaterial.SetColor("_Color", SelectionManager.Instance.GetHoverColor());
        recticle.SetActive(false);
        state = InteractableState.IE_IDLE;
    }

    public void StartHover()
    {
        if (state != InteractableState.IE_IDLE) return;

        recticle.SetActive(true);
        state = InteractableState.IE_HOVERED;
    }

    public void StopHover()
    {
        if (state != InteractableState.IE_HOVERED) return;

        recticle.SetActive(false);
        state = InteractableState.IE_IDLE;
    }

    public InteractableState GetState()
    {
        return state;
    }
}
