using UnityEngine;

public class Grabbable : MonoBehaviour
{
    bool isGrabbed = false;
    [HideInInspector] public Controller grabbingController;

    [SerializeField] bool movePosition;
    [SerializeField] bool moveRotation;

    public virtual void StartGrab()
    {
        isGrabbed = true;
    }
    public virtual void UpdateGrab(float offset, bool rotate=true)
    {
        if(isGrabbed)
        {
            if (movePosition)
            {
                transform.position = grabbingController.GetGrabPoint().transform.position + grabbingController.GetGrabPoint().forward * offset;
            }

            if (moveRotation && rotate)
            {
                transform.rotation = grabbingController.GetGrabPoint().transform.rotation;
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
            }
        }
    }
    public virtual void StopGrab()
    {
        isGrabbed = false;
        grabbingController.StopGrab();
    }
}
