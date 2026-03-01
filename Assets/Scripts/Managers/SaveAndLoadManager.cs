using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using GLTFast;
using GLTFast.Export;

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
public class LevelData
{
    public bool directionalLightEnabled;
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

    static async public void ExportScene()
    {
        // 1. Collect all root GameObjects in the active scene
        var rootObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();

        // 2. Initialize the export settings
        var exportSettings = new ExportSettings {
            Format = GltfFormat.Binary // Exports as a single .glb file
        };
        
        var gameObjectExportSettings = new GameObjectExportSettings();
        
        // 3. Create the exporter instance
        var export = new GameObjectExport(exportSettings, gameObjectExportSettings);

        // 4. Add the scene content
        export.AddScene(rootObjects, "MyRuntimeScene");

        // 5. Define the save path (e.g., Application.persistentDataPath)
        string path = System.IO.Path.Combine(Application.persistentDataPath, Instance.currentLevelName + ".glb");

        // 6. Save the file and dispose of the exporter
        bool success = await export.SaveToFileAndDispose(path);
    }

    static public void Save()
    {
        string folderPath = Path.Combine(Application.persistentDataPath, "levels");

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        LevelData levelData = new LevelData();
        levelData.directionalLightEnabled = WorldManager.GetDiretionalLightEnabled();
        List<Actor> actors = ActorsManager.GetActors();
        foreach (Actor actor in actors)
        {
            ActorData actorData = new ActorData();
            actorData.position = actor.transform.position;
            actorData.rotation = actor.transform.rotation;
            actorData.scale = actor.transform.localScale;
            actorData.name = actor.GetActorName();
            actorData.path = actor.GetResourcesPath();
            
            levelData.actors.Add(actorData);
        }

        string json = JsonUtility.ToJson(levelData, true);
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

    static public void OpenLevel(string levelName, bool bIsLoaded)
    {
        Instance.currentLevelName = levelName;
        
        SceneManager.sceneLoaded += OnSceneLoaded;
        
        SceneManager.LoadScene("Level");

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;

            if (bIsLoaded)
            {
                LoadAndSpawnLevel(levelName);
            }
        }
    }

    static public void LoadAndSpawnLevel(string levelName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, "levels", levelName + ".json");

        if(!File.Exists(filePath))
        {
            return;
        }

        string jsonText = File.ReadAllText(filePath);
        LevelData levelData = JsonUtility.FromJson<LevelData>(jsonText);

        foreach(ActorData actorData in levelData.actors)
        {
            Addressables.InstantiateAsync(actorData.path, actorData.position, actorData.rotation).Completed += (handle) =>
            {
                if(handle.Status == AsyncOperationStatus.Succeeded)
                {
                    GameObject newGameObject = handle.Result;
                    if(newGameObject)
                    {
                        newGameObject.transform.localScale = actorData.scale;
                        Actor actor = newGameObject.GetComponent<Actor>();
                        if(actor)
                        {
                            ActorsManager.AddActor(actor);
                        }
                        
                        InteractableMoveable interactableMoveable = newGameObject.GetComponent<InteractableMoveable>();
                        if (interactableMoveable)
                        {
                            interactableMoveable.isFirst = false;
                            interactableMoveable.canBeInteractedThroughState = false;
                        }
                    }
                }
            };
        }

        WorldPanel.SetToggleState(levelData.directionalLightEnabled);
    }
}
