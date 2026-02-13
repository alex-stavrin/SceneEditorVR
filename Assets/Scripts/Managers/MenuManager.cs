using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] pages;

    [SerializeField]
    TMP_InputField levelNameInputField;

    [SerializeField]
    int startingPage = 0;

    [SerializeField]
    int loadPageIndex = 2;

    [SerializeField]
    GameObject levelButton;

    [SerializeField]
    Transform loadedLevelNamesScrollRoot;

    int currentPage = -1;

    private void Start()
    {
        GoToPage(startingPage);
    }

    public void GoToPage(int newPage)
    {
        if (currentPage != -1)
        {
            pages[currentPage].SetActive(false);
        }

        pages[newPage].SetActive(true);
        currentPage = newPage;
    }

    public void OpenLoadScreen()
    {
        foreach(Transform scrollRootChild in loadedLevelNamesScrollRoot)
        {
            Destroy(scrollRootChild.gameObject);    
        }

        GoToPage(loadPageIndex);
        string[] levelNames = SaveAndLoadManager.LoadLevelNames();
        if(levelNames == null) return;
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
    }
    
    public void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else         
            Application.Quit();
        #endif
    }

    public void GoToNewScene()
    {
        SaveAndLoadManager.OpenLevel(levelNameInputField.text, false);
    }
}
