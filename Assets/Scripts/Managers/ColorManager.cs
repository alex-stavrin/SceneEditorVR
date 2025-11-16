using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public static ColorManager Instance { get; private set; }

    [Header("Arrows")]

    [SerializeField]
    public Color arrowsHoverColor = Color.yellow;

    [SerializeField]
    public Color inactiveColor = Color.grey;

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
