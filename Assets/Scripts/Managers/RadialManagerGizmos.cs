using UnityEngine;

public class RadialManagerGizmos : RadialManager
{
    public static RadialManagerGizmos Instance { get; private set; }

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
    
    public void SetArrowGizmo()
    {
        PlayerPreferencesManager.Instance.SetGizmoType(GizmoType.Arrows);
    }

    public void SetRotatorGizmo()
    {
        PlayerPreferencesManager.Instance.SetGizmoType(GizmoType.Rotators);
    }

    public void SetScalerGizmo()
    {
        PlayerPreferencesManager.Instance.SetGizmoType(GizmoType.Scalers);
    }
}
