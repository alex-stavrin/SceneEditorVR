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
    Vector3 grabOffset;

    private void Start()
    {
        if(recticle)
        {
            rectileMaterial = recticle.GetComponent<Renderer>().material;
        }

        // dont put this before getting access to rectile material
        SetState(InteractableState.IE_IDLE);
    }

    public void UpdateGrab(float distance)
    {
        if (state == InteractableState.IE_GRABBED)
        {
            if (currentGrabber)
            {
                Vector3 grabPointPos = currentGrabber.GetGrabPoint().position;
                Vector3 grabPointDirection = currentGrabber.GetGrabPoint().forward;

                transform.position = grabPointPos + grabPointDirection * distance + grabOffset;

                transform.rotation = currentGrabber.transform.rotation;
            }
        }
    }

    public void StartGrab(Controller grabber, Vector3 grabOffset)
    {
        currentGrabber = grabber;
        this.grabOffset = grabOffset;
        SetState(InteractableState.IE_GRABBED);
    }

    public void StopGrab()
    {
        if(!grabImmediately)
        {
            SetState(InteractableState.IE_SELECTED);
        }
        else
        {
            SetState(InteractableState.IE_IDLE);
        }
    }

    public void ForceStopGrab()
    {
        currentGrabber.StopGrab();
        StopGrab();
    }

    public void StartSelect()
    {
        SetState(InteractableState.IE_SELECTED);
    }

    public void StopSelect()
    {
        SetState(InteractableState.IE_IDLE);
    }

    public void StartHover()
    {
        if (state != InteractableState.IE_IDLE) return;

        SetState(InteractableState.IE_HOVERED);
    }

    public void StopHover()
    {
        if (state != InteractableState.IE_HOVERED) return;

        SetState(InteractableState.IE_IDLE);
    }

    public InteractableState GetState()
    {
        return state;
    }

    void SetState(InteractableState newState)
    {
        state = newState;
        switch (state) 
        {
            case InteractableState.IE_IDLE:
                rectileMaterial.SetColor("_Color", SelectionManager.Instance.GetHoverColor());
                recticle.SetActive(false);
                break;
            case InteractableState.IE_HOVERED:
                recticle.SetActive(true);
                break;
            case InteractableState.IE_SELECTED:
                recticle.SetActive(true);
                rectileMaterial.SetColor("_Color", SelectionManager.Instance.GetSelectedColor());
                break;
            case InteractableState.IE_GRABBED:
                break;
        }
    }
}
