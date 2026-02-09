using System.Collections.Generic;
using System.IO;
using System.Linq;
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

    string currentLevelName;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    static public void Save()
    {
        string folderPath = Path.Combine(Application.persistentDataPath, "levels");

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        ActorsData actorsData = new ActorsData();
        List<Actor> actors = ActorsManager.GetActors();

        foreach (Actor actor in actors)
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
        string filePath = Path.Combine(folderPath, Instance.currentLevelName + ".json");
        File.WriteAllText(filePath, json);
    }

    static public string[] LoadLevelNames()
    {
        string levelsPath = Path.Combine(Application.persistentDataPath, "levels");

        if (Directory.Exists(levelsPath))
        {
            return Directory.GetFiles(levelsPath, "*.json")
                            .Select(Path.GetFileNameWithoutExtension)
                            .ToArray();
        }
        else
        {
            return null;
        }
    }

    static public void SetCurrentLevelName(string newLevelName)
    {
        Instance.currentLevelName = newLevelName;
    }
}
