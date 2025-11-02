using System;
using UnityEngine;

public class PlayerPreferencesManager : MonoBehaviour
{

    public static PlayerPreferencesManager Instance { get; private set; }
    public float axisMultiplier = 2.0f;

    public GizmoType currentGizmoType = GizmoType.Arrows;

    public event Action<GizmoType> OnGizmoTypeChanged;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SetGizmoType(GizmoType newGizmoType)
    {
        currentGizmoType = newGizmoType;
        OnGizmoTypeChanged.Invoke(currentGizmoType);
    }
}
