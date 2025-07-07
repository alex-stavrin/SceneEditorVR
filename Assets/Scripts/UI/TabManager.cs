using UnityEngine;

public class TabManager : MonoBehaviour
{
    [SerializeField] GameObject[] tabs;

    int currentTab = 0;

    private void Start()
    {
        foreach(var tab in tabs)
        {
            tab.SetActive(false);
        }

        tabs[0].SetActive(true);
    }

    public void ActivateTab(int tab)
    {
        tabs[currentTab].SetActive(false);
        tabs[tab].SetActive(true);
        currentTab = tab;
    }
}
