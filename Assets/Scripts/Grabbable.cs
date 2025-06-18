using UnityEngine;

public class Grabbable : MonoBehaviour
{
    bool isGrabbed = false;
    [HideInInspector] protected Controller grabbingController;

    [SerializeField] bool movePosition = true;
    [SerializeField] bool moveRotation = true;
    [SerializeField] public bool snapGrab = true;
    [SerializeField] public bool allowDirectGrab = true;
    [SerializeField] public bool stayInRoom = true;

    [Header("Grab Move Limits")]
    [SerializeField] public bool limitGrabMove = true;
    [SerializeField] public float stopGrabMoveThreshold = 0.5f;

    [HideInInspector] public float limitOffset = 0;

    Vector3 currentGrabOffset;

    Vector3 lastValidPosition;

    public virtual void StartGrab(Controller starGrabController)
    {
        if(isGrabbed)
        {
            grabbingController.StopGrab();
        }

        grabbingController = starGrabController;
        isGrabbed = true;
    }
    public virtual void UpdateGrab(float distance, Vector3 offset, bool rotate=true)
    {
        if(isGrabbed)
        {
            Vector3 desiredPosition = Vector3.zero;
            Quaternion desiredRotation = Quaternion.identity;

            if (movePosition)
            {
                if(snapGrab)
                {
                    offset = Vector3.zero;
                }
                currentGrabOffset = offset;

                desiredPosition = grabbingController.GetGrabPoint().transform.position + offset + grabbingController.GetGrabPoint().forward * distance;
            }

            if (moveRotation && rotate)
            {
                if(allowDirectGrab)
                {
                    desiredRotation = grabbingController.GetGrabPoint().transform.rotation;
                }
            }
            else
            {
                desiredRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
            }

            (Vector3 limitedPosition, Quaternion limitedRotation) = LimitPositionAndRotation(desiredPosition, desiredRotation, offset);
            lastValidPosition = limitedPosition;

            limitOffset = Vector3.Distance(limitedPosition, desiredPosition);
            VirtualRealityConsole.PrintMessage(limitOffset.ToString(), PrintTypeVRC.Clear);

            if(movePosition)
            {
                transform.position = limitedPosition;
            }

            if(moveRotation)
            {
                transform.rotation = limitedRotation;
            }
        }
    }
    public virtual void StopGrab()
    {
        if(isGrabbed)
        {
            isGrabbed = false;
            grabbingController.StopGrab();
            grabbingController = null;
        }
    }

    public virtual (Vector3 limitedPosition, Quaternion limitedRotation) LimitPositionAndRotation
        (Vector3 desiredPosition, Quaternion desiredRotation, Vector3 offset)
    {
        BoxCollider roomBound1 = RoomManager.Instance.GetRoomBound(0);
        BoxCollider roomBound2 = RoomManager.Instance.GetRoomBound(1);
        if(roomBound1.bounds.Contains(desiredPosition) || roomBound2.bounds.Contains(desiredPosition) || !stayInRoom)
        {
            
            return (desiredPosition, desiredRotation);
        }
        else // this means we are outside the bounds
        {
            return (new Vector3(lastValidPosition.x, desiredPosition.y, lastValidPosition.z), desiredRotation);
        }

    }

    public Vector3 GetLineEndPoint()
    {
        if (isGrabbed)
        {
           if(snapGrab)
           {
                return transform.position;
           }
           else
           {
                return transform.position - currentGrabOffset;
           }
        }
        else
        {
            return Vector3.zero;
        }
    }
}
