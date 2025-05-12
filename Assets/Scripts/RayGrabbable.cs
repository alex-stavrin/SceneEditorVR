using UnityEngine;

public enum GoalOrientation
{
    Floor,
    Wall,
    Ceiling
}

public class RayGrabbable : MonoBehaviour
{

    [HideInInspector] public RayInteractor currentInteractor;
    [SerializeField] GoalOrientation goalOrientation;
    private void Update()
    {
        if (currentInteractor)
        {
            if (goalOrientation == GoalOrientation.Floor)
            {
                transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            }
        }
    }

    public void UpdatePosition(Vector3 newPosition)
    {
        if(goalOrientation == GoalOrientation.Floor)
        {
            float yClamped = Mathf.Clamp(newPosition.y, 0, Mathf.Infinity);
            transform.position = new Vector3(newPosition.x, yClamped, newPosition.z);
        }
    }
}
