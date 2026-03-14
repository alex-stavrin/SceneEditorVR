using UnityEngine;
using UnityEngine.EventSystems;

public class ModalBackdrop : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        ModalManager.CloseModal();
    }
}
