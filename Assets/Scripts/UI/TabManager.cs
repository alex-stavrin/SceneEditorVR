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
                SetButtonColor(buttons[i], ColorManager.GetHighlightColor());
            }
            else
            {
                tabs[i].gameObject.SetActive(false);
                SetButtonColor(buttons[i], ColorManager.GetNeutralColor());
            }
        }
    }

    public void SetButtonColor(Button button, Color color)
    {
        ColorBlock cb = button.colors;
        cb.normalColor = color;
        cb.pressedColor = color * 0.8f;
        cb.highlightedColor = color * 0.9f;
        button.colors = cb;
    }
}
