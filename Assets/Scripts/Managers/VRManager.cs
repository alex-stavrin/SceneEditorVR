using UnityEngine;

public class VRManager : MonoBehaviour
{
    [SerializeField] public Controller leftController;
    [SerializeField] public Controller rightController;
    public static VRManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
}
