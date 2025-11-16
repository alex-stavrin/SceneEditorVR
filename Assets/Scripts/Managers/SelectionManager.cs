using System;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; private set; }

    Interactable currentSelectable = null;

    public event Action<Interactable> OnSelected;

    public event Action OnUnSelected;

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SetCurrentSelectable(Interactable newSelectable, Controller instigator)
    {
        if (currentSelectable)
        {
            currentSelectable.StopSelect(instigator);

            if (currentSelectable.GetState() == InteractableState.IE_INTERACTING)
            {
                currentSelectable.ForceStopInteracting();
            }
        }

        currentSelectable = newSelectable;
        currentSelectable.StartSelect(instigator);

        InspectorManager.Instance.SetInspected(currentSelectable.gameObject);

        OnSelected.Invoke(currentSelectable);
    }

    public Interactable GetCurrentSelectable()
    {
        return currentSelectable;
    }

    public void UnselectCurrent()
    {
        if (currentSelectable)
        {
            currentSelectable.StopSelect(null);
            InspectorManager.Instance.SetInspected(null);

            OnUnSelected.Invoke();

            currentSelectable = null;
        }
    }
}
