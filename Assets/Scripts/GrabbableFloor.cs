using UnityEngine;

public class GrabbableFloor : Grabbable
{
    public override (Vector3 limitedPosition, Quaternion limitedRotation) LimitPositionAndRotation
        (Vector3 desiredPosition, Quaternion desiredRotation, Vector3 offset)
    {
        Vector3 actualPosition = desiredPosition;
        actualPosition.y = 0;

        return base.LimitPositionAndRotation(actualPosition, desiredRotation, offset);
    }
}
