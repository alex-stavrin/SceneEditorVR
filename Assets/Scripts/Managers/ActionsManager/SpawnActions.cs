using System.Collections.Generic;
using UnityEngine;

public class SpawnInfo
{
    public GameObject gameObjectPrefab;
    public GameObject spawnedGameObject;
    public Pose spawnPose;
}

public class SpawnAction : UserAction
{
    private List<SpawnInfo> spawnedGameObjectsInfo = new List<SpawnInfo>();

    public SpawnAction(List<GameObject> gameObjectPrefabs, List<Pose> gameObjectPoses)
    {
        for(int i = 0; i < gameObjectPrefabs.Count; i++)
        {
            SpawnInfo spawnInfo = new SpawnInfo();
            spawnInfo.gameObjectPrefab = gameObjectPrefabs[i];
            spawnInfo.spawnPose = gameObjectPoses[i];
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
                spawnInfo.spawnedGameObject = Object.Instantiate(spawnInfo.gameObjectPrefab, spawnInfo.spawnPose.position,
                    spawnInfo.spawnPose.rotation);
            }
            else
            {
                spawnInfo.spawnedGameObject.SetActive(true);
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
    }

    public override void Undo()
    {
        foreach(SpawnInfo spawnInfo in spawnedGameObjectsInfo)
        {
            spawnInfo.spawnedGameObject.SetActive(false);
            SelectionManager.UnselectCurrents();
        }
    }

    public List<GameObject> GetSpawned()
    {
        List<GameObject> spawneds = new List<GameObject>();
        foreach(SpawnInfo spawnInfo in spawnedGameObjectsInfo)
        {
            spawneds.Add(spawnInfo.spawnedGameObject);
        }
        return spawneds;
    }
}