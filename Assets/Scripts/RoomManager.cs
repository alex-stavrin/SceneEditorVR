using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance { get; private set; }

    [SerializeField] private float ceilingLevel = 5f;
    [SerializeField] BoxCollider roomBound1;
    [SerializeField] BoxCollider roomBound2;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Prevent duplicates
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Optional: persists between scenes
    }

    void Start()
    {
        // Initialization
    }

    void Update()
    {
        // Per-frame logic
    }

    public float GetCeilingLevel()
    {
        return ceilingLevel;
    }

    public BoxCollider GetRoomBound(int index)
    {
        if(index == 0)
        {
            return roomBound1;
        }
        else
        {
            return roomBound2;
        }
    }
}
