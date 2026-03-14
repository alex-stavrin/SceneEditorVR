using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance { get; private set; }

    bool directionalLightEnabled = true;

    [Header("World")]

    [SerializeField]
    Light directionalLight;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    
    public static void SetDirectionalLightEnabled(bool newDirectionalLightEnabled)
    {
        Instance.directionalLightEnabled = newDirectionalLightEnabled;
        Instance.directionalLight.enabled = newDirectionalLightEnabled;
    }

    public static bool GetDiretionalLightEnabled()
    {
        return Instance.directionalLightEnabled;
    }

    public static void BackToMenu()
    {
        SaveAndLoadManager.Save();
        SceneManager.LoadScene("Menu");
    }

    public static void Save()
    {
        SaveAndLoadManager.Save();
    }

    public static void Export()
    {
        SaveAndLoadManager.ExportScene();
    }

    void OnApplicationQuit()
    {
        SaveAndLoadManager.Save();
    }

    void OnApplicationPause(bool pause)
    {
        if(pause)
        {            
            SaveAndLoadManager.Save();
        }
    }
}
