using UnityEngine;

public class Rotateable : MonoBehaviour
{
    public void SetRotationAroundAxis(Vector3 axis, Quaternion startingRotation, float angle)
    {
        transform.rotation = Quaternion.AngleAxis(PlayerPreferencesManager.GetIfSnappedAngle(angle), axis) * startingRotation;
    }
}
