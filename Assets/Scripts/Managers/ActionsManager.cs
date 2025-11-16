using UnityEngine;

public class ActionsManager : MonoBehaviour
{
    public static ActionsManager Instance { get; private set; }

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
        if(SelectionManager.Instance.GetCurrentSelectable() != null)
        {            
            GameObject currentSelectedGameobject = SelectionManager.Instance.GetCurrentSelectable().gameObject;
            if (currentSelectedGameobject)
            {
                GameObject duplicatedObject = Instantiate(currentSelectedGameobject);
                if (duplicatedObject)
                {
                    SelectionManager.Instance.UnselectCurrent();
                    Interactable interactable = duplicatedObject.GetComponent<Interactable>();
                    if (interactable)
                    {
                        SelectionManager.Instance.SetCurrentSelectable(interactable, null);
                    }
                }
            }
        }
    }

    public void DeleteSelected()
    {
        if(SelectionManager.Instance.GetCurrentSelectable() != null)
        {            
            GameObject currentSelectedGameobject = SelectionManager.Instance.GetCurrentSelectable().gameObject;
            if (currentSelectedGameobject)
            {
                SelectionManager.Instance.UnselectCurrent();
                Destroy(currentSelectedGameobject);
            }
        }
    }
}