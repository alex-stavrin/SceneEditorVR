using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR;

public class ButtonHaptic : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{

    Button button;

    void Start()
    {
        button = GetComponent<Button>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!button) return;

        if(button.interactable)
        {
            if (eventData is PointerEventDataVR eventDataVR)
            {
                if(HapticsManager.Instance)
                {
                    InputDeviceRole deviceRole = eventDataVR.controller.GetSide();
                    HapticsManager.PlayHapticButtonClick(deviceRole);
                }
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!button) return;

        if(button.interactable)
        {
            if (eventData is PointerEventDataVR eventDataVR)
            {
                if(HapticsManager.Instance)
                {
                    InputDeviceRole deviceRole = eventDataVR.controller.GetSide();
                    HapticsManager.PlayHapticButtonHover(deviceRole);
                }
            }
        }
    }
}