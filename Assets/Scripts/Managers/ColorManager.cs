using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public static ColorManager Instance { get; private set; }

    [Header("Arrows")]

    [SerializeField]
    public Color arrowsHoverColor = Color.yellow;

    [SerializeField]
    public Color inactiveColor = Color.grey;

    [Header("Ray")]

    [SerializeField]
    public Color rayNoHitColor = Color.white;

    [SerializeField]
    public Color rayUIColor = Color.cyan;

    [SerializeField]
    public Color rayGizmoColor = Color.yellow;

    [SerializeField]
    public Color rayGizmoHit = Color.orange;

    [SerializeField]
    public Color rayMultiSelectColor = Color.red;

    [Header("Axis")]

    [SerializeField]
    Color xAxisColor;

    [SerializeField]
    Color yAxisColor;

    [SerializeField]
    Color zAxisColor;

    [Header("Color Pallette")]

    [SerializeField]
    Color highlightColor;

    [SerializeField]
    Color neutralColor;

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
    public static Color GetRayMultiSelectColor() {return Instance.rayMultiSelectColor;}
    public static Color GetXAxisColor() {return Instance.xAxisColor;}
    public static Color GetYAxisColor() {return Instance.yAxisColor;}
    public static Color GetZAxisColor() {return Instance.zAxisColor;}
    public static Color GetHighlightColor() {return Instance.highlightColor;}
    public static Color GetNeutralColor() {return Instance.neutralColor;}
}
