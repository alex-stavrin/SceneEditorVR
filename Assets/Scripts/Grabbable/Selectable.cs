using UnityEngine;

public class Selectable : MonoBehaviour
{
    [SerializeField] GameObject recticle;

    Renderer renderer;

    private void Start()
    {
        if(recticle)
        {
            recticle.SetActive(false);


        }
    }

    public void StartSelect()
    {

    }

    public void StopSelect()
    {
    }

    public void StartHover()
    {
        recticle.SetActive(true);
    }

    public void StopHover()
    {
        recticle.SetActive(false);
    }
}
