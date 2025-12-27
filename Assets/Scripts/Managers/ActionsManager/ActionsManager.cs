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
            List<GameObject> gameObjectPrefabs = new List<GameObject>();
            List<Pose> poses = new List<Pose>();
            for(int i = 0; i < SelectionManager.GetSelectedGameobjects().Count; i++)
            {
                gameObjectPrefabs.Add(SelectionManager.GetSelectedGameobjects()[i]);
                Transform newTransform = SelectionManager.GetSelectedGameobjects()[i].transform;
                poses.Add(new Pose(newTransform.position, newTransform.rotation));
            }          

            SelectionManager.UnselectCurrents();

            List<GameObject> spawnedObjcets = SpawnGameObjects(gameObjectPrefabs, poses);

            List<Interactable> newInteractables = new List<Interactable>();
            foreach(GameObject currentGameObject in spawnedObjcets)
            {
                Interactable interactable = currentGameObject.GetComponent<Interactable>();
                if(interactable)
                {
                    newInteractables.Add(interactable);
                }
            }

            SelectionManager.ReplaceAllSelected(newInteractables);
        }
    }

    public void DeleteSelected()
    {
        DeleteAction deleteAction = new DeleteAction(SelectionManager.GetSelectedGameobjects());

        // this has to be after making the action, because we are going to lose the selected gameobjects
        SelectionManager.UnselectCurrents();

        ExecuteAction(deleteAction);
    }

    public static List<GameObject> SpawnGameObjects(List<GameObject> gameObjectPrefabs, List<Pose> poses)
    {
        SpawnAction spawnAction = new SpawnAction(gameObjectPrefabs, poses);
        ExecuteAction(spawnAction);

        return spawnAction.GetSpawned();
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