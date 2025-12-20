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

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Update()
    {
        string selectedList = "";
        foreach(Interactable interactable in selected)
        {
            selectedList += interactable.name + "\n";
        }
        if(selectedList.Length == 0)
        {
            selectedList = "Empty";
        }
    
        VirtualRealityConsole.PrintMessage(selectedList, PrintTypeVRC.Clear);
    }

    public static void AddSelectable(Interactable newSelectable, Controller instigator)
    {
        newSelectable.StartSelect(instigator);

        Instance?.selected.Add(newSelectable);

        Instance?.OnSelectedAdded.Invoke(newSelectable, Instance?.selected);
    }

    public static void ReplaceSelectablesWithOne(Interactable newSelectable, Controller instigator)
    {
        UnselectCurrents();   

        newSelectable.StartSelect(instigator);

        Instance?.selected.Add(newSelectable);

        InspectorManager.Instance.SetInspected(newSelectable.gameObject);

        Instance?.OnReplaced?.Invoke(newSelectable);
        Instance?.OnSelectedAdded?.Invoke(newSelectable, Instance?.selected);
    }

    public List<Interactable> GetCurrentSelectables()
    {
        return selected;
    }

    public static void UnselectCurrents()
    {
        foreach(Interactable interactable in Instance.selected)
        {            
            interactable.StopSelect(null);
        }
        InspectorManager.Instance.SetInspected(null);
        Instance.OnUnSelected.Invoke();
        Instance.selected.Clear();
    }

    public static void UnselectAndDestroyCurrents()
    {
        foreach(Interactable interactable in Instance.selected)
        {            
            interactable.StopSelect(null);
            Destroy(interactable.gameObject);
        }

        InspectorManager.Instance.SetInspected(null);
        Instance.OnUnSelected.Invoke();

        Instance.selected.Clear();      
    }

    public static List<Interactable> GetSelected()
    {
        return Instance.selected;
    }

    public static void ReplaceAllSelected(List<Interactable> newSelectedList)
    {        
        foreach(Interactable interactable in newSelectedList)
        {
            AddSelectable(interactable, null);
        }
    }
}
