using System;
using UnityEngine;

public class PlayerPreferencesManager : MonoBehaviour
{

    public static PlayerPreferencesManager Instance { get; private set; }

    [Header("Gizmos")]

    public float axisMultiplier = 2.0f;

    public GizmoType currentGizmoType = GizmoType.Arrows;

    public event Action<GizmoType> OnGizmoTypeChanged;

    [Header("Snapping")]

    [Header("Snapping Translation")]

    public float snappingTranslationAmount = 0.5f;

    public float snappingTranslationAmountMax = 2.0f;

    [Header("Snapping Rotation")]

    public float snappingRotationAmount = 45.0f;

    public float snappingRotationAmountMax = 180.0f;

    [Header("Snapping Scacle")]

    public float snappingScaleAmount = 0.5f;

    public float snappingScaleAmountMax = 2.0f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SetGizmoType(GizmoType newGizmoType)
    {
        currentGizmoType = newGizmoType;
        OnGizmoTypeChanged.Invoke(currentGizmoType);
    }

    public static float GetSnappedNumber(float snapLimit, float number)
    {
        return snapLimit * Mathf.Round(number /  snapLimit);
    }

    public static Vector3 GetIfSnappedPosition(Vector3 position)
    {
        if(Instance.snappingTranslationAmount > 0)
        {
            return new Vector3(GetSnappedNumber(Instance.snappingTranslationAmount, position.x),
                GetSnappedNumber(Instance.snappingTranslationAmount, position.y),
                GetSnappedNumber(Instance.snappingTranslationAmount, position.z));
        }
        else
        {
            return position;
        }
    }

    public static Vector3 GetIfSnappedScale(Vector3 scale)
    {
        if(Instance.snappingScaleAmount > 0)
        {
            return new Vector3(GetSnappedNumber(Instance.snappingScaleAmount, scale.x),
                GetSnappedNumber(Instance.snappingScaleAmount, scale.y),
                GetSnappedNumber(Instance.snappingScaleAmount, scale.z));
        }
        else
        {
            return scale;
        }
    }

    public static float GetIfSnappedAngle(float angle)
    {
        if(Instance.snappingRotationAmount > 0)
        {
            return GetSnappedNumber(Instance.snappingRotationAmount, angle);
        }
        else
        {
            return angle;
        }
    }

    public static float AddTranslationSnappingAmount(float amount)
    {
        return SetTranslationSnappingAmount(Instance.snappingTranslationAmount + amount);
    }

    public static float RemoveTranslationSnappingAmount(float amount)
    {
        return SetTranslationSnappingAmount(Instance.snappingTranslationAmount - amount);
    }

    private static float SetTranslationSnappingAmount(float newAmount)
    {
        Instance.snappingTranslationAmount = Mathf.Clamp(newAmount, 0, Instance.snappingTranslationAmountMax);
        return Instance.snappingTranslationAmount;
    }

    public static float AddRotationSnappingAmount(float amount)
    {
        return SetRotationSnappingAmount(Instance.snappingRotationAmount + amount);
    }

    public static float RemoveRotationSnappingAmount(float amount)
    {
        return SetRotationSnappingAmount(Instance.snappingRotationAmount - amount);
    }

    private static float SetRotationSnappingAmount(float newAmount)
    {
        Instance.snappingRotationAmount = Mathf.Clamp(newAmount, 0, Instance.snappingRotationAmountMax);
        return Instance.snappingRotationAmount;
    }

    public static float AddScaleSnappingAmount(float amount)
    {
        return SetScaleSnappingAmount(Instance.snappingScaleAmount + amount);
    }

    public static float RemoveScaleSnappingAmount(float amount)
    {
        return SetScaleSnappingAmount(Instance.snappingScaleAmount - amount);
    }

    private static float SetScaleSnappingAmount(float newAmount)
    {
        Instance.snappingScaleAmount = Mathf.Clamp(newAmount, 0, Instance.snappingScaleAmountMax);
        return Instance.snappingScaleAmount;
    }
}
