using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; private set; }

    Selectable currentSelectable;
    Selectable currentHover;

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

    }

    public void SetCurrentHover(Selectable newHover)
    {
        if (newHover == currentHover) return;

        // is null or new hover

        if(currentHover)
        {
            currentHover.StopHover();
        }

        if(!newHover)
        {
            currentHover = null;
        }
        else
        {
            newHover.StartHover();
            currentHover = newHover;
        }
    }
}
