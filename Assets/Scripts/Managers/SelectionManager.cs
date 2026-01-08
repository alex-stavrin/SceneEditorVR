using System;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; private set; }

    List<Interactable> selected = new List<Interactable>();

    public event Action<Interactable, List<Interactable>> OnSelectedAdded;

    public event Action<Interactable> OnReplaced;

    public event Action OnUnSelected;

    public event Action OnSelectedChanged;

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public static void AddSelectable(Interactable newSelectable, Controller instigator)
    {
        newSelectable.StartSelect(instigator);
        Instance?.selected.Add(newSelectable);
        Instance?.OnSelectedAdded.Invoke(newSelectable, Instance?.selected);
        Instance.SelectedChanged();
    }

    public static void ReplaceSelectableWithOne(Interactable newSelectable, Controller instigator)
    {
        UnselectCurrents();
        AddSelectable(newSelectable, instigator);
        Instance?.OnReplaced?.Invoke(newSelectable);
    }

    public static void UnselectCurrents()
    {
        foreach(Interactable interactable in Instance?.selected)
        {            
            interactable.StopSelect(null);
        }
        Instance.SelectedChanged();
        Instance?.OnUnSelected.Invoke();
        Instance?.selected.Clear();
    }

    public static void UnselectInteractable(Interactable interactableToUnselect)
    {
        interactableToUnselect.StopSelect(null);
        Instance.SelectedChanged();
        Instance?.selected.Remove(interactableToUnselect);
        Instance?.OnUnSelected.Invoke();
    }

    private void SelectedChanged()
    {
        if(selected.Count == 1)
        {            
            InspectorManager.SetInspected(selected[0].gameObject);
        }
        else
        {
            InspectorManager.SetInspected(null);
        }
        OnSelectedChanged?.Invoke();
    }

    // Returns the list of selected interactables
    public static List<Interactable> GetSelectedInteractables()
    {
        return Instance.selected;
    }

    // Because we have a List of Interactables this returns a List of the Gameobject owners
    // of these interactable components
    public static List<GameObject> GetSelectedGameobjects()
    {
        List<GameObject> temp = new List<GameObject>();
        foreach(Interactable interactable in Instance.selected)
        {
            temp.Add(interactable.gameObject);
        }
        return temp;
    }

    // Replace the entire Interactables List with another one
    // Events are called because we do AddSelectable()
    public static void ReplaceAllSelected(List<Interactable> newSelectedList)
    {
        UnselectCurrents();
        foreach(Interactable interactable in newSelectedList)
        {
            AddSelectable(interactable, null);
        }
    }
}
