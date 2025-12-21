using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public static ColorManager Instance { get; private set; }

    [Header("Arrows")]

    [SerializeField]
    public static Color arrowsHoverColor = Color.yellow;

    [SerializeField]
    public static Color inactiveColor = Color.grey;

    [SerializeField]
    public static Color rayColor = Color.cyan;

    [SerializeField]
    public static Color rayMultiSelectColor = Color.red;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
}
