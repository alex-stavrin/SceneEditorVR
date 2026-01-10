using UnityEngine;

public class RotationVisualizerManager : MonoBehaviour
{
    public static RotationVisualizerManager Instance { get; private set; }

    [SerializeField]
    Transform rotationCenterVisualizer;

    [SerializeField]
    Transform rotationTargetVisualizer;

    [SerializeField]
    public float visualizationRadius = 0.2f;

    private Material rotationCenterMaterial;
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
    }

    Material GetMaterialFromVisualizer(Transform visualizer)
    {
        MeshRenderer visualizerMeshRenderer = visualizer.gameObject.GetComponent<MeshRenderer>();
        return visualizerMeshRenderer.material;
    }

    public void InitVisualizers(Vector3 startingLocation, Color color)
    {
        rotationCenterMaterial.SetColor("_Color", color);
        rotationTargetMaterial.SetColor("_Color", color);

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
