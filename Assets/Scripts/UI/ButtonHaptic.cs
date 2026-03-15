using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR;

[RequireComponent(typeof(Button))]
public class ButtonHaptic : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{

    Button button;

    void Start()
    {
        button = GetComponent<Button>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData is PointerEventDataVR eventDataVR)
        {
            if(HapticsManager.Instance && button.interactable)
            {
                InputDeviceRole deviceRole = eventDataVR.controller.GetSide();
                HapticsManager.PlayHapticButtonClick(deviceRole);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData is PointerEventDataVR eventDataVR)
        {
            if(HapticsManager.Instance && button.interactable)
            {
                InputDeviceRole deviceRole = eventDataVR.controller.GetSide();
                HapticsManager.PlayHapticButtonHover(deviceRole);
            }
        }
    }
}