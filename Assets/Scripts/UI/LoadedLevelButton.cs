using TMPro;
using UnityEngine;

public class LoadedLevelButton : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI levelNameText;

    public void SetLevelNameText(string levelName)
    {
        levelNameText.text = levelName;
    }
}
