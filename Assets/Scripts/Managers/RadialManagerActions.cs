using UnityEngine;

public class RadialManagerActions : RadialManager
{
    public static RadialManagerActions Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public override void Update()
    {
        base.Update();

        // 2 == duplicate
        // 3 == delete
        bool haveSomethingSelected = SelectionManager.GetSelectedActors().Count >= 1;
        SetRadialActivation(2, haveSomethingSelected);
        SetRadialActivation(3, haveSomethingSelected);

        // 4 == undo
        bool haveSomethingToUndo = ActionsManager.Instance.undoStack.Count > 0;
        SetRadialActivation(4, haveSomethingToUndo);

        // 1 == redo
        bool haveSomethingToRedo = ActionsManager.Instance.redoStack.Count > 0;
        SetRadialActivation(1, haveSomethingToRedo);
    }

    public void Undo()
    {
        ActionsManager.Undo();
    }

    public void Redo()
    {
        ActionsManager.Redo();     
    }

    public void DuplicateSelected()
    {
        ActionsManager.Instance.DuplicateSelected();
    }

    public void Cancel()
    {
        ///
    }
    
    public void DeleteSelected()
    {
        ActionsManager.Instance.DeleteSelected();
    }
}
