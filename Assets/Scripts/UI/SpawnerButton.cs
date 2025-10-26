using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(Image))]
public class SpawnerButton :  MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    GameObject gameObjectToSpawn;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData is PointerEventDataVR eventDataVR)
        {
            Vector3 spawnPosition = eventDataVR.controller.rayHitResult.point;
            GameObject spawned = Instantiate(gameObjectToSpawn, spawnPosition, Quaternion.identity);

            InteractableMoveableDraggable interactableMoveableDraggable = spawned.GetComponent<InteractableMoveableDraggable>();
            if(interactableMoveableDraggable)
            {
                eventDataVR.controller.StartInteract(interactableMoveableDraggable);
            }
        }
    }
}
