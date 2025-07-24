using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; private set; }

    Interactable currentSelectable;

    [SerializeField]
    Color hoverColor = Color.white;

    [SerializeField]
    Color selectedColor = Color.yellow;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetCurrentSelectable(Interactable newSelectable)
    {
        if(currentSelectable)
        {
            currentSelectable.StopSelect();
        }

        currentSelectable = newSelectable;
        currentSelectable.StartSelect();

        InspectorManager.Instance.SetInspected(currentSelectable.gameObject);
    }

    public Color GetSelectedColor()
    {
        return selectedColor;
    }

    public Color GetHoverColor()
    {
        return hoverColor;
    }
}
