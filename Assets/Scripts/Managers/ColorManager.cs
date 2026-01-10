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

    [Header("Axis")]

    [SerializeField]
    Color xAxisColor;

    [SerializeField]
    Color yAxisColor;

    [SerializeField]
    Color zAxisColor;

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
    public static Color GetXAxisColor() {return Instance.xAxisColor;}
    public static Color GetYAxisColor() {return Instance.yAxisColor;}
    public static Color GetZAxisColor() {return Instance.zAxisColor;}
}
