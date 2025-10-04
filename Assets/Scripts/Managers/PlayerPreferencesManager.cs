using System;
using UnityEngine;

public class PlayerPreferencesManager : MonoBehaviour
{

    public static PlayerPreferencesManager Instance { get; private set; }
    public float axisMultiplier = 2.0f;

    public GizmoType currentGizmoType = GizmoType.Arrows;

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
}
