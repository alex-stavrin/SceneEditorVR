using System.Collections.Generic;
using UnityEngine;

public class ReplaceAction : UserAction
{
    private List<Interactable> oldSelectables = null;
    private Interactable newSelected = null;

    public ReplaceAction(List<Interactable> n_oldSelectables, Interactable n_newSelected)
    {
        oldSelectables = new List<Interactable>(n_oldSelectables);
        newSelected = n_newSelected;
    }

    public override void Do()
    {
        
        SelectionManager.ReplaceSelectableWithOne(newSelected, null);
    }

    public override void Undo()
    {
        SelectionManager.ReplaceAllSelected(oldSelectables);
    }
}