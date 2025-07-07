using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] pages;

    [SerializeField]
    int startingPage = 0;

    int currentPage = -1;

    private void Start()
    {
        GoToPage(startingPage);
    }

    public void GoToPage(int newPage)
    {
        if(currentPage != -1)
        {
            pages[currentPage].SetActive(false);
        }

        pages[newPage].SetActive(true);
        currentPage = newPage;
    }
}
