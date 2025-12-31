using System.Collections.Generic;
using UnityEngine;

public class UnselectAction : UserAction
{
    private List<Interactable> selectedSnapshot = null;

    public UnselectAction(List<Interactable> selected)
    {
        selectedSnapshot = new List<Interactable>(selected);
    }

    public override void Do()
    {
        SelectionManager.UnselectCurrents();
    }

    public override void Undo()
    {
        SelectionManager.ReplaceAllSelected(selectedSnapshot);
    }
}