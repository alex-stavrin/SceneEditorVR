using UnityEngine;

public class Rotateable : MonoBehaviour
{
    public void SetRotationAroundAxis(Vector3 axis, Quaternion startingRotation, float angle)
    {
        transform.rotation = Quaternion.AngleAxis(PlayerPreferencesManager.GetIfSnappedAngle(angle), axis) * startingRotation;
    }

    public void SetRotationAroundAxisAndPoint(Vector3 point, Vector3 axis, Quaternion startingRotation, Vector3 startingPosition, float angle)
    {
        float finalAngle = PlayerPreferencesManager.GetIfSnappedAngle(angle);
        
        // Create the rotation offset
        Quaternion rotation = Quaternion.AngleAxis(finalAngle, axis);

        // 1. Update Rotation
        transform.rotation = rotation * startingRotation;

        // 2. Update Position (Orbit logic)
        Vector3 relativeOffset = startingPosition - point;
        Vector3 rotatedOffset = rotation * relativeOffset;
        
        transform.position = point + rotatedOffset;
    }
}
