using System;
using UnityEngine;

public enum InteractableState
{
    IE_IDLE,
    IE_HOVERED,
    IE_SELECTED,
    IE_INTERACTING
}

public class Interactable : MonoBehaviour
{

    [Header("Interaction")]
    [SerializeField] public bool interactImmediately = false;

    protected InteractableState state;

    protected Controller interactor = null;

    public event Action OnStartSelect;
    public event Action OnStopSelect;
    public event Action OnStartHover;
    public event Action OnStopHover;
    public event Action OnStartInteract;
    public event Action OnStopInteract;

    public virtual void Start()
    {
        SetState(InteractableState.IE_IDLE);
    }

    public void ForceStopInteracting()
    {
        StopInteract();
    }

    // Functions below used to set the state mainly

    public void StartInteract(Controller _interactor)
    {
        interactor = _interactor;
        SetState(InteractableState.IE_INTERACTING);
    }

    public void StopInteract()
    {
        if(!interactImmediately)
        {
            SetState(InteractableState.IE_SELECTED);
        }
        else
        {
            SetState(InteractableState.IE_IDLE);
        }
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

    // Events based on state changes
    public virtual void OnHoverStart()
    {
        OnStartHover?.Invoke();
    }

    public virtual void OnHoverStop()
    {
        OnStopHover?.Invoke();
    }

    public virtual void OnSelectStart()
    {
        OnStartSelect?.Invoke();
    }

    public virtual void OnSelectStop()
    {
        OnStopSelect?.Invoke();
    }

    public virtual void OnInteractStart()
    {
        OnStartInteract?.Invoke();
    }

    public virtual void OnInteractStop()
    {
        OnStopInteract?.Invoke();
    }

    public InteractableState GetState()
    {
        return state;
    }

    void SetState(InteractableState newState)
    {
        InteractableState previousState = state;
        state = newState;

        if (previousState == InteractableState.IE_SELECTED && newState != InteractableState.IE_SELECTED)
        {
            OnSelectStop();
        }

        if (previousState == InteractableState.IE_HOVERED && newState != InteractableState.IE_HOVERED)
        {
            OnHoverStop();
        }

        if (previousState == InteractableState.IE_INTERACTING && newState != InteractableState.IE_INTERACTING)
        {
            OnInteractStop();
        }
        
        switch (state)
        {
            case InteractableState.IE_IDLE:
                break;
            case InteractableState.IE_HOVERED:
                OnHoverStart();
                break;
            case InteractableState.IE_SELECTED:
                OnSelectStart();
                break;
            case InteractableState.IE_INTERACTING:
                OnInteractStart();
                break;
        }
    }
}
