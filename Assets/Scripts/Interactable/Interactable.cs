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

    InteractableState state;

    Controller currentInteractor = null;

    public event Action OnStartSelect;
    public event Action OnStopSelect;

    private void Start()
    {
        SetState(InteractableState.IE_IDLE);
    }

    public void StartInteract(Controller interactor)
    {
        currentInteractor = interactor;
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

    public void ForceStopInteracting()
    {
        StopInteract();
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

    public virtual void OnHoverStart()
    {

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
        if (state == InteractableState.IE_SELECTED && newState != InteractableState.IE_SELECTED)
        {
            OnStopSelect.Invoke();
        }
        
        state = newState;
        switch (state) 
        {
            case InteractableState.IE_IDLE:
                break;
            case InteractableState.IE_HOVERED:
                OnHoverStart();
                break;
            case InteractableState.IE_SELECTED:
                OnStartSelect.Invoke();
                break;
            case InteractableState.IE_INTERACTING:
                break;
        }
    }
}
