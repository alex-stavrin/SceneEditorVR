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
    
    public void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else         
            Application.Quit();
        #endif
    }

    public void GoToScene(string sceneName)
    {
        SaveAndLoadManager.SetCurrentLevelName(levelNameInputField.text);
        SceneManager.LoadScene(sceneName);
    }
}
