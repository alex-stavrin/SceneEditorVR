using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(Button), typeof(Image))]
public class SpawnerButton :  MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    GameObject gameObjectToSpawn;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData is PointerEventDataVR eventDataVR)
        {
            List<GameObject> gameObjects = new List<GameObject> {gameObjectToSpawn};

            List<Pose> poses = new List<Pose>();
            Vector3 spawnPosition = eventDataVR.controller.rayHitResult.point;
            poses.Add(new Pose(spawnPosition, Quaternion.identity));

            List<GameObject> spawned = ActionsManager.SpawnGameObjects(gameObjects, poses);

            // should always be 1
            if(spawned.Count != 1) return; 

            InteractableMoveableDraggable interactableMoveableDraggable = spawned[0].GetComponent<InteractableMoveableDraggable>();
            if(interactableMoveableDraggable)
            {
                eventDataVR.controller.StartInteract(interactableMoveableDraggable);
            }
        }
    }
}
