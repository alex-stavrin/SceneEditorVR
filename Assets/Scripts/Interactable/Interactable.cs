using System;
using Unity.VisualScripting;
using UnityEngine;

public enum InteractableState
{
    IE_INACTIVE,
    IE_IDLE,
    IE_HOVERED,
    IE_SELECTED,
    IE_INTERACTING
}

public class Interactable : MonoBehaviour
{

    [Header("Interaction")]
    [SerializeField] public bool interactImmediately = false;

    protected InteractableState state = InteractableState.IE_IDLE;

    protected Controller interactor = null;

    public event Action OnStartSelect;
    public event Action OnStopSelect;
    public event Action OnStartHover;
    public event Action OnStopHover;
    public event Action<Interactable> OnStartInteract;
    public event Action OnStopInteract;

    public virtual void Start()
    {
        
    }

    public void ForceStopInteracting()
    {
        StopInteract(null);
    }

    // Functions below used to set the state mainly

    public void StartInactive(Controller controllerInteractor)
    {
        SetState(controllerInteractor, InteractableState.IE_INACTIVE);
    }

    public void StopInactive(Controller controllerInteractor)
    {
        if(state != InteractableState.IE_INACTIVE) return;

        SetState(controllerInteractor, InteractableState.IE_IDLE);
    }

    public void StartInteract(Controller controllerInteractor)
    {
        if(state == InteractableState.IE_INACTIVE) return;

        interactor = controllerInteractor;
        SetState(controllerInteractor, InteractableState.IE_INTERACTING);
    }

    public void StopInteract(Controller controllerInteractor)
    {
        if(state == InteractableState.IE_INACTIVE) return;

        if(!interactImmediately)
        {
            SetState(controllerInteractor, InteractableState.IE_SELECTED);
        }
        else
        {
            SetState(controllerInteractor, InteractableState.IE_IDLE);
        }
    }

    public void StartSelect(Controller controllerInteractor)
    {
        if(state == InteractableState.IE_INACTIVE) return;

        SetState(controllerInteractor, InteractableState.IE_SELECTED);
    }

    public void StopSelect(Controller controllerInteractor)
    {
        if(state == InteractableState.IE_INACTIVE) return;
        SetState(controllerInteractor, InteractableState.IE_IDLE);
    }

    public void StartHover(Controller controllerInteractor)
    {
        if(state == InteractableState.IE_INACTIVE) return;

        if (state != InteractableState.IE_IDLE) return;
        SetState(controllerInteractor, InteractableState.IE_HOVERED);
    }

    public void StopHover(Controller controllerInteractor)
    {
        if(state == InteractableState.IE_INACTIVE) return;
        if (state != InteractableState.IE_HOVERED) return;

        SetState(controllerInteractor, InteractableState.IE_IDLE);
    }

    // Events based on state changes

    public virtual void OnInactiveStart(Controller controllerInteractor)
    {
        
    }

    public virtual void OnInactiveStop(Controller controllerInteractor)
    {
        
    }


    public virtual void OnHoverStart(Controller controllerInteractor)
    {
        OnStartHover?.Invoke();
    }

    public virtual void OnHoverStop(Controller controllerInteractor)
    {
        OnStopHover?.Invoke();
    }

    public virtual void OnSelectStart(Controller controllerInteractor)
    {
        OnStartSelect?.Invoke();
    }

    public virtual void OnSelectStop(Controller controllerInteractor)
    {
        OnStopSelect?.Invoke();
    }

    public virtual void OnInteractStart(Controller controllerInteractor)
    {
        OnStartInteract?.Invoke(this);
    }

    public virtual void OnInteractStop(Controller controllerInteractor)
    {
        OnStopInteract?.Invoke();
    }

    public InteractableState GetState()
    {
        return state;
    }

    void SetState(Controller instigator, InteractableState newState)
    {
        InteractableState previousState = state;
        state = newState;

        if (previousState == InteractableState.IE_INACTIVE && newState != InteractableState.IE_INACTIVE)
        {
            OnInactiveStop(instigator);
        }

        if (previousState == InteractableState.IE_SELECTED && newState != InteractableState.IE_SELECTED)
        {
            OnSelectStop(instigator);
        }

        if (previousState == InteractableState.IE_HOVERED && newState != InteractableState.IE_HOVERED)
        {
            OnHoverStop(instigator);
        }

        if (previousState == InteractableState.IE_INTERACTING && newState != InteractableState.IE_INTERACTING)
        {
            OnInteractStop(instigator);
        }
        
        switch (state)
        {
            case InteractableState.IE_INACTIVE:
                OnInactiveStart(instigator);
                break;
            case InteractableState.IE_IDLE:
                break;
            case InteractableState.IE_HOVERED:
                OnHoverStart(instigator);
                break;
            case InteractableState.IE_SELECTED:
                OnSelectStart(instigator);
                break;
            case InteractableState.IE_INTERACTING:
                OnInteractStart(instigator);
                break;
        }
    }
}
