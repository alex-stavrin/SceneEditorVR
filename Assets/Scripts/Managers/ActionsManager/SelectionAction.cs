using System.Collections.Generic;
using UnityEngine;

public class SelectionAction : UserAction
{

    private Interactable selected = null;

    public SelectionAction(Interactable n_selected)
    {
        selected = n_selected;
    }

    public override void Do()
    {
        // Handled by the selection manager
        SelectionManager.AddSelectable(selected, null);
    }

    public override void Undo()
    {
        SelectionManager.UnselectInteractable(selected);
    }
}