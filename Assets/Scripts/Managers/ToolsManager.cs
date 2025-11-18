using TMPro;
using UnityEngine;

public class ToolsManager : MonoBehaviour
{
    [Header("Translation Snapping")]

    [SerializeField]
    float translationSnappingDelta = 0.1f;

    [SerializeField]
    TextMeshProUGUI translationAmountText;

    [Header("Rotation Snapping")]

    [SerializeField]
    float rotationSnappingDelta = 5.0f;

    [SerializeField]
    TextMeshProUGUI rotationAmountText;

    [Header("Scale snapping")]

    [SerializeField]
    float scaleSnappingDelta = 0.1f;

    [SerializeField]
    TextMeshProUGUI scaleAmountText;

    void Start()
    {
        translationAmountText.SetText(PlayerPreferencesManager.Instance.snappingTranslationAmount.ToString("F1"));
        rotationAmountText.SetText(PlayerPreferencesManager.Instance.snappingRotationAmount.ToString("F1"));   
        scaleAmountText.SetText(PlayerPreferencesManager.Instance.snappingScaleAmount.ToString("F1"));          
    }

    public void AddTranslationSnappingAmount()
    {
        float res = PlayerPreferencesManager.AddTranslationSnappingAmount(translationSnappingDelta);
        translationAmountText.SetText(res.ToString("F1"));
    }

    public void RemoveTranslationSnappingAmount()
    {
        float res = PlayerPreferencesManager.RemoveTranslationSnappingAmount(translationSnappingDelta);
        translationAmountText.SetText(res.ToString("F1"));
    }

    public void AddRotationSnappingAmount()
    {
        float res = PlayerPreferencesManager.AddRotationSnappingAmount(rotationSnappingDelta);
        rotationAmountText.SetText(res.ToString("F1"));
    }

    public void RemoveRotationSnappingAmount()
    {
        float res = PlayerPreferencesManager.RemoveRotationSnappingAmount(rotationSnappingDelta);
        rotationAmountText.SetText(res.ToString("F1"));
    }

    public void AddScaleSnappingAmount()
    {
        float res = PlayerPreferencesManager.AddScaleSnappingAmount(scaleSnappingDelta);
        scaleAmountText.SetText(res.ToString("F1"));
    }

    public void RemoveScaleSnappingAmount()
    {
        float res = PlayerPreferencesManager.RemoveScaleSnappingAmount(scaleSnappingDelta);
        scaleAmountText.SetText(res.ToString("F1"));
    }
}
