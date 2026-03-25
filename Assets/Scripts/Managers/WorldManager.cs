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

    public void ModalMainMenu()
    {
        ModalManager.OpenModal("Go back to Main Menu?", "Cancel", "Confirm", BackToMainMenu, false);
    }

    public static void BackToMainMenu()
    {
        SaveAndLoadManager.Save();
        SceneManager.LoadScene("Menu");
    }

    public static void ModalSave()
    {
        ModalManager.OpenModal("Save scene " + SaveAndLoadManager.GetCurrentLevelName() + "?", "Cancel", "Confirm", SaveAndLoadManager.Save,
            true);
    }

    public void ModalExport()
    {
        ModalManager.OpenModal("Export scene " + SaveAndLoadManager.GetCurrentLevelName() + "?", "Cancel", "Confirm",
            SaveAndLoadManager.ExportScene, true);
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
