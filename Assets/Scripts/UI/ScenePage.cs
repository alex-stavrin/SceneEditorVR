using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScenePage : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI sceneName;

    [SerializeField]
    Button saveButton;

    [SerializeField]
    Button exportButton;

    void OnEnable()
    {
        if(SaveAndLoadManager.Instance)
        {            
            sceneName.SetText(SaveAndLoadManager.GetCurrentLevelName());
            ActorsManager.OnActorAdded += OnActorAdded;
            ActorsManager.OnActorRemoved += OnActorRemoved;
        }

        SetButtonsState(ActorsManager.GetActors().Count > 0);
    }

    void OnDisable()
    {
        ActorsManager.OnActorAdded -= OnActorAdded;
        ActorsManager.OnActorRemoved -= OnActorRemoved;
    }

    void OnActorAdded(Actor actor)
    {
        SetButtonsState(ActorsManager.GetActors().Count > 0);
    }

    void OnActorRemoved(Actor actor)
    {
        SetButtonsState(ActorsManager.GetActors().Count > 0);
    }

    void SetButtonsState(bool value)
    {
        saveButton.interactable = value;
        exportButton.interactable = value;       
    }
}
