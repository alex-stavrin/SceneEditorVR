using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR;

public class ButtonHaptic : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
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

    public void OnPointerEnter(PointerEventData eventData)
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