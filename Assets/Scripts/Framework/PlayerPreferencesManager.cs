using System;
using UnityEngine;

public class PlayerPreferencesManager : MonoBehaviour
{

    public static PlayerPreferencesManager Instance { get; private set; }

    public bool useAxes;

    public event Action<bool> OnUseAxesChanged;

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

    public void SetUseAxes(bool newUseAxes)
    {
        useAxes = newUseAxes;
        OnUseAxesChanged.Invoke(useAxes);
    }
}
