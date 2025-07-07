using UnityEngine;
using UnityEngine.EventSystems;

public class DeleteButton : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData pointerData)
    {
        GameObject rootObject = transform.root.gameObject;
        if(rootObject)
        {
            Destroy(rootObject);
        }
    }
}
