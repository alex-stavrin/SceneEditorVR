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
        SaveAndLoadManager.OpenLevel(levelName, true);
    }
}
