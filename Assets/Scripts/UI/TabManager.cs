using UnityEngine;
using UnityEngine.UI;

public class TabManager : MonoBehaviour
{
    [SerializeField]
    Transform[] tabs;

    [SerializeField]
    Button[] buttons;

    [SerializeField]
    int startingTab;

    [SerializeField]
    Color activeColor;

    [SerializeField]
    Color inactiveColor;

    void Start()
    {
        SetTab(startingTab);
    }

    public void SetTab(int tab)
    {
        for (int i = 0; i < tabs.Length; i++)
        {
            if (i == tab)
            {

                tabs[i].gameObject.SetActive(true);
                SetButtonPrimaryColor(buttons[i], activeColor);
            }
            else
            {
                tabs[i].gameObject.SetActive(false);
                SetButtonPrimaryColor(buttons[i], inactiveColor);
            }
        }
    }
    
    void SetButtonPrimaryColor(Button button, Color newPrimaryColor)
    {
        ColorBlock colorBlock = button.colors;

        colorBlock.normalColor = newPrimaryColor;

        button.colors = colorBlock;
    }
}
