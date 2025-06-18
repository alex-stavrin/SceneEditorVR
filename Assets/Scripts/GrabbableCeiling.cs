using UnityEngine;

public class GrabbableCeiling : Grabbable
{
    public override (Vector3 limitedPosition, Quaternion limitedRotation) LimitPositionAndRotation
        (Vector3 desiredPosition, Quaternion desiredRotation, Vector3 offset)
    {
        Vector3 actualPosition = desiredPosition;
        actualPosition.y = RoomManager.Instance.GetCeilingLevel();
        return base.LimitPositionAndRotation(actualPosition, desiredRotation, offset);
    }
}
