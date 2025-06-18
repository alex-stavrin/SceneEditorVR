using UnityEngine.EventSystems;

public class PointerEventDataVR : PointerEventData
{
    public Controller controller;
    
    public PointerEventDataVR(EventSystem es) : base(es)
    {

    }
}