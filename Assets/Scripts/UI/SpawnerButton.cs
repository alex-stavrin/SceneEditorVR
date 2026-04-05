using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(Button), typeof(Image))]
public class SpawnerButton :  MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    string prefabPath;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData is PointerEventDataVR eventDataVR)
        {
            List<string> paths = new List<string> {prefabPath};

            List<SpawnTransform> poses = new List<SpawnTransform>();
            Vector3 spawnPosition = eventDataVR.controller.rayHitResult.point;
            SpawnTransform spawnTransform;
            spawnTransform.location = spawnPosition;
            spawnTransform.rotation = Quaternion.identity;
            spawnTransform.scale = Vector3.one;
            poses.Add(spawnTransform);

            ActionsManager.SpawnGameObjects(paths, poses, eventDataVR.controller);
        }
    }
}
