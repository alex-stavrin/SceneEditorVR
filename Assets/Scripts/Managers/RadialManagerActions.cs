using UnityEngine;

public class RadialManagerActions : RadialManager
{
    public static RadialManagerActions Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void DuplicateSelected()
    {
        ActionsManager.Instance.DuplicateSelected();
    }

    public void Cancel()
    {
        ///
    }
    
    public void DeleteSelected()
    {
        ActionsManager.Instance.DeleteSelected();
    }
}
