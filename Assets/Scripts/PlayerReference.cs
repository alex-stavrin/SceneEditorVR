using UnityEngine;

public class PlayerReference : MonoBehaviour
{
    private void Awake()
    {
        Destroy(gameObject);
    }
}
