using UnityEngine;

public class Scaleable : MonoBehaviour
{
    public void ScaleTo(Vector3 newScale)
    {
        SetScale(newScale);
    }

    public void ScaleBy(Vector3 deltaScale)
    {
        SetScale(transform.localScale += deltaScale);
    }

    private void SetScale(Vector3 newScale)
    {
        transform.localScale = newScale;
    }
}
