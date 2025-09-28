using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; private set; }

    Interactable currentSelectable;



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
    }

    public void UnselectCurrent()
    {
        if(currentSelectable)
        {
            currentSelectable.StopSelect();
            InspectorManager.Instance.SetInspected(null);
        }
    }
}
