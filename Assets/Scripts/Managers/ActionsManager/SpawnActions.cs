using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement;
using UnityEngine.ResourceManagement.AsyncOperations;

public struct SpawnTransform
{
    public Vector3 location;
    public Vector3 scale;
    public Quaternion rotation;
}

public class SpawnInfo
{
    public string prefabPath;
    public GameObject spawnedGameObject;
    public SpawnTransform spawnTransform;
}

public class SpawnAction : UserAction
{
    private List<SpawnInfo> spawnedGameObjectsInfo = new List<SpawnInfo>();

    Controller instigator;

    int spawnCount = 0;

    public SpawnAction(List<string> prefabsPaths, List<SpawnTransform> gameObjectPoses, Controller n_instigator)
    {
        instigator = n_instigator;
        for(int i = 0; i < prefabsPaths.Count; i++)
        {
            SpawnInfo spawnInfo = new SpawnInfo();
            spawnInfo.prefabPath = prefabsPaths[i];
            spawnInfo.spawnTransform = gameObjectPoses[i];
            spawnInfo.spawnedGameObject = null;
            spawnedGameObjectsInfo.Add(spawnInfo);
        }
    }

    public override void Do()
    {
        foreach(SpawnInfo spawnInfo in spawnedGameObjectsInfo)
        {
            if(spawnInfo.spawnedGameObject == null)
            {
                Addressables.InstantiateAsync(spawnInfo.prefabPath, spawnInfo.spawnTransform.location, spawnInfo.spawnTransform.rotation)
                .Completed += (handle) => 
                {
                    if (handle.Status == AsyncOperationStatus.Succeeded)
                    {
                        spawnCount++;
                        spawnInfo.spawnedGameObject = handle.Result;
                        spawnInfo.spawnedGameObject.transform.localScale = spawnInfo.spawnTransform.scale;

                        if(instigator != null) // we are spawning
                        {
                            VRManager.InteractDraggable(handle.Result, instigator);
                        }
                        else // we are duplicating
                        {
                            if(spawnCount == spawnedGameObjectsInfo.Count)
                            {
                                List<Interactable> newInteractables = new List<Interactable>();
                                foreach(SpawnInfo spawnInfoL in spawnedGameObjectsInfo)
                                {
                                    Interactable interactable = spawnInfoL.spawnedGameObject.GetComponent<Interactable>();
                                    if(interactable)
                                    {
                                        // if we are duplicating we cant interact
                                        interactable.canBeInteractedThroughState = false;
                                        newInteractables.Add(interactable);
                                    }
                                }

                                SelectionManager.ReplaceAllSelected(newInteractables);
                            }
                        }

                        if(spawnInfo.spawnedGameObject)
                        {
                            Actor actor = spawnInfo.spawnedGameObject.GetComponent<Actor>();
                            if (actor)
                            {
                                ActorsManager.AddActor(actor);
                            }
                        }
                    }
                };
            }
            else
            {
                spawnInfo.spawnedGameObject.SetActive(true);
            }
        }
    }

    public override void Undo()
    {
        foreach(SpawnInfo spawnInfo in spawnedGameObjectsInfo)
        {
            Actor spawnActor = spawnInfo.spawnedGameObject.GetComponent<Actor>();
            if(spawnActor)
            {
                ActorsManager.RemoveActor(spawnActor);
            }
            spawnInfo.spawnedGameObject.SetActive(false);
            SelectionManager.UnselectCurrents();
        }
    }
}