using TMPro;
using UnityEngine;

public class InspectorManager : MonoBehaviour
{
    public static InspectorManager Instance { get; private set; }

    [SerializeField]
    TextMeshProUGUI inspectedNameText;

    GameObject currentInspected;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
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