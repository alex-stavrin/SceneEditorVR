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
        if(SelectionManager.GetSelectedInteractables().Count > 0)
        {
            List<string> prefabPaths = new List<string>();
            List<Pose> poses = new List<Pose>();
            List<Actor> selectedActors = SelectionManager.GetSelectedActors();
            for(int i = 0; i < SelectionManager.GetSelectedGameobjects().Count; i++)
            {
                prefabPaths.Add(selectedActors[i].GetResourcesPath());
                Transform newTransform = selectedActors[i].gameObject.transform;
                poses.Add(new Pose(newTransform.position, newTransform.rotation));
            }          

            SelectionManager.UnselectCurrents();

            SpawnGameObjects(prefabPaths, poses, null);


        }
    }

    public void DeleteSelected()
    {
        DeleteAction deleteAction = new DeleteAction(SelectionManager.GetSelectedGameobjects());

        // this has to be after making the action, because we are going to lose the selected gameobjects
        SelectionManager.UnselectCurrents();

        ExecuteAndAddAction(deleteAction);
    }

    public static void SpawnGameObjects(List<string> prefabPaths, List<Pose> poses, Controller instigator)
    {
        SpawnAction spawnAction = new SpawnAction(prefabPaths, poses, instigator);
        ExecuteAndAddAction(spawnAction);
    }

    public static void AddSelectableAction(Interactable newSelectable, Controller instigator)
    {
        SelectionAction selectionAction = new SelectionAction(newSelectable);
        ExecuteAndAddAction(selectionAction);       
    }

    public static void ReplaceSelectablesWithOneAction(Interactable newSelectable, Controller instigator)
    {
        ReplaceAction replaceAction = new ReplaceAction(SelectionManager.GetSelectedInteractables(), newSelectable);
        ExecuteAndAddAction(replaceAction);
    }

    public static void UnselectCurrentsAction()
    {
        List<Interactable> selectedInteractables = SelectionManager.GetSelectedInteractables();
        if(selectedInteractables.Count > 0)
        {            
            UnselectAction unselectAction = new UnselectAction(SelectionManager.GetSelectedInteractables());
            ExecuteAndAddAction(unselectAction);
        }
    }

    // === Internal functions of ActionsManager === //

    public static void ExecuteAndAddAction(UserAction userAction)
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
        if(Instance.undoStack.Count <= 0) return;

        UserAction poppedAction = Instance.undoStack.Pop();
        if(poppedAction != null)
        {            
            poppedAction.Undo();
            Instance.redoStack.Push(poppedAction);
        }
    }

    public static void Redo()
    {
        if(Instance.redoStack.Count <= 0) return;

        UserAction poppedAction = Instance.redoStack.Pop();
        if (poppedAction != null)
        {            
            poppedAction.Do();
            Instance.undoStack.Push(poppedAction);
        }
    }


}