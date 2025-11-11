using TMPro;
using UnityEngine;

public class InspectorManager : MonoBehaviour
{
    public static InspectorManager Instance { get; private set; }

    [Header("General")]

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

    void Update()
    {
        if(currentInspected)
        {
            Vector3 inspectedPosition = currentInspected.transform.position;
            labelLocationX.text = inspectedPosition.x.ToString();
            labelLocationY.text = inspectedPosition.y.ToString();
            labelLocationZ.text = inspectedPosition.z.ToString();

            Vector3 inspectedRotation = currentInspected.transform.rotation.eulerAngles;
            labelRotationX.text = inspectedRotation.x.ToString();
            labelRotationY.text = inspectedRotation.y.ToString();
            labelRotationZ.text = inspectedRotation.z.ToString();

            Vector3 inspectedScale = currentInspected.transform.localScale;
            labelScaleX.text = inspectedScale.x.ToString();
            labelScaleY.text = inspectedScale.y.ToString();
            labelScaleZ.text = inspectedScale.z.ToString();
        }
    }

    public void SetInspected(GameObject inspected)
    {
        if(inspected)
        {
            currentInspected = inspected;
            inspectedNameText.text = inspected.name;
        }
        else
        {
            inspectedNameText.text = "";
        }
    }
}