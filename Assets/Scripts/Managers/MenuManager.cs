using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    private static MenuManager _instance;
    public static MenuManager Instance { get { return _instance; } }

    [Header("General")]
    [SerializeField]
    GameObject[] pages;

    [SerializeField]
    int startingPage = 0;

    [Header("New Scene")]

    [SerializeField]
    TMP_InputField levelNameInputField;

    [Header("Load Scene")]

    [SerializeField]
    int loadPageIndex = 2;

    [SerializeField]
    GameObject levelButton;

    [SerializeField]
    Button loadLevelButton;

    [SerializeField]
    Transform loadedLevelNamesScrollRoot;

    [SerializeField]
    GameObject noLevelsSavedText;

    // # Private

    // ## General
    int currentPage = -1;

    // ## Load Scene

    string selectedLoadedLevelName;
    LoadedLevelButton lastSelectedLoadedLevelButton = null;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        GoToPage(startingPage);
    }

    public void GoToPage(int newPage)
    {
        for(int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(false);
        }

        pages[newPage].SetActive(true);
        currentPage = newPage;
    }

    public void OpenLoadScreen()
    {
        if(loadLevelButton)
        {
            loadLevelButton.interactable = false;
        }

        foreach(Transform scrollRootChild in loadedLevelNamesScrollRoot)
        {
            Destroy(scrollRootChild.gameObject);    
        }

        GoToPage(loadPageIndex);
        string[] levelNames = SaveAndLoadManager.LoadLevelNames();
        if(levelNames == null) return;
        noLevelsSavedText.SetActive(false);
        selectedLoadedLevelName = "";
        foreach(string levelName in levelNames)
        {
            GameObject levelButtonGameobject = Instantiate(levelButton);
            if(levelButtonGameobject)
            {
                levelButtonGameobject.transform.SetParent(loadedLevelNamesScrollRoot);
                LoadedLevelButton loadedLevelButton = levelButtonGameobject.GetComponent<LoadedLevelButton>();
                if(loadedLevelButton)
                {
                    loadedLevelButton.SetLevelName(levelName);
                }

                RectTransform rect = levelButtonGameobject.GetComponent<RectTransform>();
                if (rect)
                {
                    rect.anchoredPosition3D = new Vector3(0,0,0);
                    rect.localScale = new Vector3(1, 1, 1);
                }
            }
        }

        if(levelNames.Length == 0)
        {
            noLevelsSavedText.SetActive(true);
        }
    }

    public void ActuallyQuit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else         
            Application.Quit();
        #endif  
    }
    
    public void Quit()
    {
        ModalManager.OpenModal("Are you sure you want to quit the application?", "Cancel", "Confirm", ActuallyQuit, false);
    }

    public void GoToNewScene()
    {
        SaveAndLoadManager.OpenLevel(levelNameInputField.text, false);
    }

    public static void SetSelectedLoadedLevelName(LoadedLevelButton selectedLoadedLevelButton, string levelName)
    {
        // if same level return
        if(Instance.selectedLoadedLevelName == levelName) return;

        if(Instance.loadLevelButton)
        {
            Instance.loadLevelButton.interactable = true;
        }

        selectedLoadedLevelButton.SetButtonColor(ColorManager.GetHighlightColor());
        Instance.selectedLoadedLevelName = levelName;
        if(Instance.lastSelectedLoadedLevelButton)
        {
            Instance.lastSelectedLoadedLevelButton.SetButtonColor(ColorManager.GetNeutralColor());
        }
        Instance.lastSelectedLoadedLevelButton = selectedLoadedLevelButton;
    }

    public void ActuallyLoadScene()
    {
        SaveAndLoadManager.OpenLevel(selectedLoadedLevelName, true);
    }

    public static void LoadScene()
    {
        ModalManager.OpenModal("Open scene " + Instance.selectedLoadedLevelName + "?", "Close", "Confirm", Instance.ActuallyLoadScene,
            false);
    }
}
