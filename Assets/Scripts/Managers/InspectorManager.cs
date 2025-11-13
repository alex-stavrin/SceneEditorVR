using TMPro;
using UnityEngine;

public class InspectorManager : MonoBehaviour
{
    public static InspectorManager Instance { get; private set; }

    [Header("General")]
    [SerializeField]
    GameObject inspectedElements;

    [SerializeField]
    TextMeshProUGUI inspectedNameText;

    [Header("Location")]

    [SerializeField]
    TextMeshProUGUI labelLocationX;

    [SerializeField]
    TextMeshProUGUI labelLocationY;

    [SerializeField]
    TextMeshProUGUI labelLocationZ;

    [Header("Rotation")]

    [SerializeField]
    TextMeshProUGUI labelRotationX;

    [SerializeField]
    TextMeshProUGUI labelRotationY;

    [SerializeField]
    TextMeshProUGUI labelRotationZ;

    [Header("Scale")]
    [SerializeField]
    TextMeshProUGUI labelScaleX;

    [SerializeField]
    TextMeshProUGUI labelScaleY;

    [SerializeField]
    TextMeshProUGUI labelScaleZ;
    


    GameObject currentInspected;

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
        inspectedElements.SetActive(false);
    }

    void Update()
    {
        if(currentInspected)
        {
            Vector3 inspectedPosition = currentInspected.transform.position;
            labelLocationX.text = inspectedPosition.x.ToString("F2");
            labelLocationY.text = inspectedPosition.y.ToString("F2");
            labelLocationZ.text = inspectedPosition.z.ToString("F2");

            Vector3 inspectedRotation = currentInspected.transform.rotation.eulerAngles;
            labelRotationX.text = inspectedRotation.x.ToString("F2");
            labelRotationY.text = inspectedRotation.y.ToString("F2");
            labelRotationZ.text = inspectedRotation.z.ToString("F2");

            Vector3 inspectedScale = currentInspected.transform.localScale;
            labelScaleX.text = inspectedScale.x.ToString("F2");
            labelScaleY.text = inspectedScale.y.ToString("F2");
            labelScaleZ.text = inspectedScale.z.ToString("F2");
        }
    }

    public void SetInspected(GameObject inspected)
    {
        
        if(inspected)
        {
            inspectedElements.SetActive(true);
            currentInspected = inspected;
            inspectedNameText.text = inspected.name;
        }
        else
        {
            inspectedElements.SetActive(false);
        }
    }
}