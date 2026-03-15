using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TextMeshProUGUI))]
[RequireComponent(typeof(Button))]
public class LoadedLevelButton : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI levelNameText;

    string levelName;

    Button button;

    void Start()
    {
        button = GetComponent<Button>();
        if(button)
        {
            button.onClick.AddListener(OnButtonClicked);
        }
    }

    public void SetLevelName(string newLevelName)
    {
        levelNameText.text = newLevelName;
        levelName = newLevelName;
    }

    public void OnButtonClicked()
    {
        MenuManager.SetSelectedLoadedLevelName(this, levelName);
    }

    public void SetButtonColor(Color color)
    {
        ColorBlock cb = button.colors;
        cb.normalColor = color;
        cb.pressedColor = color * 0.8f;
        cb.highlightedColor = color * 0.9f;
        button.colors = cb;
    }
}
