using System.Collections.Generic;
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
    private List<Material> radialMaterials = new List<Material>();

    private bool[] radialActivated;

    int currentPick = -1;

    void Start()
    {
        radialsRoot.SetActive(false);

        for (int i = 0; i < radials.Length; i++)
        {
            MeshRenderer meshRenderer = radials[i].GetComponent<MeshRenderer>();
            if (meshRenderer)
            {
                radialMaterials.Add(meshRenderer.material);
            }
        }

        radialActivated = new bool[radials.Length];
        for (int i = 0; i < radialActivated.Length; i++)
        {
            radialActivated[i] = true;
        }
    }

    void Update()
    {
        if (currentController)
        {
            // 2 == duplicate
            // 3 == delete
            bool haveSomethingSelected = SelectionManager.GetSelectedActors().Count >= 1;
            SetRadialActivation(2, haveSomethingSelected);
            SetRadialActivation(3, haveSomethingSelected);

            Vector3 radialUp = radialsRoot.transform.up;
            Vector3 radialPlane = radialsRoot.transform.forward;
            Vector3 controllerDirection = currentController.transform.position - radialsRoot.transform.position;
            Vector3 projectedControllerDirection = Vector3.ProjectOnPlane(controllerDirection, radialPlane);
            float radialAngle = Vector3.SignedAngle(radialUp, projectedControllerDirection, radialPlane);

            for(int i = 0; i < minMaxOptions.Length; i++)
            {
                if(radialAngle >= minMaxOptions[i].x && radialAngle <= minMaxOptions[i].y && radialActivated[i])
                {
                    PickRadial(i);
                    break;
                }
            }
        }
    }

    void PickRadial(int i)
    {
        for (int j = 0; j < radialMaterials.Count; j++)
        {
            if (j == i)
            {
                if (currentPick != i)
                {
                    if(HapticsManager.Instance) HapticsManager.PlayHapticRadialPick(currentController.GetSide());
                    radialMaterials[j].SetColor("_Color", ColorManager.GetHighlightColor());
                    radialMaterials[j].SetFloat("_Alpha", 1.0f);
                    currentPick = i;
                }
            }
            else
            {
                radialMaterials[j].SetColor("_Color", ColorManager.GetNeutralColor());
                radialMaterials[j].SetFloat("_Alpha", 0.5f);
            }
        }
    }

    public void CallRadial(Controller controllerCalling)
    {
        SoundsManager.PlayRadialOpen(radialsRoot.transform.position);
        
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
            SoundsManager.PlayRadialClose(radialsRoot.transform.position);

            functions[currentPick].Invoke();

            currentController = null;
            radialsRoot.SetActive(false);
        }
    }

    public void SetRadialActivation(int radialIndex, bool value)
    {
        radialActivated[radialIndex] = value;
    }
}
