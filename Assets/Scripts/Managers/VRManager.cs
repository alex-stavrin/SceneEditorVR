using UnityEngine;

public class VRManager : MonoBehaviour
{
    [SerializeField] public Controller leftController;
    [SerializeField] public Controller rightController;
    public static VRManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    
    public static void InteractDraggable(GameObject gameobject, Controller controller)
    {
        InteractableMoveable interactableMoveableDraggable = gameobject.GetComponent<InteractableMoveable>();
        if(interactableMoveableDraggable)
        {
            controller.StartInteract(interactableMoveableDraggable);
        }
    }
}
