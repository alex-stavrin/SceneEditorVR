using UnityEngine;

public class GrabbableWall : Grabbable
{
    [SerializeField] LayerMask layerMask;
    [SerializeField] float stickOffset = 0.1f;
    [SerializeField] float stickDistance = 0.5f;
    public override (Vector3 limitedPosition, Quaternion limitedRotation) LimitPositionAndRotation
        (Vector3 desiredPosition, Quaternion desiredRotation, Vector3 offset)
    {

        RaycastHit hitInfo;
        bool rayHit = Physics.Raycast(grabbingController.GetRayStart(), grabbingController.GetGrabPoint().transform.forward,
            out hitInfo, grabbingController.GetRayRange(), layerMask);

        float distance = Vector3.Distance(hitInfo.point, desiredPosition);

        if(rayHit)
        {
            Vector3 toOther = desiredPosition - hitInfo.point;
            float dot = Vector3.Dot(hitInfo.normal, toOther.normalized);
            if(distance < stickDistance || dot < 0f)
            {
                Quaternion newRotation = Quaternion.LookRotation(hitInfo.normal);
                Vector3 newPosition = hitInfo.point + hitInfo.normal * stickOffset + offset;

                return base.LimitPositionAndRotation(newPosition, newRotation, offset);

            }
        }

        return base.LimitPositionAndRotation(desiredPosition, desiredRotation, offset);
    }
}
