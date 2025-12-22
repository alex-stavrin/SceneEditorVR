using System.Collections.Generic;
using UnityEngine;

public class ActionsManager : MonoBehaviour
{
    public static ActionsManager Instance { get; private set; }

    Stack<UserAction> undoStack = new Stack<UserAction>();
    Stack<UserAction> redoStack = new Stack<UserAction>();

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

    public static void ExecuteAction(UserAction userAction)
    {
        userAction.Do();
        AddAction(userAction);
    }

    public static void AddAction(UserAction userAction)
    {
        Instance.undoStack.Push(userAction);
        Instance.redoStack.Clear();       
    }

    public static void Undo()
    {
        UserAction poppedAction = Instance.undoStack.Pop();
        poppedAction.Undo();
        Instance.redoStack.Push(poppedAction);
    }

    public static void Redo()
    {
        UserAction poppedAction = Instance.redoStack.Pop();
        poppedAction.Do();
        Instance.undoStack.Push(poppedAction);
    }
}