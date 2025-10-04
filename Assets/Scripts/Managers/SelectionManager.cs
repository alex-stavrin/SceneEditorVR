using System;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; private set; }

    Interactable currentSelectable;

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
        DontDestroyOnLoad(gameObject);
    }

    public void SetCurrentSelectable(Interactable newSelectable)
    {
        if (currentSelectable)
        {
            currentSelectable.StopSelect();

            if (currentSelectable.GetState() == InteractableState.IE_INTERACTING)
            {
                currentSelectable.ForceStopInteracting();
            }
        }

        currentSelectable = newSelectable;
        currentSelectable.StartSelect();

        InspectorManager.Instance.SetInspected(currentSelectable.gameObject);

        OnSelected.Invoke(currentSelectable);
    }

    public void UnselectCurrent()
    {
        if (currentSelectable)
        {
            currentSelectable.StopSelect();
            InspectorManager.Instance.SetInspected(null);

            OnUnSelected.Invoke();
        }
    }
}
