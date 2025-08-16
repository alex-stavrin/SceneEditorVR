using UnityEngine;

public class PlayerPreferencesManager : MonoBehaviour
{

    public static PlayerPreferencesManager Instance { get; private set; }

    public bool useAxes;

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
