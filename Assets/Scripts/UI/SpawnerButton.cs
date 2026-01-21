using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement;

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

            List<Pose> poses = new List<Pose>();
            Vector3 spawnPosition = eventDataVR.controller.rayHitResult.point;
            poses.Add(new Pose(spawnPosition, Quaternion.identity));

            ActionsManager.SpawnGameObjects(paths, poses, eventDataVR.controller);
        }
    }
}
