using UnityEngine;

public class RadialManager : MonoBehaviour
{
    public static RadialManager Instance { get; private set; }

    [SerializeField]
    GameObject radialsRoot;

    [SerializeField]
    GameObject[] radials;

    [Header("Option 0 angles")]
    [SerializeField]
    float option0Min;
    [SerializeField]
    float option0Max;

    [Header("Option 1 angles")]
    [SerializeField]
    float option1Min;
    [SerializeField]
    float option1Max;

    [Header("Option 2 angles")]
    [SerializeField]
    float option2Min;
    [SerializeField]
    float option2Max;

    private Controller currentController;
    private Material[] radialMaterials = new Material[3];

    int currentPick = -1;

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

    void Start()
    {
        radialsRoot.SetActive(false);

        for (int i = 0; i < radials.Length; i++)
        {
            MeshRenderer meshRenderer = radials[i].GetComponent<MeshRenderer>();
            if (meshRenderer)
            {
                radialMaterials[i] = meshRenderer.material;
            }
        }
    }

    void Update()
    {
        if (currentController)
        {
            Vector3 radialUp = radialsRoot.transform.up;
            Vector3 radialPlane = radialsRoot.transform.forward;
            Vector3 controllerDirection = currentController.transform.position - radialsRoot.transform.position;
            Vector3 projectedControllerDirection = Vector3.ProjectOnPlane(controllerDirection, radialPlane);
            float radialAngle = Vector3.SignedAngle(radialUp, projectedControllerDirection, radialPlane);

            if (radialAngle >= option0Min && radialAngle <= option0Max)
            {
                PickRadial(0);
            }
            else if (radialAngle >= option1Min && radialAngle <= option1Max)
            {
                PickRadial(1);
            }
            else if (radialAngle >= option2Min && radialAngle <= option2Max)
            {
                PickRadial(2);
            }
        }
    }

    void PickRadial(int i)
    {
        for (int j = 0; j < radialMaterials.Length; j++)
        {
            if (j == i)
            {
                if (currentPick != i)
                {                    
                    radialMaterials[j].SetFloat("_Alpha", 1.0f);
                    currentPick = i;
                }
            }
            else
            {
                radialMaterials[j].SetFloat("_Alpha", 0.5f);
            }
        }
    }

    public void CallRadial(Controller controllerCalling)
    {
        currentController = controllerCalling;
        radialsRoot.SetActive(true);
        radialsRoot.transform.position = controllerCalling.transform.position;
        Vector3 direction = radialsRoot.transform.position - PlayerRig.Instance.GetPlayerHead().position;
        direction.Normalize();
        direction.y = 0;
        radialsRoot.transform.rotation = Quaternion.LookRotation(-direction);
    }

    public void DismissRadial()
    {
        if (currentController)
        {
            PlayerPreferencesManager ppmInstance = PlayerPreferencesManager.Instance;
            switch (currentPick)
            {
                case 0:
                    ppmInstance.SetGizmoType(GizmoType.Arrows);
                    break;
                case 1:
                    ppmInstance.SetGizmoType(GizmoType.Rotators);
                    break;
                case 2:
                    ppmInstance.SetGizmoType(GizmoType.Scalers);
                    break;
            }

            currentController = null;
            radialsRoot.SetActive(false);
        }
    }
}
