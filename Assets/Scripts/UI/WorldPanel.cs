using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum SkyType
{
    ST_Morning,
    ST_Noon,
    ST_Night
}

public class WorldPanel : MonoBehaviour
{
    [Header("SkyMaterials")]

    [SerializeField]
    Material morningMaterial;

    [SerializeField]
    Material noonMaterial;

    [SerializeField]
    Material nightMaterial;

    [Header("World")]

    [SerializeField]
    Light directionalLight;

    [Header("UI")]

    [SerializeField]
    TMP_Dropdown dropdown;

    [SerializeField]
    Toggle directionalLightToggle;

    private void Awake()
    {
        directionalLightToggle.onValueChanged.AddListener(OnDirectionalLightToggleChanged);
        dropdown.onValueChanged.AddListener(OnDropdownChanged);
    }

    public void OnDirectionalLightToggleChanged(bool value)
    {
        directionalLight.enabled = value;
    }

    public void OnDropdownChanged(int value)
    {
        switch ((SkyType)value)
        {
            case SkyType.ST_Morning:
                RenderSettings.skybox = morningMaterial;
                break;
            case SkyType.ST_Noon:
                RenderSettings.skybox = noonMaterial;
                break;
            case SkyType.ST_Night:
                RenderSettings.skybox = nightMaterial;
                break;
        }

        DynamicGI.UpdateEnvironment();

    }
}
