using System;
using UnityEngine;

public class PlayerPreferencesManager : MonoBehaviour
{

    public static PlayerPreferencesManager Instance { get; private set; }
    public float axisMultiplier = 2.0f;

    public GizmoType currentGizmoType = GizmoType.Arrows;

    public event Action<GizmoType> OnGizmoTypeChanged;

    public bool snapping = true;

    public float snappingTranslationAmount = 0.5f;

    public float snappingRotationAmount = 45.0f;

    public float snappingScaleAmount = 0.5f;

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
        if(Instance.snapping)
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
        if(Instance.snapping)
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
        if(Instance.snapping)
        {
            return GetSnappedNumber(Instance.snappingRotationAmount, angle);
        }
        else
        {
            return angle;
        }
    }
}
