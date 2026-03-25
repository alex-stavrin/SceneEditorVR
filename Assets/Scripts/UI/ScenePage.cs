using TMPro;
using UnityEngine;

public class ScenePage : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI sceneName;

    void OnEnable()
    {
        if(SaveAndLoadManager.Instance)
        {            
            sceneName.SetText(SaveAndLoadManager.GetCurrentLevelName());
        }
    }
}
