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

    public virtual void Update()
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
        if (currentPick == i) return;

        currentPick = i;
        if(HapticsManager.Instance) HapticsManager.PlayHapticRadialPick(currentController.GetSide());

        for (int j = 0; j < radialMaterials.Count; j++)
        {
            if (j == i)
            {
                radialMaterials[j].SetColor("_Color", ColorManager.GetHighlightColor());
                radialMaterials[j].SetFloat("_Alpha", 1.0f);
            }
            else
            {
                if (radialActivated[j])
                {
                    radialMaterials[j].SetColor("_Color", ColorManager.GetNeutralColor());
                    radialMaterials[j].SetFloat("_Alpha", 0.5f);
                }
                else
                {
                    radialMaterials[j].SetColor("_Color", ColorManager.GetInactiveColor());
                }
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

        currentPick = -1;
        for (int j = 0; j < radialMaterials.Count; j++)
        {
            if (radialActivated[j])
            {
                radialMaterials[j].SetColor("_Color", ColorManager.GetNeutralColor());
                radialMaterials[j].SetFloat("_Alpha", 0.5f);
            }
            else
            {
                radialMaterials[j].SetColor("_Color", ColorManager.GetInactiveColor());
            }
        }
    }

    public void DismissRadial()
    {
        if (currentController)
        {
            SoundsManager.PlayRadialClose(radialsRoot.transform.position);

            if (currentPick >= 0 && currentPick < functions.Length)
            {
                functions[currentPick].Invoke();
            }

            currentController = null;
            radialsRoot.SetActive(false);
        }
    }

    public void SetRadialActivation(int radialIndex, bool value)
    {
        radialActivated[radialIndex] = value;

        if(!value)
        {
            radialMaterials[radialIndex].SetColor("_Color", ColorManager.GetInactiveColor());
        }
    }
}
