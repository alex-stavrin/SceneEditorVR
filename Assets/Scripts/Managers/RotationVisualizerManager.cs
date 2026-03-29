using UnityEngine;

public class RotationVisualizerManager : MonoBehaviour
{
    public static RotationVisualizerManager Instance { get; private set; }

    [SerializeField]
    Transform rotationCenterVisualizer;

    [SerializeField]
    Transform rotationTargetVisualizer;

    [SerializeField]
    Transform rotationCenterLineVisualizer;

    [SerializeField]
    LineRenderer rotationLine;

    [SerializeField]
    public float visualizationRadius = 0.2f;

    private Material rotationCenterMaterial;
    private Material rotationCenterLineMateral;
    private Material rotationTargetMaterial;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        rotationCenterVisualizer.gameObject.SetActive(false);
        rotationTargetVisualizer.gameObject.SetActive(false);

        rotationCenterMaterial = GetMaterialFromVisualizer(rotationCenterVisualizer);
        rotationTargetMaterial = GetMaterialFromVisualizer(rotationTargetVisualizer);
        rotationCenterLineMateral = GetMaterialFromVisualizer(rotationCenterLineVisualizer);
    }

    Material GetMaterialFromVisualizer(Transform visualizer)
    {
        MeshRenderer visualizerMeshRenderer = visualizer.gameObject.GetComponent<MeshRenderer>();
        return visualizerMeshRenderer.material;
    }

    public static void UpdateLineStartWorldToLocal(Vector3 startLocationWorld)
    {
        Vector3 localPos = Instance.rotationLine.transform.InverseTransformPoint(startLocationWorld);
        Instance.rotationLine.SetPosition(0, startLocationWorld);
    }

    public static void UpdateLineEndWorldToLocal(Vector3 endLocationWorld)
    {
        Vector3 localPos = Instance.rotationLine.transform.InverseTransformPoint(endLocationWorld);
        Instance.rotationLine.SetPosition(1, endLocationWorld);
    }

    public void InitVisualizers(Vector3 startingLocation, Vector3 rotatingVector)
    {
        Color color;
        if(rotatingVector == new Vector3(1,0,0))
        {
            color = ColorManager.GetXAxisColor();
            rotationCenterLineVisualizer.transform.rotation = Quaternion.Euler(0,0,90);
        }
        else if (rotatingVector == new Vector3(0,1,0))
        {
            color = ColorManager.GetYAxisColor();
            rotationCenterLineVisualizer.transform.rotation = Quaternion.Euler(0,0,0);
        }
        else
        {
            color = ColorManager.GetZAxisColor();
            rotationCenterLineVisualizer.transform.rotation = Quaternion.Euler(90,0,0);
        }

        rotationCenterMaterial.SetColor("_Color", color);
        rotationTargetMaterial.SetColor("_Color", color);
        rotationCenterLineMateral.SetColor("_Color", color);

        rotationLine.startColor = color;
        rotationLine.endColor = color;

        rotationCenterVisualizer.gameObject.SetActive(true);
        rotationTargetVisualizer.gameObject.SetActive(true);

        rotationCenterVisualizer.position = startingLocation;
    }

    public void SetTargetVisualizerLocation(Vector3 location)
    {
        rotationTargetVisualizer.position = location;
    }

    public void DismissVisualizers()
    {
        rotationCenterVisualizer.gameObject.SetActive(false);
        rotationTargetVisualizer.gameObject.SetActive(false);
    }
}
