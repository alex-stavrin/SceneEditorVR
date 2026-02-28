using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR;

public class ButtonHapticHover : MonoBehaviour, IPointerEnterHandler
{
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