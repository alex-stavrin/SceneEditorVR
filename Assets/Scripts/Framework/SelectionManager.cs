using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; private set; }

    Selectable currentSelectable;

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

    public void SetCurrentSelectable(Selectable newSelectable)
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
