using UnityEngine;

public class Selectable : MonoBehaviour
{
    [SerializeField] GameObject selectableCanvas;

    private void Start()
    {
        selectableCanvas.SetActive(false);
    }

    public void StartSelect()
    {
        selectableCanvas.SetActive(true);
    }

    public void StopSelect()
    {
        selectableCanvas.SetActive(false);
    }
}
