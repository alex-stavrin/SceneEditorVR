using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class ActorData
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
    public string name;
    public string path;
}

[System.Serializable]
public class ActorsData
{
    public List<ActorData> actors = new List<ActorData>();
}

public class SaveAndLoadManager : MonoBehaviour
{
    public static SaveAndLoadManager Instance { get; private set; }


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    static public void Save()
    {

        ActorsData actorsData = new ActorsData();

        List<Actor> actors = ActorsManager.GetActors();
        foreach(Actor actor in actors)
        {
            ActorData actorData = new ActorData();
            actorData.position = actor.transform.position;
            actorData.rotation = actor.transform.rotation;
            actorData.scale = actor.transform.localScale;
            actorData.name = actor.GetActorName();
            actorData.path = actor.GetResourcesPath();
            
            actorsData.actors.Add(actorData);
        }

        string json = JsonUtility.ToJson(actorsData, true);
        File.WriteAllText(Application.persistentDataPath + "/level.json", json);
    }

    static public void Load()
    {
        
    }
}
