using UnityEngine;
using UnityEngine.Events;

public class RadialManager : MonoBehaviour
{

    [SerializeField]
    GameObject radialsRoot;

    [SerializeField]
    GameObject[] radials;

    [SerializeField]
    Vector2[] minMaxOptions;

    [SerializeField]
    UnityEvent[] functions;

    private Controller currentController;
    private Material[] radialMaterials = new Material[3];

    int currentPick = -1;



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

            for(int i = 0; i < minMaxOptions.Length; i++)
            {
                if(radialAngle >= minMaxOptions[i].x && radialAngle <= minMaxOptions[i].y)
                {
                    PickRadial(i);
                    break;
                }
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
            functions[currentPick].Invoke();

            currentController = null;
            radialsRoot.SetActive(false);
        }
    }
}
