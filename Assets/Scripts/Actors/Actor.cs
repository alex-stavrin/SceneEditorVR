using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class Actor : MonoBehaviour
{
    [SerializeField]
    GameObject arrows;

    Interactable interactable;

    void Start()
    {
        interactable = GetComponent<Interactable>();
        interactable.OnStartSelect += OnInteractableStartSelect;
        interactable.OnStopSelect += OnInteractableStopSelect;
        PlayerPreferencesManager.Instance.OnUseAxesChanged += OnUseAxesChanged;

        // on start actor is not selected
        UpdateArrows(false);
    }

    void OnInteractableStartSelect()
    {
        UpdateArrows(true);
    }

    void OnInteractableStopSelect()
    {
        UpdateArrows(false);
    }

    void OnUseAxesChanged(bool newUseAxes)
    {
        UpdateArrows(interactable.GetState() == InteractableState.IE_SELECTED);
    }

    void UpdateArrows(bool selectionState)
    {
        arrows.SetActive(PlayerPreferencesManager.Instance.useAxes && selectionState);
    }
}
