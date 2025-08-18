using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(Image))]
public class ToggleButton : MonoBehaviour, IPointerClickHandler
{
    public bool toggleState = false;

    private Image buttonImage;

    public virtual void Start()
    {
        buttonImage = GetComponent<Image>();
        if (buttonImage)
        {            
            UpdateUI();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        toggleState = !toggleState;
        UpdateUI();
        OnStateUpdated(toggleState);
    }

    public virtual void OnStateUpdated(bool newState)
    {

    }

    public void UpdateUI()
    {
        if (toggleState)
        {
            buttonImage.color = Color.green;
        }
        else
        {
            buttonImage.color = Color.red;
        }
    }
}
