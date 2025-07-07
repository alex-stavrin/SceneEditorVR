using UnityEngine;
using UnityEngine.EventSystems;

public class SpawnerButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] GameObject spawnObject;
    public void OnPointerDown(PointerEventData pointerData)
    {
        if(pointerData is PointerEventDataVR pointerDataVR)
        {
            Controller controller = pointerDataVR.controller;
            if(controller)
            {
                var spawnedObject = Instantiate(spawnObject, controller.rayHitResult.point, Quaternion.identity);
                Grabbable grabbable = spawnedObject.GetComponent<Grabbable>();
                if(grabbable)
                {
                    controller.StartGrab(grabbable, controller.rayHitResult.distance, Vector3.zero);
                }
            }
        }
    }
}
