using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class KeyboardButtonVR : MonoBehaviour, IPointerDownHandler
{
    KeyboardVR keyboard;
    TextMeshProUGUI childText;

    private void Awake()
    {
        keyboard = GetComponentInParent<KeyboardVR>();
        if(keyboard)
        {
            childText = gameObject.GetComponentInChildren<TextMeshProUGUI>();

            if (childText.text.All(char.IsLetter))
            {
                keyboard.CapsChanged += KeyboardCapsChanged;
            }
        }
    }
    public void OnPointerDown(PointerEventData pointerData)
    {
        if(keyboard)
        {
            if (childText)
            {
                keyboard.KeyPressed(childText.text);   
            }
        }
    }

    void KeyboardCapsChanged(bool newCaps)
    {
        if(newCaps)
        {
            childText.text = childText.text.ToUpper();
        }
        else
        {
            childText.text = childText.text.ToLower();
        }
    }
}
