using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public static ColorManager Instance { get; private set; }

    [Header("Arrows")]

    [SerializeField]
    public Color arrowsHoverColor = Color.yellow;

    [SerializeField]
    public Color inactiveColor = Color.grey;

    [SerializeField]
    public Color rayColor = Color.cyan;

    [SerializeField]
    public Color rayMultiSelectColor = Color.red;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public static Color GetArrowsHoverColor() {return Instance.arrowsHoverColor;}
    public static Color GetInactiveColor() {return Instance.inactiveColor;}
    public static Color GetRayColor() {return Instance.rayColor;}
    public static Color GetRayMultiSelectColor() {return Instance.rayMultiSelectColor;}
}
