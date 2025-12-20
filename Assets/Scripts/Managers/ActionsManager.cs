using System.Collections.Generic;
using UnityEngine;

public class ActionsManager : MonoBehaviour
{
    public static ActionsManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void DuplicateSelected()
    {
        if(SelectionManager.GetSelected().Count > 0)
        {
            List<Interactable> tempList = new List<Interactable>();
            foreach(Interactable interactable in SelectionManager.GetSelected())
            {
                GameObject newObject = Instantiate(interactable.gameObject);
                if(newObject)
                {
                    Interactable newInteractable = newObject.GetComponent<Interactable>();
                    tempList.Add(newInteractable);
                }
            }          
            SelectionManager.UnselectCurrents();
            SelectionManager.ReplaceAllSelected(tempList);
        }
    }

    public void DeleteSelected()
    {
        SelectionManager.UnselectAndDestroyCurrents();
    }
}