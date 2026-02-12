using UnityEngine;
using UnityEngine.UI;

public class WorldPanel : MonoBehaviour
{
    public static WorldPanel Instance { get; private set; }

    [Header("UI")]

    [SerializeField]
    Toggle directionalLightToggle;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        directionalLightToggle.onValueChanged.AddListener(OnDirectionalLightToggleChanged);

    }

    public void OnDirectionalLightToggleChanged(bool value)
    {
        WorldManager.SetDirectionalLightEnabled(value);
    }

    static public void SetToggleState(bool state)
    {
        Instance.directionalLightToggle.isOn = state;
    }
}